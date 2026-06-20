using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerApp
{
    public class Server
    {
        private TcpListener _listener;
        private bool _isRunning;
        private List<ClientHandler> _clients = new List<ClientHandler>();

        // Các sự kiện đồng bộ dữ liệu ra Giao diện Form1
        public event Action<string> OnLogReceived;
        public event Action<int> OnProgressUpdated;

        public void Start(int port)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _isRunning = true;
                OnLogReceived?.Invoke($"Server đã khởi động thành công trên cổng {port}...");

                Thread listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                OnLogReceived?.Invoke($"Lỗi khi khởi động Server: {ex.Message}");
            }
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    OnLogReceived?.Invoke($"Client kết nối thành công từ địa chỉ: {client.Client.RemoteEndPoint}");

                    ClientHandler handler = new ClientHandler(client, this);
                    lock (_clients)
                    {
                        _clients.Add(handler);
                    }

                    Thread clientThread = new Thread(handler.Process);
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch
                {
                    break; // Vòng lặp dừng lại khi Server.Stop() được gọi
                }
            }
        }

        // Đẩy tiến trình phần trăm file nhận được từ ClientHandler ra Form1
        public void UpdateProgress(int percentage)
        {
            OnProgressUpdated?.Invoke(percentage);
        }

        public void RemoveClient(ClientHandler handler)
        {
            lock (_clients)
            {
                _clients.Remove(handler);
            }
            OnLogReceived?.Invoke("Một Client đã ngắt kết nối khỏi hệ thống.");
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            lock (_clients)
            {
                foreach (var client in _clients) client.Close();
                _clients.Clear();
            }
        }
    }
}
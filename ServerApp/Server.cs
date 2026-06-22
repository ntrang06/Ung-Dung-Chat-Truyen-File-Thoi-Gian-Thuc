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

        // Định nghĩa các sự kiện ủy quyền (Delegates) để gửi thông báo ra Form UI
        public event Action<string> OnLogReceived;
        public event Action<string> OnClientConnected;
        public event Action<string> OnClientDisconnected;
        public event Action<string> OnMessageReceived;
        public event Action<int> OnProgressUpdated;
        public event Action<string, string, string> OnFileReceived;

        public void Start(int port)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _isRunning = true;
                OnLogReceived?.Invoke($"Máy chủ bắt đầu lắng nghe trên cổng {port}...");

                Thread listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                OnLogReceived?.Invoke($"Lỗi khi khởi động cổng Socket: {ex.Message}");
            }
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    string ep = client.Client.RemoteEndPoint.ToString();

                    OnLogReceived?.Invoke($"Client kết nối thành công: {ep}");
                    OnClientConnected?.Invoke(ep);

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
                    break;
                }
            }
        }

        public void BroadcastMessage(string message)
        {
            lock (_clients)
            {
                foreach (var client in _clients)
                {
                    client.SendMessage(message);
                }
            }
        }

        public void InvokeMessageReceived(string msg) => OnMessageReceived?.Invoke(msg);
        public void InvokeProgressUpdated(int percent) => OnProgressUpdated?.Invoke(percent);
        public void InvokeFileReceived(string name, string size, string status) => OnFileReceived?.Invoke(name, size, status);

        public void RemoveClient(ClientHandler handler, string endPoint)
        {
            lock (_clients)
            {
                _clients.Remove(handler);
            }
            OnClientDisconnected?.Invoke(endPoint);
            OnLogReceived?.Invoke($"Client {endPoint} đã ngắt kết nối.");
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
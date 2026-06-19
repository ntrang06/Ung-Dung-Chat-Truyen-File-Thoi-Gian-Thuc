using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerApp
{
    class Server
    {
        private TcpListener listener;
        private bool isRunning = false;

        public Action<string> OnMessageReceived;

        public void StartServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            isRunning = true;

            Thread thread = new Thread(ListenClients);
            thread.IsBackground = true;
            thread.Start();

            OnMessageReceived?.Invoke("Server đã khởi động...");
        }

        private void ListenClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    OnMessageReceived?.Invoke("Client đã kết nối.");

                    Thread clientThread =
                        new Thread(() => HandleClient(client));

                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch
                {
                    break;
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[4096];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break;

                    string message =
                        Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    OnMessageReceived?.Invoke(message);
                }
                catch
                {
                    break;
                }
            }

            client.Close();

            OnMessageReceived?.Invoke("Client đã ngắt kết nối.");
        }
    }
}
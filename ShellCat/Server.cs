using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ShellCat
{

    #region Server

    public struct ClientInfo
    {
        public string Name;
        public NetworkStream Stream;
    }

    public class Server
    {
        private readonly List<RemoteClient> _clientList = new List<RemoteClient>();
        public int ListenPort;
        private readonly MainForm _mainForm;
        private TcpListener _listener;

        public Server(int listenPort, MainForm mainForm)
        {
            _mainForm = mainForm;
            ListenPort = listenPort;
        }

        private void WriteLog(string content)
        {
            Utils.WriteServerLog(_mainForm.RtbServerStatus, content);
        }

        public void RemoveClient(RemoteClient client)
        {
            lock (_clientList)
            {
                _clientList.Remove(client);
            }
        }

        public bool RunListen()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, ListenPort);
                _listener.Start(); // 开启对控制端口的侦听
                WriteLog($"Server is running on 0.0.0.0:{ListenPort} ...");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RunServer()
        {
            while (true)
            {
                // 获取一个连接，同步方法，在此处中断
                var client = _listener.AcceptTcpClient();
                var remoteClient = new RemoteClient(client, _mainForm, this);
                _clientList.Add(remoteClient);
                remoteClient.BeginRead();
            }
        }


        public void StopServer()
        {
            _listener.Stop();
            foreach (var client in _clientList)
            {
                client.Dispose();
            }
            WriteLog($"Server stop ...");
        }
    }

    #endregion

    #region RemoteClient

    public class RemoteClient
    {
        private const int BufferSize = 8192;
        private readonly MainForm _mainForm;
        private readonly byte[] _buffer;
        private readonly TcpClient _client;
        private ClientInfo _clientInfo;
        private readonly ShellTab _shellTab;
        private readonly string _remoteEndPoint;
        private readonly Server _server;

        #region RemoteClient

        public RemoteClient(TcpClient client, MainForm mainForm, Server server)
        {
            _client = client;
            _server = server;
            _mainForm = mainForm;
            Utils.AddIpListView(_mainForm.ListViewIp, client.Client.RemoteEndPoint.ToString());
            // 打印连接到的客户端信息
            WriteLog($"Client Connected! {client.Client.LocalEndPoint} <-- {client.Client.RemoteEndPoint}");
            _remoteEndPoint = client.Client.RemoteEndPoint.ToString();
            _shellTab = Utils.AddShellTab(_mainForm,
                this, client.Client.RemoteEndPoint.ToString());
            _shellTab.AppendText($"Client Connected {DateTime.Now.ToString()}\r\n");

            // 获得流
            try
            {
                _clientInfo.Stream = this._client.GetStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            _buffer = new byte[BufferSize];
        }

        #endregion

        private void WriteLog(string content)
        {
            Utils.WriteServerLog(_mainForm.RtbServerStatus, content);
        }

        #region BeginRead

        // 开始进行读取
        public void BeginRead()
        {
            var callBack = new AsyncCallback(OnReadComplete);
            try
            {
                _clientInfo.Stream.BeginRead(_buffer, 0, BufferSize, callBack, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        private void OnLostConnection()
        {
            _server.RemoveClient(this);
            Dispose();
            _shellTab.AppendText("\r\n++++++++CONNECTION LOST++++++++\r\n");
            WriteLog($"Remote {_remoteEndPoint} lost connection");
            _shellTab.ConnectionLost = true;
            Utils.RemoveIpListView(_mainForm.ListViewIp, _remoteEndPoint);
            _mainForm.RemoveCachedTab(_remoteEndPoint);
        }

        #region OnReadComplete

        // 在读取完成时进行回调
        private void OnReadComplete(IAsyncResult ar)
        {
            try
            {
                var bytesRead = 0;
                lock (_clientInfo.Stream)
                {
                    bytesRead = _clientInfo.Stream.EndRead(ar);
                }
                if (bytesRead == 0)
                {
                    OnLostConnection();
                    return;
                }

                var msg = Encoding.UTF8.GetString(_buffer, 0, bytesRead);
                WriteLog($"Recv From {_remoteEndPoint}");
                WriteLog(msg);
                _shellTab.AppendText(msg);

                Array.Clear(_buffer, 0, _buffer.Length); // 清空缓存，避免脏读

                // 再次调用BeginRead()，完成时调用自身，形成无限循环
                lock (_clientInfo.Stream)
                {
                    BeginRead();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message); // 捕获异常时退出程序
                OnLostConnection();
            }
        }

        #endregion

        #region SendMessage

        // 发送消息到客户端
        public void SendMessage(string msg)
        {
            if (!_client.Connected)
            {
                _shellTab.ConnectionLost = true;
                return;
            }

            var stream = _clientInfo.Stream;
            WriteLog($"Sent To {_remoteEndPoint}");
            var temp = Encoding.UTF8.GetBytes(msg); // 获得缓存
            try
            {
                lock (stream)
                {
                    stream.Write(temp, 0, temp.Length); // 发往客户端
                }

                WriteLog(msg);
            }
            catch (Exception ex)
            {
                WriteLog("+++++");
                WriteLog(ex.Message);
            }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            try
            {
                if (_clientInfo.Stream != null)
                    _clientInfo.Stream.Dispose();
                _client?.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }

    #endregion
}
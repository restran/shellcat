using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using static ShellCat.Utils;

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
        public List<RemoteClient> ClientList = new List<RemoteClient>();
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
            lock (ClientList)
            {
                client.StopConnection();
                ClientList.Remove(client);
            }
        }

        public void RemoveClient(string removeEndpoint)
        {
            lock (ClientList)
            {
                for (var i = 0; i < ClientList.Count; i++)
                {
                    var item = ClientList[i];
                    if (item._remoteEndPoint.Equals(removeEndpoint))
                    {
                        RemoveClient(item);
                        break;
                    }
                }
            }
        }

        public void SendMessageToAllClient(string cmd)
        {
            lock (ClientList)
            {
                foreach (var item in ClientList)
                {
                    item.SendMessage(cmd);
                }
            }
        }

        public void SendMessageToClient(string remoteEndpoint, string cmd)
        {
            lock (ClientList)
            {
                foreach (var item in ClientList)
                {
                    if (item._remoteEndPoint.Equals(remoteEndpoint))
                    {
                        item.SendMessage(cmd);
                    }
                }
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
                // 如果勾选，一个IP只保留一个，则自动判断
                var sameIpExists = false;
                if (_mainForm.ckbKeepOne.Checked)
                {
                    lock (ClientList)
                    {                 
                        foreach (var item in ClientList)
                        {
                            var remoteIp = client.Client.RemoteEndPoint.ToString().Split(':')[0];
                            var itemIp = item._remoteEndPoint.Split(':')[0];
                            if (itemIp.Equals(remoteIp))
                            {
                                WriteLog($"This IP has exists! Skip this connection. {client.Client.RemoteEndPoint}");
                                sameIpExists = true;
                                break;
                            }
                        }
                    }
                }
                if (!sameIpExists)
                {
                    var remoteClient = new RemoteClient(client, _mainForm, this);
                    ClientList.Add(remoteClient);
                    remoteClient.BeginRead();
                }
                else
                {
                    // 断开连接
                    try
                    {
                        client?.Close();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        public void StopServer()
        {
            _listener.Stop();
            foreach (var client in ClientList)
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
        private readonly TabTextItem _shellTab;
        public readonly string _remoteEndPoint;
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
            string initMsg = $"Client Connected {DateTime.Now.ToString()}\r\n";
            this._shellTab = _mainForm.AddShellTab(this, initMsg, client.Client.RemoteEndPoint.ToString());
            //_shellTab.AppendText($"Client Connected {DateTime.Now.ToString()}\r\n");

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

        public void StopConnection()
        {
            Dispose();
            WriteLog($"Stop connection {_remoteEndPoint}");
        }

        private void OnLostConnection()
        {
            try
            {
                _server.RemoveClient(this);
                Dispose();
                _shellTab.AppendText("\r\n++++++++CONNECTION LOST++++++++\r\n");
                WriteLog($"Remote {_remoteEndPoint} lost connection");
                _shellTab.ConnectionLost();
                Utils.RemoveIpListView(_mainForm.ListViewIp, _remoteEndPoint);
                lock (_mainForm._lockObject)
                {
                    if (_mainForm._showingBatchCmdForm && _mainForm._batchCmdForm != null)
                    {
                        Utils.RemoveIpListView(_mainForm._batchCmdForm.lvwIP, _remoteEndPoint, 1);
                    }
                }
                _mainForm.RemoveCachedTab(_remoteEndPoint);
            }
            catch (Exception e)
            {
                WriteLog(e.Message);
            }
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

                lock (_mainForm._lockObject)
                {
                    if (_mainForm._showingBatchCmdForm && !_mainForm._batchCmdForm.IsDisposed)
                    {
                        _mainForm._batchCmdForm.AppendOutputText("......................\n");
                        _mainForm._batchCmdForm.AppendOutputText($"{_remoteEndPoint}\n");
                        _mainForm._batchCmdForm.AppendOutputText(msg);
                        _mainForm._batchCmdForm.AppendOutputText("\n");
                    }
                }

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
            try
            {
                if (!_client.Connected)
                {
                    _shellTab.ConnectionLost();
                    return;
                }

                var stream = _clientInfo.Stream;
                WriteLog($"Sent To {_remoteEndPoint}");
                var temp = Encoding.UTF8.GetBytes(msg); // 获得缓存
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
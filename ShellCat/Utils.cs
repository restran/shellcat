using System;
using System.Windows.Forms;

namespace ShellCat
{
    public class Utils
    {
        public class TabTextItem
        {
            public string title = "";
            public string text = "";
            public RemoteClient client;
            public ShellTab shellTab;

            public void ConnectionLost()
            {
                if (shellTab != null && !shellTab.IsDisposed)
                {
                    shellTab.ConnectionLost = true;
                }
            }

            public ShellTab AddShellTab()
            {
                shellTab = new ShellTab(this.client)
                {
                    Dock = DockStyle.Fill
                };

                shellTab.AppendText(text);
                return shellTab;
            }

            public void AppendText (string t)
            {
                this.text += t;
                if (shellTab != null && !shellTab.IsDisposed)
                {
                    shellTab.AppendText(t);
                }
            }
        }

        public static void WriteServerLog(RichTextBox textBox, string content)
        {
            // 对于该控件的请求来自于创建该控件所在线程以外的线程
            if (textBox.InvokeRequired)
            {
                var rtbSet = new DelegateRichTextBox(delegate(RichTextBox tb, string cnt)
                {
                    tb.AppendText(cnt + "\r\n");
                    tb.ScrollToCaret(); //让滚动条拉到最底处   
                });
                textBox.Invoke(rtbSet, textBox, content);
            }
            else
            {
                textBox.AppendText(content + "\r\n");
                textBox.ScrollToCaret(); //让滚动条拉到最底处   
            }
        }

        public static void AddIpListView(ListView lvw, string ip)
        {
            // 对于该控件的请求来自于创建该控件所在线程以外的线程
            if (lvw.InvokeRequired)
            {
                var lvwSet = new DelegateIpListView(delegate(ListView _lvw, string _ip)
                {
                    var remote = new ListViewItem(_ip);
                    remote.SubItems.Add(DateTime.Now.ToShortTimeString());
                    _lvw.Items.Add(remote);
                    _lvw.Columns[0].Text = $"IP ({_lvw.Items.Count})";
                });
                lvw.Invoke(lvwSet, lvw, ip);
            }
            else
            {
                var remote = new ListViewItem(ip);
                remote.SubItems.Add(DateTime.Now.ToShortTimeString());
                lvw.Items.Add(remote);
                lvw.Columns[0].Text = $"IP ({lvw.Items.Count})";
            }
        }

        public static void RemoveIpListView(ListView lvw, string ip, int textIndex=0)
        {
            // 对于该控件的请求来自于创建该控件所在线程以外的线程
            if (lvw.InvokeRequired)
            {
                var lvwSet = new DelegateIpListView(delegate(ListView _lvw, string _ip)
                {
                    //var remote = new ListViewItem(_ip);
                    //remote.SubItems.Add(DateTime.Now.ToShortTimeString());
                    for (var i = 0; i < _lvw.Items.Count; i++)
                    {
                        if (_lvw.Items[i].SubItems[textIndex].Text.Equals(ip))
                        {
                            _lvw.Items.RemoveAt(i);
                            break;
                        }
                    }
                    _lvw.Columns[0].Text = $"IP ({_lvw.Items.Count})";
                });
                lvw.Invoke(lvwSet, lvw, ip);
            }
            else
            {
                //var remote = new ListViewItem(ip);
                //remote.SubItems.Add(DateTime.Now.ToShortTimeString());
                for (var i = 0; i < lvw.Items.Count; i++)
                {
                    if (lvw.Items[i].SubItems[textIndex].Equals(ip))
                    {
                        lvw.Items.RemoveAt(i);
                        break;
                    }
                }
                lvw.Columns[0].Text = $"IP ({lvw.Items.Count})";
            }
        }

        private delegate void DelegateRichTextBox(RichTextBox textBox, string content);

        private delegate void DelegateIpListView(ListView lvw, string ip);

        private delegate void DelegateTabControl(TabControl tabControl, RemoteClient client, string ip);
    }
}
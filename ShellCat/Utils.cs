using System;
using System.Windows.Forms;

namespace ShellCat
{
    public class Utils
    {
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

        public static void RemoveIpListView(ListView lvw, string ip)
        {
            // 对于该控件的请求来自于创建该控件所在线程以外的线程
            if (lvw.InvokeRequired)
            {
                var lvwSet = new DelegateIpListView(delegate(ListView _lvw, string _ip)
                {
                    var remote = new ListViewItem(_ip);
                    remote.SubItems.Add(DateTime.Now.ToShortTimeString());
                    for (var i = 0; i < _lvw.Items.Count; i++)
                    {
                        if (_lvw.Items[i].Text.Equals(ip))
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
                var remote = new ListViewItem(ip);
                remote.SubItems.Add(DateTime.Now.ToShortTimeString());
                for (var i = 0; i < lvw.Items.Count; i++)
                {
                    if (lvw.Items[i].Text.Equals(ip))
                    {
                        lvw.Items.RemoveAt(i);
                        break;
                    }
                }
                lvw.Columns[0].Text = $"IP ({lvw.Items.Count})";
            }
        }

        public static ShellTab AddShellTab(MainForm mainForm, RemoteClient client, string ip)
        {
            ShellTab tab = null;
            var tabControl = mainForm.TabControlShell;
            // 对于该控件的请求来自于创建该控件所在线程以外的线程
            if (tabControl.InvokeRequired)
            {
                var control = new DelegateTabControl(delegate (TabControl _tb, RemoteClient _client, string _ip)
                {
                    tab = new ShellTab(_client)
                    {
                        Dock = DockStyle.Fill
                    };
                    var tp = new TabPage {Text = _ip};
                    tp.Controls.Add(tab);
  
                    mainForm.AddCachedTab(tp);
                    mainForm.ShowShellTab(_ip);
                });
                tabControl.Invoke(control, tabControl, client, ip);
            }
            else
            {
                var tp = new TabPage { Dock = DockStyle.Fill };
                tab = new ShellTab(client)
                {
                    Dock = DockStyle.Fill
                };
                tp.Controls.Add(tab);
                tp.Text = ip;
                tabControl.TabPages.Add(tp);
            }

            return tab;
        }

        //public static void RemoveShellTab(TabControl tabControl, RemoteClient client, string ip)
        //{
        //    // 对于该控件的请求来自于创建该控件所在线程以外的线程
        //    if (tabControl.InvokeRequired)
        //    {
        //        var control = new DelegateTabControl(delegate (TabControl _tb, RemoteClient _client, string _ip)
        //        {
        //            for (var i = 0; i < _tb.TabPages.Count; i++)
        //            {
        //                if (_tb.TabPages[i].Text.Equals(ip))
        //                {
        //                    _tb.TabPages.RemoveAt(i);
        //                    break;
        //                }
        //            }
        //        });
        //        tabControl.Invoke(control, tabControl, client, ip);
        //    }
        //    else
        //    {
        //        for (var i = 0; i < tabControl.TabPages.Count; i++)
        //        {
        //            if (tabControl.TabPages[i].Text.Equals(ip))
        //            {
        //                tabControl.TabPages.RemoveAt(i);
        //                break;
        //            }
        //        }
        //    }
        //}

        private delegate void DelegateRichTextBox(RichTextBox textBox, string content);

        private delegate void DelegateIpListView(ListView lvw, string ip);

        private delegate void DelegateTabControl(TabControl tabControl, RemoteClient client, string ip);
    }
}
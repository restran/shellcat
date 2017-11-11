using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ShellCat
{
    public partial class MainForm : Form
    {
        public readonly Server _server;
        public BatchCmdForm _batchCmdForm;
        public Object _lockObject = new object();
        public bool _showingBatchCmdForm = false;
        private Thread _threadService;
        public List<TabPage> CachedTabList = new List<TabPage>();

        public void AddCachedTab(TabPage tp)
        {
            lock (CachedTabList)
            {
                var found = CachedTabList.Any(tabPage => tp.Text.Equals(tabPage.Text));

                if (found == false)
                {
                    CachedTabList.Add(tp);
                }
            }
        }

        public void RemoveCachedTab(string ip)
        {
            lock (CachedTabList)
            {
                for (var i = 0; i < CachedTabList.Count; i++)
                {
                    if (CachedTabList[i].Text.Equals(ip))
                    {
                        CachedTabList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public RichTextBox RtbServerStatus
        {
            set { rtbServerStatus = value; }
            get { return rtbServerStatus; }
        }

        public ListView ListViewIp
        {
            set { lvwIP = value; }
            get { return lvwIP; }
        }

        public TabControl TabControlShell
        {
            set { tabControl = value; }
            get { return tabControl; }
        }

        public MainForm()
        {
            InitializeComponent();
            _server = new Server(Convert.ToInt32(tbPort.Text), this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text.Equals("Start"))
            {
                var success = true;
                try
                {
                    int port = Convert.ToInt32(tbPort.Text);
                    _server.ListenPort = port;
                    success = _server.RunListen();
                }
                catch (Exception)
                {
                    success = false;
                }

                if (success == false)
                {
                    MessageBox.Show($"Fail to run server on port {tbPort.Text}");
                    return;
                }
           
                this._threadService = new Thread(this._server.RunServer);
                this._threadService.Start();
                this.btnStart.Text = @"Stop";
            }
            else
            {
                try
                {
                    this._threadService.Abort();
                    this._threadService.Join(500);
                    this._server.StopServer();
                    if (this._batchCmdForm != null)
                    {
                        
                    }
                }
                catch (Exception ex)
                {
                    Utils.WriteServerLog(rtbServerStatus, ex.StackTrace);
                }

                this.btnStart.Text = @"Start";
            }
        }

        /// <summary>
        /// 关闭窗体，需要关闭后台服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_threadService != null)
                {
                    this._threadService.Abort();
                    this._threadService.Join(500);
                }

                this._server.StopServer();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void rtbServerStatus_TextChanged(object sender, EventArgs e)
        {
        }

        public void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    {
                        tabControl.SelectedTab = tabControl.TabPages[i];
                    }
                }

                if (tabControl.SelectedTab.Text != @"Server Status")
                {
                    this.contextMenuStrip.Show(tabControl, e.X, e.Y);
                }
            }
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < tabControl.TabPages.Count; i++)
            {
                if (tabControl.TabPages[i].Text.Equals(tabControl.SelectedTab.Text))
                {
                    tabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        public void ShowShellTab(string ip)
        {
            var found = false;
            for (var i = 0; i < tabControl.TabPages.Count; i++)
            {
                if (ip.Equals(tabControl.TabPages[i].Text))
                {
                    tabControl.SelectedIndex = i;
                    found = true;
                }
            }

            if (found == false)
            {
                lock (CachedTabList)
                {
                    foreach (var t in CachedTabList)
                    {
                        if (ip.Equals(t.Text))
                        {
                            tabControl.TabPages.Add(t);
                            tabControl.SelectedIndex = tabControl.TabCount - 1;
                        }
                    }
                }
            }
        }

        private void lvwIP_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = ((ListView) sender).SelectedItems[0];
            string ip = item.Text;
            ShowShellTab(ip);
        }

        private void closeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwIP.SelectedItems.Count <= 0)
            {
                return;
            }

            var selectedItem = lvwIP.SelectedItems[0];
            this._server.RemoveClient(selectedItem.Text);
   
            for (var i = 0; i < tabControl.TabPages.Count; i++)
            {
                if (tabControl.TabPages[i].Text.Equals(selectedItem.Text))
                {
                    tabControl.TabPages.RemoveAt(i);
                    break;
                }
            }

            Console.WriteLine(selectedItem.Text);
        }

        private void lvwIP_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lvwIP.SelectedItems.Count > 0)
                {
                    this.ctxMenuIpList.Show(lvwIP, e.X, e.Y);
                }
            }
        }

        private void btnBatchCmd_Click(object sender, EventArgs e)
        {
            if (this._batchCmdForm == null || this._batchCmdForm.IsDisposed)
            {
                this._batchCmdForm = new BatchCmdForm(this);
              
            }
            this._batchCmdForm.Show();
            lock (_lockObject)
            {
                this._showingBatchCmdForm = true;
            }
        }

        private void tsmiCloseAll_Click(object sender, EventArgs e)
        {
            for (int i = tabControl.TabPages.Count - 1; i >= 0; i--)
            {
                if (tabControl.TabPages[i].Text != "Server Status")
                {
                    tabControl.TabPages.RemoveAt(i);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShellCat
{
    public partial class BatchCmdForm : Form
    {
        private int _oldLength = 0;
        public MainForm _mainForm;

        public BatchCmdForm(MainForm mainForm)
        {
            InitializeComponent();
            this._mainForm = mainForm;
            foreach(ListViewItem item in mainForm.lvwIP.Items)
            {
                var remote = new ListViewItem("");
                remote.SubItems.Add(item.Text);
                this.lvwIP.Items.Add(remote);
                this.lvwIP.Columns[0].Text = $"IP ({this.lvwIP.Items.Count})";
            }

            this.lvwIP.Columns[0].Tag = true;
            foreach (ListViewItem item in this.lvwIP.Items)
            {
                item.Checked = true;
            }

            rtbInput.Font = new Font(rtbInput.Font.FontFamily, 10.5f);
            rtbOutput.Font = new Font(rtbOutput.Font.FontFamily, 10.5f);

            rtbInput.AppendText("You are using bath cmd mode, all your shell cmd will send to all selected servers.");
            rtbInput.AppendText("\n$ ");
            _oldLength = rtbInput.Text.Length;
        }

        private delegate void DelegateRichTextBox(RichTextBox textBox, string content);

        private void lvwIP_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(e.Header.Tag);
                }
                catch (Exception)
                {
                }
                CheckBoxRenderer.DrawCheckBox(e.Graphics,
                    new Point(e.Bounds.Left + 4, e.Bounds.Top + 4),
                    value ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                    System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void lvwIP_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvwIP_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvwIP_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(this.lvwIP.Columns[e.Column].Tag);
                }
                catch (Exception)
                {
                }
                this.lvwIP.Columns[e.Column].Tag = !value;
                foreach (ListViewItem item in this.lvwIP.Items)
                {
                    item.Checked = !value;
                }
           
                this.lvwIP.Invalidate();
            }
        }

        public void AppendOutputText(string content)
        {
            // 对于该控件的请求来自于创建该控件所在线程以外的线程
            if (rtbOutput.InvokeRequired)
            {
                var rtbSet = new DelegateRichTextBox(delegate (RichTextBox tb, string cnt)
                {
                    tb.AppendText(cnt);
                    tb.ScrollToCaret(); //让滚动条拉到最底处   
                });
                rtbOutput.Invoke(rtbSet, rtbOutput, content);
            }
            else
            {
                rtbOutput.AppendText(content);
                rtbOutput.ScrollToCaret(); //让滚动条拉到最底处  
            }
        }

        /// <summary>
        /// 往回删除的时候，如果发现少于上次接收完成的文本长度，就禁止再继续删除，自动撤销删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtbInput_TextChanged(object sender, EventArgs e)
        {
            if (rtbInput.TextLength < _oldLength)
            {
                rtbInput.Undo();
            }
        }

        private void BatchCmdForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (_mainForm._lockObject)
            {
                this._mainForm._showingBatchCmdForm = false;
            }
        }

        private void rtbInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var cmd = rtbInput.Text.Substring(_oldLength);
                _oldLength = rtbInput.TextLength;
                for (var i = 0; i < lvwIP.Items.Count; i++)
                {
                    if (lvwIP.Items[i].Checked)
                    {
                        var remote = lvwIP.Items[i].SubItems[1].Text;
                        _mainForm._server.SendMessageToClient(remote, cmd + "\n");
                    }
                }

                rtbInput.AppendText("$ ");
                _oldLength = rtbInput.TextLength;
            }
        }
    }
}

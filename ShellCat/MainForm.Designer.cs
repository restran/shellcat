namespace ShellCat
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lvwIP = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctxMenuIpList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ckbKeepOne = new System.Windows.Forms.CheckBox();
            this.btnBatchCmd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtbServerStatus = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuIpList.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwIP
            // 
            this.lvwIP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvwIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwIP.Location = new System.Drawing.Point(0, 0);
            this.lvwIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lvwIP.MultiSelect = false;
            this.lvwIP.Name = "lvwIP";
            this.lvwIP.Size = new System.Drawing.Size(268, 407);
            this.lvwIP.TabIndex = 1;
            this.lvwIP.UseCompatibleStateImageBehavior = false;
            this.lvwIP.View = System.Windows.Forms.View.Details;
            this.lvwIP.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwIP_MouseClick);
            this.lvwIP.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwIP_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "IP";
            this.columnHeader1.Width = 168;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time";
            this.columnHeader2.Width = 76;
            // 
            // ctxMenuIpList
            // 
            this.ctxMenuIpList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeConnectionToolStripMenuItem});
            this.ctxMenuIpList.Name = "ctxMenuIpList";
            this.ctxMenuIpList.Size = new System.Drawing.Size(178, 26);
            // 
            // closeConnectionToolStripMenuItem
            // 
            this.closeConnectionToolStripMenuItem.Name = "closeConnectionToolStripMenuItem";
            this.closeConnectionToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.closeConnectionToolStripMenuItem.Text = "Close Connection";
            this.closeConnectionToolStripMenuItem.Click += new System.EventHandler(this.closeConnectionToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(676, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 481);
            this.panel1.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lvwIP);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 74);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(268, 407);
            this.panel4.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ckbKeepOne);
            this.panel3.Controls.Add(this.btnBatchCmd);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnStart);
            this.panel3.Controls.Add(this.tbPort);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(268, 74);
            this.panel3.TabIndex = 5;
            // 
            // ckbKeepOne
            // 
            this.ckbKeepOne.AutoSize = true;
            this.ckbKeepOne.Checked = true;
            this.ckbKeepOne.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbKeepOne.Location = new System.Drawing.Point(8, 44);
            this.ckbKeepOne.Name = "ckbKeepOne";
            this.ckbKeepOne.Size = new System.Drawing.Size(135, 21);
            this.ckbKeepOne.TabIndex = 5;
            this.ckbKeepOne.Text = "Keep One Every IP";
            this.ckbKeepOne.UseVisualStyleBackColor = true;
            // 
            // btnBatchCmd
            // 
            this.btnBatchCmd.Location = new System.Drawing.Point(161, 39);
            this.btnBatchCmd.Name = "btnBatchCmd";
            this.btnBatchCmd.Size = new System.Drawing.Size(96, 28);
            this.btnBatchCmd.TabIndex = 3;
            this.btnBatchCmd.Text = "Batch Cmd";
            this.btnBatchCmd.UseVisualStyleBackColor = true;
            this.btnBatchCmd.Click += new System.EventHandler(this.btnBatchCmd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Listen on";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(161, 6);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(96, 28);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(89, 9);
            this.tbPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(61, 23);
            this.tbPort.TabIndex = 0;
            this.tbPort.Text = "8080";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(676, 481);
            this.tabControl.TabIndex = 3;
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.rtbServerStatus);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(668, 451);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server Status";
            // 
            // rtbServerStatus
            // 
            this.rtbServerStatus.BackColor = System.Drawing.Color.Black;
            this.rtbServerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbServerStatus.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbServerStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.rtbServerStatus.Location = new System.Drawing.Point(0, 0);
            this.rtbServerStatus.Margin = new System.Windows.Forms.Padding(0);
            this.rtbServerStatus.Name = "rtbServerStatus";
            this.rtbServerStatus.ReadOnly = true;
            this.rtbServerStatus.Size = new System.Drawing.Size(668, 451);
            this.rtbServerStatus.TabIndex = 0;
            this.rtbServerStatus.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(676, 481);
            this.panel2.TabIndex = 4;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClose,
            this.tsmiCloseAll});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 48);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(152, 22);
            this.tsmiClose.Text = "Close Tab";
            this.tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // tsmiCloseAll
            // 
            this.tsmiCloseAll.Name = "tsmiCloseAll";
            this.tsmiCloseAll.Size = new System.Drawing.Size(152, 22);
            this.tsmiCloseAll.Text = "Close All Tab";
            this.tsmiCloseAll.Click += new System.EventHandler(this.tsmiCloseAll_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 481);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Name = "MainForm";
            this.Text = "ShellCat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ctxMenuIpList.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox rtbServerStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.ContextMenuStrip ctxMenuIpList;
        private System.Windows.Forms.ToolStripMenuItem closeConnectionToolStripMenuItem;
        private System.Windows.Forms.Button btnBatchCmd;
        public System.Windows.Forms.CheckBox ckbKeepOne;
        public System.Windows.Forms.ListView lvwIP;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseAll;
        public System.Windows.Forms.TabControl tabControl;
    }
}


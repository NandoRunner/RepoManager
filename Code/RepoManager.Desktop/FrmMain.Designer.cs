namespace SourceManager.Desktop
{
    partial class FrmMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.lbLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.abrirNoExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.gitBashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.codeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ignorarChecagemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.desbloquearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.pnMain = new System.Windows.Forms.Panel();
            this.chkSendByEmail = new System.Windows.Forms.CheckBox();
            this.btnListBlocked = new System.Windows.Forms.Button();
            this.btnTrelloBoards = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.btnListAll = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.txtPasta = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLog
            // 
            this.lbLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lbLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 16;
            this.lbLog.Location = new System.Drawing.Point(12, 168);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(760, 356);
            this.lbLog.TabIndex = 2;
            this.lbLog.SelectedIndexChanged += new System.EventHandler(this.lbLog_SelectedIndexChanged);
            this.lbLog.DoubleClick += new System.EventHandler(this.lbRepo_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirNoExplorerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.gitBashToolStripMenuItem,
            this.toolStripMenuItem2,
            this.codeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.ignorarChecagemToolStripMenuItem,
            this.desbloquearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(241, 132);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // abrirNoExplorerToolStripMenuItem
            // 
            this.abrirNoExplorerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("abrirNoExplorerToolStripMenuItem.Image")));
            this.abrirNoExplorerToolStripMenuItem.Name = "abrirNoExplorerToolStripMenuItem";
            this.abrirNoExplorerToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.abrirNoExplorerToolStripMenuItem.Text = "Open in Explorer";
            this.abrirNoExplorerToolStripMenuItem.Click += new System.EventHandler(this.abrirNoExplorerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(237, 6);
            // 
            // gitBashToolStripMenuItem
            // 
            this.gitBashToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("gitBashToolStripMenuItem.Image")));
            this.gitBashToolStripMenuItem.Name = "gitBashToolStripMenuItem";
            this.gitBashToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.gitBashToolStripMenuItem.Text = "Git Bash";
            this.gitBashToolStripMenuItem.Click += new System.EventHandler(this.gitBashToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(237, 6);
            // 
            // codeToolStripMenuItem
            // 
            this.codeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("codeToolStripMenuItem.Image")));
            this.codeToolStripMenuItem.Name = "codeToolStripMenuItem";
            this.codeToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.codeToolStripMenuItem.Text = "Code";
            this.codeToolStripMenuItem.Click += new System.EventHandler(this.codeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(237, 6);
            // 
            // ignorarChecagemToolStripMenuItem
            // 
            this.ignorarChecagemToolStripMenuItem.Name = "ignorarChecagemToolStripMenuItem";
            this.ignorarChecagemToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.ignorarChecagemToolStripMenuItem.Text = "Ignore Check Pending Changes";
            this.ignorarChecagemToolStripMenuItem.Click += new System.EventHandler(this.ignorarChecagemToolStripMenuItem_Click);
            // 
            // desbloquearToolStripMenuItem
            // 
            this.desbloquearToolStripMenuItem.Name = "desbloquearToolStripMenuItem";
            this.desbloquearToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.desbloquearToolStripMenuItem.Text = "Unblock Check";
            this.desbloquearToolStripMenuItem.Click += new System.EventHandler(this.desbloquearToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgressBar,
            this.tsLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLabel
            // 
            this.tsLabel.Name = "tsLabel";
            this.tsLabel.Size = new System.Drawing.Size(118, 17);
            this.tsLabel.Text = "toolStripStatusLabel1";
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Name = "tsProgressBar";
            this.tsProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // pnMain
            // 
            this.pnMain.BackColor = System.Drawing.Color.LemonChiffon;
            this.pnMain.Controls.Add(this.chkSendByEmail);
            this.pnMain.Controls.Add(this.btnListBlocked);
            this.pnMain.Controls.Add(this.btnTrelloBoards);
            this.pnMain.Controls.Add(this.btnSair);
            this.pnMain.Controls.Add(this.label1);
            this.pnMain.Controls.Add(this.btnAbrir);
            this.pnMain.Controls.Add(this.btnPesquisar);
            this.pnMain.Controls.Add(this.btnListAll);
            this.pnMain.Controls.Add(this.btnCheck);
            this.pnMain.Controls.Add(this.txtPasta);
            this.pnMain.Location = new System.Drawing.Point(12, 12);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(760, 137);
            this.pnMain.TabIndex = 17;
            // 
            // chkSendByEmail
            // 
            this.chkSendByEmail.AutoSize = true;
            this.chkSendByEmail.Location = new System.Drawing.Point(23, 104);
            this.chkSendByEmail.Name = "chkSendByEmail";
            this.chkSendByEmail.Size = new System.Drawing.Size(93, 17);
            this.chkSendByEmail.TabIndex = 26;
            this.chkSendByEmail.Text = "Send by Email";
            this.chkSendByEmail.UseVisualStyleBackColor = true;
            // 
            // btnListBlocked
            // 
            this.btnListBlocked.BackColor = System.Drawing.Color.Salmon;
            this.btnListBlocked.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnListBlocked.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnListBlocked.Location = new System.Drawing.Point(335, 60);
            this.btnListBlocked.Name = "btnListBlocked";
            this.btnListBlocked.Size = new System.Drawing.Size(130, 38);
            this.btnListBlocked.TabIndex = 25;
            this.btnListBlocked.Text = "&List Blocked";
            this.btnListBlocked.UseVisualStyleBackColor = false;
            this.btnListBlocked.Click += new System.EventHandler(this.btnListBlocked_Click);
            // 
            // btnTrelloBoards
            // 
            this.btnTrelloBoards.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTrelloBoards.Enabled = false;
            this.btnTrelloBoards.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrelloBoards.Location = new System.Drawing.Point(471, 60);
            this.btnTrelloBoards.Name = "btnTrelloBoards";
            this.btnTrelloBoards.Size = new System.Drawing.Size(130, 38);
            this.btnTrelloBoards.TabIndex = 24;
            this.btnTrelloBoards.Text = "&List Trello Boards";
            this.btnTrelloBoards.UseVisualStyleBackColor = true;
            this.btnTrelloBoards.Visible = false;
            // 
            // btnSair
            // 
            this.btnSair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSair.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSair.ImageIndex = 0;
            this.btnSair.ImageList = this.imageList1;
            this.btnSair.Location = new System.Drawing.Point(672, 60);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(70, 38);
            this.btnSair.TabIndex = 23;
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "sair");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Base Repos Folder:";
            // 
            // btnAbrir
            // 
            this.btnAbrir.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAbrir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbrir.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrir.Image = ((System.Drawing.Image)(resources.GetObject("btnAbrir.Image")));
            this.btnAbrir.Location = new System.Drawing.Point(710, 12);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(32, 28);
            this.btnAbrir.TabIndex = 21;
            this.btnAbrir.UseVisualStyleBackColor = false;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPesquisar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPesquisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.Image = ((System.Drawing.Image)(resources.GetObject("btnPesquisar.Image")));
            this.btnPesquisar.Location = new System.Drawing.Point(672, 12);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(32, 28);
            this.btnPesquisar.TabIndex = 20;
            this.btnPesquisar.UseVisualStyleBackColor = false;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // btnListAll
            // 
            this.btnListAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnListAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnListAll.Location = new System.Drawing.Point(23, 60);
            this.btnListAll.Name = "btnListAll";
            this.btnListAll.Size = new System.Drawing.Size(130, 38);
            this.btnListAll.TabIndex = 19;
            this.btnListAll.Text = "&List All";
            this.btnListAll.UseVisualStyleBackColor = true;
            this.btnListAll.Click += new System.EventHandler(this.btnListAll_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.Location = new System.Drawing.Point(159, 60);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(170, 38);
            this.btnCheck.TabIndex = 18;
            this.btnCheck.Text = "&Check Pending Changes";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // txtPasta
            // 
            this.txtPasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasta.Location = new System.Drawing.Point(155, 16);
            this.txtPasta.Name = "txtPasta";
            this.txtPasta.Size = new System.Drawing.Size(511, 22);
            this.txtPasta.TabIndex = 17;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnSair;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.pnMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Repo Manager";
            this.Load += new System.EventHandler(this.FrmSourceManager_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnMain.ResumeLayout(false);
            this.pnMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsLabel;
        private System.Windows.Forms.ToolStripProgressBar tsProgressBar;
        private System.Windows.Forms.Panel pnMain;
        internal System.Windows.Forms.Button btnAbrir;
        internal System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.Button btnListAll;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.TextBox txtPasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem abrirNoExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gitBashToolStripMenuItem;
        private System.Windows.Forms.Button btnTrelloBoards;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ignorarChecagemToolStripMenuItem;
        private System.Windows.Forms.Button btnListBlocked;
        private System.Windows.Forms.ToolStripMenuItem desbloquearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.CheckBox chkSendByEmail;
    }
}


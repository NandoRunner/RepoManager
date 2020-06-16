using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FAndradeTecInfo.Utils;
using SourceManager.Desktop.Business;
using SourceManager.Desktop.Controllers;
using SourceManager.Desktop.Model;

namespace SourceManager.Desktop
{
    public partial class FrmMain : Form
    {
        private string selectedPath;
        public FrmMain()
        {
            InitializeComponent();

            MyReg.SubKey = "SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName;
        }

        private void FrmSourceManager_Load(object sender, EventArgs e)
        {
            txtPasta.Text = MyReg.Read("PastaVerifica");
            this.Text = Application.ProductName + " - " 
                + Application.CompanyName
                + "          Version: " + Application.ProductVersion;

            MyForm.FormName = "FrmMain";
            MyForm.ListBoxName = "lbLog";
            BeginProcess();
            EndProcess();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            BeginProcess(sender);

            tsLabel.Text = "Starting processing...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, 0);

            gb.CheckPending();

            if (chkSendByEmail.Checked)
                SendToEmail("Pending Changes Repositories List");

            EndProcess(sender);
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsLabel.Text = "Starting processing...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, 0);

            gb.ListAll();

            if (chkSendByEmail.Checked)
                SendToEmail("All Repositories List");

            EndProcess(sender);
        }

        private void SendToEmail(string subject)
        {
            var emailFrom  = string.IsNullOrEmpty(MyReg.Read("EmailFrom")) ? string.Empty : MyReg.Read("EmailFrom");

            using (SimpleLogin frm = new SimpleLogin(Application.ProductName, emailFrom))
            {
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string body = String.Join("\n", lbLog.Items.Cast<String>().ToList());
                    try
                    {
                        BaseEmail email = new BaseEmail(frm.Login, "smtp.gmail.com", "587", frm.Pwd);
                        email.SendMessage(subject, body, "nando.az@gmail.com");

                        MyReg.Write("EmailFrom", frm.Login);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Sending email error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnListBlocked_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsLabel.Text = "Starting processing...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, 0);

            gb.ListBlocked();

            EndProcess(sender);
        }

        private void BeginProcess(object sender = null)
        {
            Cursor.Current = Cursors.WaitCursor;
            pnMain.Enabled = false;
            lbLog.DataSource = null;
            lbLog.Items.Clear();
            tsProgressBar.Value = 0;
            tsLabel.Text = "";

            foreach(Control c in pnMain.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    c.BackColor = Color.Wheat;
                }
            }
            if (sender != null)
            {
                ((Button)sender).BackColor = Color.Salmon;
                bool visible = true;
                if (((Button)sender).Name == "btnListBlocked")
                {
                    visible = false;
                }
                desbloquearToolStripMenuItem.Visible = !visible;
                ignorarChecagemToolStripMenuItem.Visible = visible;

            }
        }

        private void EndProcess(object sender = null)
        {
            if (sender != null)
            {
                ((Button)sender).BackColor = Color.Orange;
                MyReg.Write("PastaVerifica", txtPasta.Text);
            }
            pnMain.Enabled = true;
            chkSendByEmail.Checked = false;
            Cursor.Current = Cursors.Default;
        }

        private void lbRepo_DoubleClick(object sender, EventArgs e)
        {
            if (lbLog.SelectedIndex == -1)
                return;

            gitBashToolStripMenuItem_Click(sender, e);
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                SelectedPath = txtPasta.Text
            })
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                    txtPasta.Text = fbd.SelectedPath;
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            MyFS.RunExplorer(txtPasta.Text);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo sair?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private bool ValidarCampos()
        {
            string caption = "Erro na configuração";

            if (string.IsNullOrEmpty(txtPasta.Text))
            {
                MessageBox.Show("Informe a pasta de projetos!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!MyFS.FolderExists(txtPasta.Text))
            {
                MessageBox.Show("Pasta de projetos não encontrada!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void abrirNoExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyFS.RunExplorer(this.selectedPath);
        }

        private void gitBashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyFS.RunGitBash(this.selectedPath);
        }

        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyFS.RunVSCode(this.selectedPath);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool selected = true;

            if (lbLog.SelectedIndex == -1)
            {
                selected = false;
            }
            else
            {
                this.selectedPath = txtPasta.Text + lbLog.SelectedItem.ToString();
            }

            abrirNoExplorerToolStripMenuItem.Enabled = selected;
            gitBashToolStripMenuItem.Enabled = selected;
            ignorarChecagemToolStripMenuItem.Enabled = selected;
            desbloquearToolStripMenuItem.Enabled = selected;
            

            if (selected && lbLog.SelectedItem.ToString().ToLower().Contains("ionic"))
            {
                codeToolStripMenuItem.Enabled = selected;
            }
            else
            {
                codeToolStripMenuItem.Enabled = false;
            }
        }

        private async void btnTrelloBoards_Click(object sender, EventArgs e)
        {

            TrelloController trello = new TrelloController();
            var ret = await trello.GetBoards().ConfigureAwait(true);

            foreach (TrelloBoard tb in ret)
            {
                lbLog.Items.Add(tb.name);
            }

            lbLog.Refresh();

        }

        private void ignorarChecagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();

            var rb = new RepoBusiness(this.selectedPath);
            rb.IgnoreCheck();

            //EndProcess();
        }

        private void desbloquearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();

            var rb = new RepoBusiness(this.selectedPath);
            rb.IgnoreCheck(false);

            //EndProcess();
        }

        private void lbLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedPath = txtPasta.Text + lbLog.SelectedItem.ToString();
        }
    }
}

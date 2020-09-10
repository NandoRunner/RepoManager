using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FAndradeTI.Util;
using FAndradeTI.Util.FileSystem;
using FAndradeTI.Util.Network;
using FAndradeTI.Util.WinForms;

namespace SourceManager.Desktop
{
    public partial class FrmMain : Form
    {
        private string selectedPath;
        public FrmMain()
        {
            InitializeComponent();

            WinReg.SubKey = "SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName;
        }

        private void FrmSourceManager_Load(object sender, EventArgs e)
        {
            txtPasta.Text = WinReg.Read("PastaVerifica");
            txtEmail.Text = WinReg.Read("ToEmail") ?? string.Empty;
            this.Text = Application.ProductName + " - " 
                + Application.CompanyName
                + "          Version: " + Application.ProductVersion;

            FormControl.FormName = "FrmMain";
            FormControl.ListBoxName = "lbLog";
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

            tsInfo.Text = "Starting processing...";
            this.Refresh();

            Business.CheckPending(txtPasta.Text);

            if (chkSendByEmail.Checked)
                SendToEmail("Pending Changes Repositories List");

            EndProcess(sender);
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsInfo.Text = "Starting processing...";
            this.Refresh();

            Business.ListAll(txtPasta.Text);


            if (chkSendByEmail.Checked)
                SendToEmail("All Repositories List");

            EndProcess(sender);
        }

        private void SendToEmail(string subject)
        {
            var emailFrom  = WinReg.Read("FromEmail") ?? string.Empty;

            using (SimpleLogin frm = new SimpleLogin(Application.ProductName, emailFrom))
            {
                var result = frm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string body = String.Join("\n", lbLog.Items.Cast<String>().ToList());

                    var email = new GmailManager(frm.Login, "", frm.Pwd);
                    if (!email.Send(subject, body, txtEmail.Text, ""))
                    {
                        MessageBox.Show(EmailManager.LastErrorMessage, "Sending email error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    WinReg.Write("FromEmail", frm.Login);
                }
            }
        }

        private void btnListBlocked_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsInfo.Text = "Starting processing...";
            this.Refresh();

            Business.ListBlocked(txtPasta.Text);

            EndProcess(sender);
        }

        private void BeginProcess(object sender = null)
        {
            Cursor.Current = Cursors.WaitCursor;
            pnMain.Enabled = false;
            lbLog.DataSource = null;
            lbLog.Items.Clear();
            tsProgressBar.Value = 0;
            tsInfo.Text = "";

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
                //todo: revert
                //ignorarChecagemToolStripMenuItem.Visible = visible;

            }
        }

        private void EndProcess(object sender = null)
        {
            if (sender != null)
            {
                ((Button)sender).BackColor = Color.Orange;
                WinReg.Write("PastaVerifica", txtPasta.Text);
                WinReg.Write("ToEmail", txtEmail.Text);

            }
            pnMain.Enabled = true;
            //chkSendByEmail.Checked = false;
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
            ProcManager.RunExplorer(txtPasta.Text);
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

            if (!FS.FolderExists(txtPasta.Text))
            {
                MessageBox.Show("Pasta de projetos não encontrada!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (chkSendByEmail.Checked && string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Email destino inválido!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void abrirNoExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcManager.RunExplorer(this.selectedPath);
        }

        private void gitBashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcManager.RunGitBash(this.selectedPath);
        }

        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcManager.RunVSCode(this.selectedPath);
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
            //todo: revert
            //ignorarChecagemToolStripMenuItem.Enabled = selected;
            desbloquearToolStripMenuItem.Enabled = selected;


            if (selected && lbLog.SelectedItem.ToString().ToLower().Contains("ionic"))
            //if (selected)
            {
                codeToolStripMenuItem.Enabled = selected;
            }
            else
            {
                codeToolStripMenuItem.Enabled = false;
            }
        }

        private void ignorarChecagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();
            
            Business.IgnoreCheck(this.selectedPath);

            //EndProcess();
        }

        private void desbloquearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();

            Business.IgnoreCheck(this.selectedPath, false);

            //EndProcess();
        }

        private void lbLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedPath = txtPasta.Text + lbLog.SelectedItem.ToString();
        }

        private void chkSendByEmail_Click(object sender, EventArgs e)
        {
            txtEmail.Visible = chkSendByEmail.Checked;
        }
    }
}

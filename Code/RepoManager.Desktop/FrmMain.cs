using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using FAndradeTI.Util;
using FAndradeTI.Util.FileSystem;
using FAndradeTI.Util.Network;
using FAndradeTI.Util.WinForms;
using RepoManager.Desktop.Model;

namespace RepoManager.Desktop
{
    public partial class FrmMain : Form
    {
        private string _json;
        private string _selectedPath;
        private string _userName;
        private string _initMessage;

        public FrmMain()
        {
            InitializeComponent();

            WinReg.SubKey = "SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName;
        }

        private void abrirNoExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcManager.RunExplorer(this._selectedPath);
        }

        private void BeginProcess(object sender = null)
        {
            Cursor.Current = Cursors.WaitCursor;
            pnMain.Enabled = false;
            lbLog.DataSource = null;
            lbLog.Items.Clear();
            tsProgressBar.Value = 0;
            tsInfo.Text = "";

            foreach (Control c in pnMain.Controls)
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

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            ProcManager.RunExplorer(txtPasta.Text);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            BeginProcess(sender);

            tsInfo.Text = _initMessage;
            this.Refresh();

            Business.CheckPending(txtPasta.Text);

            //if (chkSendByEmail.Checked)
            //    SendToEmail("Pending Changes Repositories List");

            EndProcess(sender);
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsInfo.Text = _initMessage;
            this.Refresh();

            Business.ListAll(txtPasta.Text);

            Business.SaveListRepo(FS.PathCombine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDoc‌​uments), _json));

            if (chkSendByEmail.Checked)
                SendToEmail("All Repositories List");

            EndProcess(sender);
        }

        private void btnListBlocked_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsInfo.Text = _initMessage;
            this.Refresh();

            Business.ListBlocked(txtPasta.Text);

            EndProcess(sender);
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

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsInfo.Text = _initMessage;
            this.Refresh();

            string file = string.Empty;

            while (string.IsNullOrEmpty(file))
            {
                file = FS.GetFile(_json, "Select Repo list file", "JSON file (*.json)|*.json|Text File (*.txt)|*.txt", Application.StartupPath);
            }

            Business.RestoreListRepo(txtPasta.Text, file, _userName);

            EndProcess(sender);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja mesmo sair?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void chkSendByEmail_Click(object sender, EventArgs e)
        {
            txtEmail.Visible = chkSendByEmail.Checked;
        }

        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcManager.RunVSCode(this._selectedPath);
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
                this._selectedPath = FS.PathCombine(txtPasta.Text, lbLog.SelectedItem.ToString());
            }

            abrirNoExplorerToolStripMenuItem.Enabled = selected;
            gitBashToolStripMenuItem.Enabled = selected;
            //todo: revert
            //ignorarChecagemToolStripMenuItem.Enabled = selected;
            desbloquearToolStripMenuItem.Enabled = selected;

            string[] listCodeProjs = { "ionic", "react", "rails" };

            if (selected && listCodeProjs.Any(lbLog.SelectedItem.ToString().ToLower(CultureInfo.CurrentCulture).Contains))
            //if (selected)
            {
                codeToolStripMenuItem.Enabled = selected;
            }
            else
            {
                codeToolStripMenuItem.Enabled = false;
            }
        }

        private void desbloquearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();

            //Business.IgnoreCheck(this._selectedPath, false);

            //EndProcess();
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

        private void FrmRepoManager_Load(object sender, EventArgs e)
        {
            txtPasta.Text = WinReg.Read("PastaVerifica");
            txtEmail.Text = WinReg.Read("ToEmail") ?? string.Empty;
            this.Text = Application.ProductName + " - " 
                + Application.CompanyName
                + "          Version: " + Application.ProductVersion;

            FormControl.FormName = "FrmMain";
            FormControl.ListBoxName = "lbLog";

            _json = "ListRepo.json";
            _userName = "NandoRunner";
            _initMessage = "Starting processing...";

            BeginProcess();
            EndProcess();
        }
        private void gitBashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcManager.RunGitBash(this._selectedPath);
        }

        private void ignorarChecagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();

            //Business.IgnoreCheck(this._selectedPath);

            //EndProcess();
        }

        private void lbLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._selectedPath = FS.PathCombine(txtPasta.Text, lbLog.SelectedItem.ToString());
        }

        private void lbRepo_DoubleClick(object sender, EventArgs e)
        {
            if (lbLog.SelectedIndex == -1)
                return;

            gitBashToolStripMenuItem_Click(sender, e);
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

                    var email = new GmailManager(frm.Login, "", frm.Pwd)
                    {
                        Attach = FS.PathCombine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDoc‌​uments), _json)
                    };

                    if (!email.Send(subject, body, txtEmail.Text, ""))
                    {
                        MessageBox.Show(EmailManager.LastErrorMessage, "Sending email error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    WinReg.Write("FromEmail", frm.Login);
                }
            }
        }
        private bool ValidarCampos()
        {
            string caption = "Configuration error";

            if (string.IsNullOrEmpty(txtPasta.Text))
            {
                MessageBox.Show("Please inform repositories folder!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!FS.FolderExists(txtPasta.Text))
            {
                MessageBox.Show("Repository folder not found!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (chkSendByEmail.Checked && string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Invalid target e-mail!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}

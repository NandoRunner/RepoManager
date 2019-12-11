using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FAndradeTecInfo.Utils;
using SourceManager.Desktop.Business;
using SourceManager.Desktop.Controllers;
using SourceManager.Desktop.Model;

namespace SourceManager.Desktop
{
    public partial class FrmMain : Form
    {
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

            MyReg.Write("PastaVerifica", txtPasta.Text);

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

            MyReg.Write("PastaVerifica", txtPasta.Text);

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

                    BaseEmail email = new BaseEmail(frm.Login, "smtp.gmail.com", "587", frm.Pwd);

                    try
                    {
                        List<string> values = new List<string>();

                        foreach (object o in lbLog.Items)
                            values.Add(o.ToString());

                        string selectedItems = String.Join("\n", values);

                        email.SendMessage(subject, selectedItems, "nando.az@gmail.com");

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

            MyReg.Write("PastaVerifica", txtPasta.Text);

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
            if (sender != null) ((Button)sender).BackColor = Color.Orange;

            pnMain.Enabled = true;
            chkSendByEmail.Checked = false;
            Cursor.Current = Cursors.Default;
        }

        private void lbRepo_DoubleClick(object sender, EventArgs e)
        {
            if (lbLog.SelectedIndex == -1)
                return;

            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.RunGitBash();
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
            System.Diagnostics.Process.Start("explorer.exe", txtPasta.Text);
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

            if (!System.IO.Directory.Exists(txtPasta.Text))
            {
                MessageBox.Show("Pasta de projetos não encontrada!", caption,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void abrirNoExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.OpenExplorer();
        }

        private void gitBashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.RunGitBash();

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool selected = true;

            if (lbLog.SelectedIndex == -1) selected = false;

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

            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.IgnoreCheck();

            //EndProcess();
        }



        private void desbloquearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BeginProcess();

            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.IgnoreCheck(false);

            //EndProcess();
        }

        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.Code();
        }
    }
}

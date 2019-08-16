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
    public partial class FrmSourceManager : Form
    {
        public FrmSourceManager()
        {
            InitializeComponent();

            Reg.subKey = "SOFTWARE\\" + Application.CompanyName + "\\" + Application.ProductName;
        }

        private void FrmSourceManager_Load(object sender, EventArgs e)
        {
            txtPasta.Text = Reg.Read("PastaVerifica");
            this.Text = Application.ProductName + " - " 
                + Application.CompanyName
                + "          Version: " + Application.ProductVersion;
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

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, ref lbLog, ref statusStrip1);

            gb.CheckPending();

            Reg.Write("PastaVerifica", txtPasta.Text);

            EndProcess(sender);
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsLabel.Text = "Starting processing...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, ref lbLog, ref statusStrip1);

            gb.ListAll();

            Reg.Write("PastaVerifica", txtPasta.Text);

            EndProcess(sender);
        }

        private void btnListBlocked_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            BeginProcess(sender);

            tsLabel.Text = "Starting processing...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, ref lbLog, ref statusStrip1);

            gb.ListBlocked();

            Reg.Write("PastaVerifica", txtPasta.Text);

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
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = txtPasta.Text;

            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
                txtPasta.Text = fbd.SelectedPath;
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
        }

        private async void btnTrelloBoards_Click(object sender, EventArgs e)
        {

            TrelloController trello = new TrelloController();
            var ret = await trello.GetBoards();

            foreach (TrelloBoard tb in ret)
            {
                lbLog.Items.Add(tb.name);
            }

            lbLog.Refresh();

        }

        private void ignorarChecagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginProcess();

            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.IgnoreCheck();

            EndProcess();
        }



        private void desbloquearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginProcess();

            var rb = new RepoBusiness(txtPasta.Text + lbLog.SelectedItem.ToString());
            rb.IgnoreCheck(false);

            EndProcess();
        }
    }
}

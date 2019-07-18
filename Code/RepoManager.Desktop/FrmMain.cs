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

            initProcess();
            finishProcess();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            initProcess();

            tsLabel.Text = "Inicializando o processamento...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, ref lbRepo, ref statusStrip1);

            gb.CheckPending();

            Reg.Write("PastaVerifica", txtPasta.Text);

            finishProcess();
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            initProcess();

            tsLabel.Text = "Inicializando o processamento...";
            this.Refresh();

            RepoBusiness gb = new RepoBusiness(txtPasta.Text, ref lbRepo, ref statusStrip1);

            gb.ListAll();

            Reg.Write("PastaVerifica", txtPasta.Text);

            finishProcess();
        }

        private void initProcess()
        {
            Cursor.Current = Cursors.WaitCursor;
            panel1.Enabled = false;
            lbRepo.Items.Clear();
            tsProgressBar.Value = 0;
            tsLabel.Text = "";
        }

        private void finishProcess()
        {

            panel1.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void lbRepo_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", 
                System.IO.Path.Combine(txtPasta.Text, lbRepo.SelectedItem.ToString()));
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
            Application.Exit();
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

        private void lbRepo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

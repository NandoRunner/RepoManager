using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SourceManager.Desktop.Business;

namespace SourceManager.Desktop
{
    public partial class FrmSourceManager : Form
    {
        public FrmSourceManager()
        {
            InitializeComponent();
        }

        private void FrmSourceManager_Load(object sender, EventArgs e)
        {
            txtRoot.Text = @"C:\_Cloud\Projs\GitHub";
            initProcess();
            finishProcess();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            initProcess();

            tsLabel.Text = "Inicializando o processamento...";
            this.Refresh();

            GitBusiness gb = new GitBusiness(txtRoot.Text, ref lbRepo, ref statusStrip1);

            gb.CheckPending();

            finishProcess();
        }

        private void btnListAll_Click(object sender, EventArgs e)
        {
            initProcess();

            tsLabel.Text = "Inicializando o processamento...";
            this.Refresh();

            GitBusiness gb = new GitBusiness(txtRoot.Text, ref lbRepo, ref statusStrip1);

            gb.ListAll();

            finishProcess();
        }

        private void initProcess()
        {
            btnCheck.Enabled = false;
            btnListAll.Enabled = false;
            lbRepo.Items.Clear();
            tsProgressBar.Value = 0;
            tsLabel.Text = "";
            Cursor.Current = Cursors.WaitCursor;
        }

        private void finishProcess()
        {

            btnCheck.Enabled = true;
            btnListAll.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

    }
}

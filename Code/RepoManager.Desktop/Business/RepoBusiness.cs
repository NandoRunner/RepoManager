using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using System.Collections;
using System.Windows.Forms;

namespace SourceManager.Desktop.Business
{
    public class RepoBusiness
    {
        private string _basePath;
        private ListBox _lbRepo;
        private StatusStrip _statusStrip;
        private int numRepos;
        private int numPendingRepos;

        public StringBuilder sb;

        private ArrayList lstRepos;

        public RepoBusiness(string basePath, ref ListBox lbRepo, ref StatusStrip statusStrip)
        {
            _basePath = basePath;
            _lbRepo = lbRepo;
            _lbRepo.Items.Clear();
            _statusStrip = statusStrip;
            numRepos = 0;
            numPendingRepos = 0;

            lstRepos = new ArrayList();

            sb = new StringBuilder();

            LoadRepos(_basePath);

            ((ToolStripProgressBar)_statusStrip.Items[1]).Maximum = lstRepos.Count;
            _statusStrip.Refresh();
        }

        private void LoadRepos(string directory)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(directory);

            foreach (string subdirectory in subdirectoryEntries)
            {
                if (!LibGit2Sharp.Repository.IsValid(subdirectory))
                {
                    LoadRepos(subdirectory);
                }
                else
                {
                    lstRepos.Add(subdirectory);
                }
            }
        }

        public void CheckPending()
        {
            ExecSubDirectories(true);
        }

        public void ListAll()
        {
            ExecSubDirectories(false);
        }

        private void ExecSubDirectories(bool onlyPending)
        {
            foreach (string repo in lstRepos)
            {
                ((ToolStripProgressBar)_statusStrip.Items[1]).Value++;
                _statusStrip.Refresh();

                if (!onlyPending)
                {
                    _lbRepo.Items.Add(repo.Replace(_basePath, ""));
                    _lbRepo.Refresh();
                    ((ToolStripLabel)_statusStrip.Items[0]).Text = (++numRepos).ToString() + " repositórios encontrados";
                    _statusStrip.Refresh();
                    continue;
                }

                var objRepo = new LibGit2Sharp.Repository(repo);
                LibGit2Sharp.RepositoryStatus rs = objRepo.RetrieveStatus();

                if (rs.IsDirty)
                {
                    _lbRepo.Items.Add(repo.Replace(_basePath, ""));
                    _lbRepo.Refresh();
                    ((ToolStripLabel)_statusStrip.Items[0]).Text = (++numPendingRepos).ToString() + " repositórios com pendências encontrados"; ;
                    _statusStrip.Refresh();
                }
            }
        }
    }
}

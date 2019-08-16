using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using System.Collections;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SourceManager.Desktop.Business
{
    public class RepoBusiness
    {
        private string _basePath;
        private string _workingPath;
        private ListBox _lbRepo;
        private StatusStrip _statusStrip;
        private int numRepos;
        private int numPendingRepos;

        private List<string> listIgnoreCheck;


        public StringBuilder sb;

        private ArrayList lstRepos;

        public RepoBusiness(string workingPath)
        {
            _workingPath = workingPath;

            listIgnoreCheck = new List<string>();
        }

        public RepoBusiness(string basePath, ref ListBox lbRepo, ref StatusStrip statusStrip)
        {
            listIgnoreCheck = new List<string>();

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

        public void OpenExplorer()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = _workingPath
            };
            System.Diagnostics.Process.Start(psi);
        }

        public void IgnoreCheck(bool block = true)
        {
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Cloud\Projs\GitHub\RepoManager\Code\RepoManager.Desktop\RepoManager.mdf;Integrated Security=True";
            using (var conn = new SqlConnection(connString))
            {
                string sqlString = string.Empty;

                if (!block)
                {
                    sqlString = $"delete from ignoreCheck where name = '{_workingPath}'";
                }
                else
                {
                    sqlString = $"insert into ignoreCheck (name) values ('{_workingPath}')";
                }
                using (var command = new SqlCommand(sqlString, conn))
                {
                    conn.Open();
                    var result = command.ExecuteNonQuery();
                }
            }

        }

        private void LoadIgnoreCheck()
        {
            listIgnoreCheck.Clear();

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Cloud\Projs\GitHub\RepoManager\Code\RepoManager.Desktop\RepoManager.mdf;Integrated Security=True";
            using (var conn = new SqlConnection(connString))
            {
                string sqlString = @"select * from ignoreCheck";
                using (var command = new SqlCommand(sqlString, conn))
                {
                    conn.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listIgnoreCheck.Add(reader.GetString(1));
                        }
                    }
                }
            }
        }

        public void RunGitBash()
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.Arguments = @"/k ""C:\Program Files\Git\usr\bin\bash.exe"" --login -i ";
            psi.WorkingDirectory = _workingPath;
            System.Diagnostics.Process.Start(psi);
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
            LoadIgnoreCheck();

            ExecSubDirectories(true);
        }

        public void ListAll()
        {
            ExecSubDirectories(false);
        }

        public void ListBlocked()
        {
            LoadIgnoreCheck();

            _lbRepo.DataSource = listIgnoreCheck;
            _lbRepo.Refresh();
            ((ToolStripLabel)_statusStrip.Items[0]).Text = listIgnoreCheck.Count.ToString() + " Ignored repos found";
            _statusStrip.Refresh();
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
                    ((ToolStripLabel)_statusStrip.Items[0]).Text = (++numRepos).ToString() + " repos found";
                    _statusStrip.Refresh();
                    continue;
                }

                if (listIgnoreCheck.Contains(repo))
                    continue;
                
                var objRepo = new LibGit2Sharp.Repository(repo);
                LibGit2Sharp.RepositoryStatus rs = objRepo.RetrieveStatus();

                if (rs.IsDirty)
                {
                    _lbRepo.Items.Add(repo.Replace(_basePath, ""));
                    _lbRepo.Refresh();
                    ((ToolStripLabel)_statusStrip.Items[0]).Text = (++numPendingRepos).ToString() + " Pending changes repos found"; ;
                    _statusStrip.Refresh();
                }
            }
        }
    }
}

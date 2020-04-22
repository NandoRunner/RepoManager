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
using FAndradeTecInfo.Utils;
using System.Globalization;

namespace SourceManager.Desktop.Business
{
    public class RepoBusiness
    {
        private readonly string _basePath;
        private readonly string _workingPath;
        private int numRepos;
        private int numPendingRepos;
        private CultureInfo ci;
        private DateTime dtIni;

        private int all = 0;
        private int repos = 0;
        private int level = 0;


        private List<string> listIgnoreCheck;


        private StringBuilder sb;

        private readonly ArrayList lstRepos;

        public StringBuilder Sb { get => sb; set => sb = value; }

        private void Init()
        {
            listIgnoreCheck = new List<string>();

            MyStatusStrip.FormName = "FrmMain";
            MyStatusStrip.StatusStripName = "statusStrip1";

            ci = new CultureInfo("pt-BR");
        }

        public RepoBusiness(string workingPath)
        {
            _workingPath = workingPath;

            Init();
        }

        public RepoBusiness(string basePath, int num)
        {
            dtIni = DateTime.Now;

            Init();

            _basePath = basePath;

            MyForm.ClearListBox();
            numRepos = num;
            numPendingRepos = num;

            lstRepos = new ArrayList();

            Sb = new StringBuilder();

            LoadRepos(_basePath);

            MyStatusStrip.InitStatusStrip(string.Empty, lstRepos.Count);
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

        public void Code()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "code",
                Arguments = _workingPath
            };
            System.Diagnostics.Process.Start(psi);
        }

        public void RunGitBash()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = @"C:\Program Files\Git\usr\bin\bash.exe",
                Arguments = " --login -i ",
                //FileName = "cmd.exe",
                //Arguments = @"/k ""C:\Program Files\Git\usr\bin\bash.exe"" --login -i ",
                WorkingDirectory = _workingPath
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
                            Application.DoEvents();
                        }
                    }
                }
            }
        }

        private void LoadRepos(string directory)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(directory);

            foreach (string subdirectory in subdirectoryEntries)
            {
                var mydir = new DirectoryInfo(subdirectory);
                if (mydir.Name.Substring(0, 1) == "_")
                { 
                    continue;
                }

                this.all++;
                Application.DoEvents();
                if (!LibGit2Sharp.Repository.IsValid(subdirectory))
                {
                    level++;
                    LoadRepos(subdirectory);
                    level--;
                }
                else
                {
                    this.repos++;
                    lstRepos.Add(subdirectory);
                    MyStatusStrip.UpdateLabel($"repos: {this.repos} / all: {this.all}");
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

            MyForm.UpdateListBox(listIgnoreCheck);

            MyStatusStrip.UpdateLabel(listIgnoreCheck.Count.ToString(ci) + " Ignored repos found");
        }



        private void ExecSubDirectories(bool onlyPending)
        {
            foreach (string repo in lstRepos)
            {
                Application.DoEvents();
                MyStatusStrip.UpdateProgressBar();

                if (!onlyPending)
                {
                    MyForm.UpdateListBox(repo.Replace(_basePath, ""));
                    MyStatusStrip.UpdateLabel((++numRepos).ToString(ci) + " repos found");
                    continue;
                }

                if (listIgnoreCheck.Contains(repo))
                    continue;

                try
                {
                    using (Repository objRepo = new Repository(repo))
                    {
                        RepositoryStatus rs = objRepo.RetrieveStatus();

                        if (rs.IsDirty)
                        {
                            MyForm.UpdateListBox(repo.Replace(_basePath, ""));
                            MyStatusStrip.UpdateLabel((++numPendingRepos).ToString(ci) + " Pending changes repos found");
                        }
                    }
                }
                catch (LibGit2SharpException ex)
                {
                    MessageBox.Show($"{repo}\n\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (onlyPending)
            {
                MyStatusStrip.UpdateLabel($"{this.numPendingRepos} Pending changes repos found in {(DateTime.Now - dtIni).Seconds} seconds");
            }
            else
            {
                MyStatusStrip.UpdateLabel($"{this.repos} Pending changes repos found in {(DateTime.Now - dtIni).Seconds} seconds");
            }
        }
    }
}

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

            MyForm.UpdateListBox(listIgnoreCheck);

            MyStatusStrip.UpdateLabel(listIgnoreCheck.Count.ToString(ci) + " Ignored repos found");
        }



        private void ExecSubDirectories(bool onlyPending)
        {
            foreach (string repo in lstRepos)
            {
                MyStatusStrip.UpdateProgressBar();

                if (!onlyPending)
                {
                    MyForm.UpdateListBox(repo.Replace(_basePath, ""));
                    MyStatusStrip.UpdateLabel((++numRepos).ToString(ci) + " repos found");
                    continue;
                }

                if (listIgnoreCheck.Contains(repo))
                    continue;

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
        }
    }
}

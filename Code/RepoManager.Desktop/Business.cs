using FAndradeTI.Util.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using LibGit2Sharp;
using System.Data;

namespace SourceManager.Desktop
{
    public static class Business
    {
        private static int numRepos;
        private static int numPendingRepos;

        private static DateTime dtIni;

        public static int level = 0;
        public static int all = 0;
        public static int repos = 0;

        private static List<string> listIgnoreCheck = new List<string>();
        private static ArrayList lstRepos;

        public static StringBuilder Sb = new StringBuilder();

        private static void Init(string basePath)
        {
            dtIni = DateTime.Now;

            FormControl.ClearListBox();
            numRepos = 0;
            numPendingRepos = 0;

            lstRepos = new ArrayList();

            LoadRepos(basePath);

            StatusStripControl.InitStatusStrip(string.Empty, lstRepos.Count);
        }


        public static  void ListAll(string basePath)
        {
            Init(basePath);
            ExecSubDirectories(basePath, false);
        }

        public static void CheckPending(string basePath)
        {
            Init(basePath);

            //LoadIgnoreCheck();

            ExecSubDirectories(basePath, true);
        }

        public static void ListBlocked(string basePath)
        {
            Init(basePath);

            //LoadIgnoreCheck();

            FormControl.UpdateListBox(listIgnoreCheck);

            StatusStripControl.UpdateLabel(listIgnoreCheck.Count.ToString() + " Ignored repos found");
        }

        // todo: replace localdb by json
        private static void LoadIgnoreCheck()
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



        private static void ExecSubDirectories(string basePath, bool onlyPending)
        {
            foreach (string repo in lstRepos)
            {
                Application.DoEvents();
                StatusStripControl.UpdateProgressBar();

                if (!onlyPending)
                {
                    FormControl.UpdateListBox(repo.Replace(basePath, ""));
                    StatusStripControl.UpdateLabel((++numRepos).ToString() + " repos found");
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
                            FormControl.UpdateListBox(repo.Replace(basePath, ""));
                            StatusStripControl.UpdateLabel((++numPendingRepos).ToString() + " Pending changes repos found");
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
                StatusStripControl.UpdateLabel($"{numPendingRepos} Pending changes repos found in {(DateTime.Now - dtIni).Seconds} seconds");
            }
            else
            {
                StatusStripControl.UpdateLabel($"{repos} Pending changes repos found in {(DateTime.Now - dtIni).Seconds} seconds");
            }
        }

        public static void IgnoreCheck(string workingPath, bool block = true)
        {
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Cloud\Projs\GitHub\RepoManager\Code\RepoManager.Desktop\RepoManager.mdf;Integrated Security=True";

            using (var conn = new SqlConnection(connString))
            {
                string sqlString = string.Empty;

                using (var command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.Parameters.Add("@name", SqlDbType.NChar).Value = workingPath;

                    if (!block)
                    {
                        command.CommandText = "DELETE FROM ignoreCheck WHERE name=@name";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTOO  ignoreCheck (name) VALUES (@name)";
                    }

                    conn.Open();
                    var result = command.ExecuteNonQuery();
                }
            }

        }


        public static void LoadRepos(string directory)
        {
            string[] subdirectoryEntries = Directory.GetDirectories(directory);

            foreach (string subdirectory in subdirectoryEntries)
            {
                var mydir = new DirectoryInfo(subdirectory);
                if (mydir.Name.Substring(0, 1) == "_")
                {
                    continue;
                }

                all++;
                //Application.DoEvents();
                if (!LibGit2Sharp.Repository.IsValid(subdirectory))
                {
                    level++;
                    LoadRepos(subdirectory);
                    level--;
                }
                else
                {
                    repos++;
                    lstRepos.Add(subdirectory);
                    StatusStripControl.UpdateLabel($"repos: {repos} / all: {all}");
                }
            }
        }

    }
}

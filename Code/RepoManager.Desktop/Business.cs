using FAndradeTI.Util.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LibGit2Sharp;
using RepoManager.Desktop.Model;
using FAndradeTI.Util.FileSystem;
using Microsoft.Alm.Authentication;
using System.Globalization;

namespace RepoManager.Desktop
{
    public static class Business
    {
        private static int _numRepos;
        private static int _numPending;
        private static int _numRestored;
        private static string _basePath;


        private static DateTime _dtIni;

        private static int _level;
        private static int _all;

        private static List<string> _listIgnoreCheck;

        private static List<RepoInfo> _listRepo;


        private static void Init(string basePath)
        {
            _dtIni = DateTime.Now;

            FormControl.ClearListBox();
            _numRepos = 0;
            _numPending = 0;
            _numRestored = 0;

            _level = 0;
            _all = 0;


            _listIgnoreCheck = new List<string>();
            _listRepo = new List<RepoInfo>();

            _basePath = basePath;

            LoadRepos(basePath);

            StatusStripControl.InitStatusStrip(string.Empty, _listRepo.Count);
        }


        public static void ListAll(string basePath)
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

            FormControl.UpdateListBox(_listIgnoreCheck);

            StatusStripControl.UpdateLabel(_listIgnoreCheck.Count.ToString(CultureInfo.CurrentCulture) + " Ignored repos found");
        }

        // todo: replace localdb by json
        ////private static void LoadIgnoreCheck()
        ////{
        ////    _listIgnoreCheck.Clear();

        ////    string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Cloud\Projs\GitHub\RepoManager\Code\RepoManager.Desktop\RepoManager.mdf;Integrated Security=True";
        ////    using (var conn = new SqlConnection(connString))
        ////    {
        ////        string sqlString = @"select * from ignoreCheck";
        ////        using (var command = new SqlCommand(sqlString, conn))
        ////        {
        ////            conn.Open();
        ////            var reader = command.ExecuteReader();

        ////            if (reader.HasRows)
        ////            {
        ////                while (reader.Read())
        ////                {
        ////                    _listIgnoreCheck.Add(reader.GetString(1));
        ////                    Application.DoEvents();
        ////                }
        ////            }
        ////        }
        ////    }
        ////}

        private static void ExecSubDirectories(string basePath, bool onlyPending)
        {
            foreach (var repo in _listRepo)
            {
                Application.DoEvents();
                StatusStripControl.UpdateProgressBar();

                if (!onlyPending)
                {
                    FormControl.UpdateListBox(repo.Path.Replace(basePath, ""));
                    StatusStripControl.UpdateLabel((_numRepos).ToString(CultureInfo.CurrentCulture) + " repos found");
                    continue;
                }

                if (_listIgnoreCheck.Contains(repo.Path))
                    continue;

                try
                {
                    using (Repository objRepo = new Repository(FS.PathCombine(_basePath, repo.Path)))
                    {
                        RepositoryStatus rs = objRepo.RetrieveStatus();

                        if (rs.IsDirty)
                        {
                            FormControl.UpdateListBox(repo.Path.Replace(basePath, ""));
                            StatusStripControl.UpdateLabel((++_numPending).ToString(CultureInfo.CurrentCulture) + " repos with pending changes  found");
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
                StatusStripControl.UpdateLabel($"{_numPending} repos with pending changes found in {(DateTime.Now - _dtIni).Seconds} seconds");
            }
            else
            {
                StatusStripControl.UpdateLabel($"{_numRepos} repos found in {(DateTime.Now - _dtIni).Seconds} seconds");
            }
        }

        //public static void IgnoreCheck(string workingPath, bool block = true)
        //{
        //    string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Cloud\Projs\GitHub\RepoManager\Code\RepoManager.Desktop\RepoManager.mdf;Integrated Security=True";

        //    using (var conn = new SqlConnection(connString))
        //    {
        //        string sqlString = string.Empty;

        //        using (var command = new SqlCommand())
        //        {
        //            command.Connection = conn;
        //            command.Parameters.Add("@name", SqlDbType.NChar).Value = workingPath;

        //            if (!block)
        //            {
        //                command.CommandText = "DELETE FROM ignoreCheck WHERE name=@name";
        //            }
        //            else
        //            {
        //                command.CommandText = "INSERT INTOO  ignoreCheck (name) VALUES (@name)";
        //            }

        //            conn.Open();
        //            var result = command.ExecuteNonQuery();
        //        }
        //    }

        //}

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

                _all++;
                //Application.DoEvents();
                if (!LibGit2Sharp.Repository.IsValid(subdirectory))
                {
                    _level++;
                    LoadRepos(subdirectory);
                    _level--;
                }
                else
                {
                    _numRepos++;
                    var ri = new RepoInfo()
                    {
                        Name = subdirectory.Split('\\').Last().ToString(),
                        Path = subdirectory.Replace(_basePath+"\\", "")
                    };
                    _listRepo.Add(ri);
                    StatusStripControl.UpdateLabel($"repos: {_numRepos} / all: {_all}");
                }
            }
        }


        public static void SaveListRepo(string file)
        {
            FS.SaveJson<RepoInfo>(_listRepo, file);
        }


        public static void RestoreListRepo(string basePath, string file, string userName)
        {
            if (file == null)   throw new ArgumentNullException(nameof(file));

            dynamic list;

            if (file.Split('.').Last().ToString() == "txt") list = FS.LoadText(file);
            else list = FS.LoadJson<RepoInfo>(file);

            foreach (var item in list)
            {
                if (item.GetType() == file.GetType())
                {
                    var ri = new RepoInfo()
                    {
                        Name = ((string)item).Split('\\').Last().ToString(),
                        Path = FS.PathCombine(basePath, (string)item)
                    };

                    if (FS.FolderExists(ri.Path)) continue;

                    Application.DoEvents();
                    FormControl.UpdateListBox((string)item);
                    RestoreRepo(ri, basePath, userName);
                }
                else
                {
                    item.Path = FS.PathCombine(basePath, item.Path);

                    if (FS.FolderExists(item.Path)) continue;

                    RestoreRepo(item, basePath, userName);
                }
            }
            StatusStripControl.UpdateLabel($"{_numRestored} repos restored in {(DateTime.Now - _dtIni).Seconds} seconds");
        }

        private static void RestoreRepo(RepoInfo ri, string basePath, string userName)
        {
            var fullPath = ri.Path.Replace("\\" + ri.Name, "");

            if (!FS.FolderExists(fullPath)) FS.CreateFolder(fullPath);

            var cloneUrl = $"{ri.Name}.git";

            var site = ri.Path.Replace(basePath + "\\", "").Split('\\').First().ToString().ToLower(CultureInfo.CurrentCulture);

            if (site == "github")
            {
                cloneUrl = $"https://github.com/{userName}/" + cloneUrl;
                RestoreGit(ri, cloneUrl);
            }
            else
            {
                if (site == "bitbucket") cloneUrl = $"https://{userName}1@bitbucket.org/atexbr/" + cloneUrl;
                else if (site == "gitlab") cloneUrl = $"http://67.205.172.109/{userName}/{cloneUrl}";
                ProcManager.RunGitClone(fullPath, cloneUrl);
            }

            Application.DoEvents();
            StatusStripControl.UpdateLabel($"{(++_numRestored).ToString(CultureInfo.CurrentCulture)} repos restored - {ri.Name}");
        }

        private static void RestoreGit(RepoInfo ri, string cloneUrl)
        {
            var secrets = new SecretStore("git");
            var auth = new BasicAuthentication(secrets);

            var creds = auth.GetCredentials(new TargetUri("https://github.com"));

            var options = new CloneOptions
            {
                CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = creds.Username,
                    Password = creds.Password
                },
            };

            Repository.Clone(cloneUrl, ri.Path, options);

        }

    }
}

﻿using System;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Resources;

namespace SourceManager.Desktop
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex m = new Mutex(true, $"my{Application.ProductName}", out bool createdNew);
            m.Dispose();

            if (!createdNew)
            {
                var Rm = new ResourceManager("SourceManager.Desktop.Properties", Assembly.GetExecutingAssembly());

                // myApp is already running...
                MessageBox.Show(Rm.GetString("MessageBoxText01", CultureInfo.CurrentCulture),
                                Application.ProductName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (FrmMain frm = new FrmMain())
            {
                Application.Run(frm);
            }
        }
    }
}

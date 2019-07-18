using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace SourceManager.Desktop
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            bool createdNew;

            Mutex m = new Mutex(true, "my" + Application.ProductName, out createdNew);

            if (!createdNew)
            {
                // myApp is already running...
                MessageBox.Show("O aplicativo já está em execução!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmSourceManager());
        }
    }
}

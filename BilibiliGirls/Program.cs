using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilibiliGirls
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var pname = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            var pArray = System.Diagnostics.Process.GetProcessesByName(pname);
            if (pArray.Length > 1)
            {
                System.Windows.Forms.MessageBox.Show("不能一心二用，我也不会影分身！", "哼~", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

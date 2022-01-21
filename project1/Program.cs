using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 frm1 = new Form1();
            Application.Run(frm1);          
            if (frm1.IsLogin == true)
            {
                Form2 frm2 = new Form2();
                Application.Run(frm2);
            }
        }
    }
}

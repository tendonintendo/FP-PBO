// Program.cs
using System;
using System.Windows.Forms;

namespace FP
{
    static class Program
    {
        /// <summary>
        /// Titik masuk aplikasi.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Ganti MainForm dengan HomeForm
            Application.Run(new LoseForm());
        }
    }
}

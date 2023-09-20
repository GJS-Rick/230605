using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using nsUI;

namespace GJSControl
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isFirstOpen;

            Mutex mutex = new Mutex(false, Application.ProductName, out isFirstOpen);

            if (!isFirstOpen)
            {
                mutex.Dispose();
                mutex = null;
                MessageBox.Show("GJS Control has opened\n重複開啟!");
                return;
            }

           // EurekaLogSystem.ExceptionHandler.Activate();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FmMain());

            mutex.Dispose();
        }
    }
}

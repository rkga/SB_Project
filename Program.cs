using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace StudioBoss
{
  
    static class Program
    {
 
        private static Mutex mutex = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
          


            const string appName = "StudioBoss";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                //app is already running! Exiting the application  
                MessageBox.Show("Application is already running.");
              //  Process.GetCurrentProcess().Kill();
                Application.Exit();
                //return;
            }

            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new frmScannerApp());
               // StudioBoss mw = new StudioBoss();
                Application.Run(new StudioBoss());

            }
            catch {
                MessageBox.Show(@"Zebra Barcode Driver not detected, the installation will start now !");
                //MessageBox.Show(Application.StartupPath + @"\Zebra_CoreScanner_Driver_64bit.exe");
                ProcessStartInfo startInfo = new ProcessStartInfo(Application.StartupPath + @"\Zebra_CoreScanner_Driver_64bit.exe");
                startInfo.UseShellExecute = true;//This should not block your program
                Process.Start(startInfo);
                //Application.DoEvents();
               // Process.GetCurrentProcess().Kill();
                Application.Exit();
                //Process.Start(Application.StartupPath  + @"\Zebra_CoreScanner_Driver_64bit.exe");

            }

         
        }
    }
}
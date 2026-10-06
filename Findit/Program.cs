using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Findit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            // Used by the installer to validate a license key; no UI.
            if (args.Length > 0 && string.Equals(args[0], "/checkkey", StringComparison.OrdinalIgnoreCase))
            {
                return Licensing.CheckKeyCommand(args);
            }
            RunApplication();
            return 0;
        }

        // Kept out of Main so "/checkkey" never loads the UI or its dependencies: the installer
        // runs it from a temp folder holding only Findit.exe and NPC.Licensing.dll.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RunApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}

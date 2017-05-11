using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PDFFinder
{
    public class FileAssociationManager
    {
        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder sOut, [In][Out] ref uint nOut);

        [DllImport("shell32.dll", EntryPoint = "ExtractIconA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr ExtractIcon(int hInst, string lpszExeFileName, int nIconIndex);

        [Flags]
        public enum AssocF
        {
            Init_NoRemapCLSID = 0x1,
            Init_ByExeName = 0x2,
            Open_ByExeName = 0x2,
            Init_DefaultToStar = 0x4,
            Init_DefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }

        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic,
            InfoTip,
            QuickTip,
            TileInfo,
            ContentType,
            DefaultIcon,
            ShellExtension,
            DropTarget,
            DelegateExecute,
            Max
        }

        public IEnumerable<string> ListOfProgids(string ext)
        {
            List<string> progs = new List<string>();
            string baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\." + ext;

            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(baseKey + @"\OpenWithProgids"))
            {
                if (rk != null)
                {
                    foreach (string item in rk.GetValueNames())
                        progs.Add(item);
                }
            }
            return progs;
        }

        public string GetApplicationName(string association)
        {
            uint cOut = 0;
            if (AssocQueryString(AssocF.Verify, AssocStr.FriendlyAppName, association, null, null, ref cOut) != 1)
                return null;
            StringBuilder pOut = new StringBuilder((int)cOut);
            if (AssocQueryString(AssocF.Verify, AssocStr.FriendlyAppName, association, null, pOut, ref cOut) != 0)
                return null;
            return pOut.ToString();
        }

        public string GetApplicationPath(string association)
        {
            uint cOut = 0;
            if (AssocQueryString(AssocF.Verify, AssocStr.Executable, association, null, null, ref cOut) != 1)
                return null;
            StringBuilder pOut = new StringBuilder((int)cOut);
            if (AssocQueryString(AssocF.Verify, AssocStr.Executable, association, null, pOut, ref cOut) != 0)
                return null;
            return pOut.ToString();
        }

        public Icon ExtractIconFromFile(string filePath)
        {
            try
            {
                Icon icon = Icon.ExtractAssociatedIcon(filePath);
                return icon;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}

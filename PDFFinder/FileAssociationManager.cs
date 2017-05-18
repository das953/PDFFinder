using Microsoft.Win32;
using PDFFinder.BusinessLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        /// <summary>
        /// Enumeration which represents file properties (name of file, icon, full path etc.)
        /// </summary>
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

        #region Get properties associated with file (private methods)
        /// <summary>
        /// Get list of progId for file extension
        /// </summary>
        /// <param name="ext">File extension</param>
        /// <returns></returns>
        private IEnumerable<string> ListOfProgids(string ext)
        {
            List<string> progs = new List<string>();
            string baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + ext;

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

        /// <summary>
        /// Get application name for handling file of particular extension or progId
        /// </summary>
        /// <param name="association">File extension (will get default application name associated with file extension) or progId (will get application name associated with progId)</param>
        /// <returns></returns>
        private string GetApplicationName(string association)
        {
            uint cOut = 0;
            if (AssocQueryString(AssocF.Verify, AssocStr.FriendlyAppName, association, null, null, ref cOut) != 1)
                return null;
            StringBuilder pOut = new StringBuilder((int)cOut);
            if (AssocQueryString(AssocF.Verify, AssocStr.FriendlyAppName, association, null, pOut, ref cOut) != 0)
                return null;
            return pOut.ToString();
        }

        /// <summary>
        /// Get application path for handling file of particular extension or progId
        /// </summary>
        /// <param name="association">File extension (will get default application path associated with file extension) or progId (will get application path associated with progId)</param>
        /// <returns></returns>
        private string GetApplicationPath(string association)
        {
            uint cOut = 0;
            if (AssocQueryString(AssocF.Verify, AssocStr.Executable, association, null, null, ref cOut) != 1)
                return null;
            StringBuilder pOut = new StringBuilder((int)cOut);
            if (AssocQueryString(AssocF.Verify, AssocStr.Executable, association, null, pOut, ref cOut) != 0)
                return null;
            return pOut.ToString();
        }

        /// <summary>
        /// Get application icon
        /// </summary>
        /// <param name="filePath">Path to application</param>
        /// <returns></returns>
        private Icon ExtractIconFromFile(string filePath)
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
        #endregion

        #region Get/set applications properties in registry (public methods)
        /// <summary>
        /// Get applications associated with file extension
        /// </summary>
        /// <param name="ext">File extension</param>
        /// <returns></returns>
        public IEnumerable<AppDescription> GetAssociatedApplications(string ext)
        {
            List<AppDescription> applications = new List<AppDescription>();
            IEnumerable<string> progIdList = ListOfProgids(ext);
            foreach (var progId in progIdList)
            {
                string appName = GetApplicationName(progId);
                string appPath = GetApplicationPath(progId);
                Icon appIcon = ExtractIconFromFile(appPath);
                ImageSource imageSource;
                using (Bitmap bmp = appIcon.ToBitmap())
                {
                    var stream = new MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    imageSource = BitmapFrame.Create(stream);
                }
                applications.Add(new AppDescription { Name = appName, Path = appPath, Icon = imageSource, ProgId = progId });
            }
            return applications;
        }

        /// <summary>
        /// Get default application associated with file extension
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public AppDescription GetDefaultApplication(string ext)
        {
            string appName = GetApplicationName(ext);
            string appPath = GetApplicationPath(ext);
            Icon appIcon = ExtractIconFromFile(appPath);
            ImageSource imageSource;
            using (Bitmap bmp = appIcon.ToBitmap())
            {
                var stream = new MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                imageSource = BitmapFrame.Create(stream);
            }
            return new AppDescription { Name = appName, Path = appPath, Icon = imageSource };
        }

        /// <summary>
        /// Save application in registry
        /// </summary>
        /// <param name="progId">ProgId</param>
        /// <param name="ext">File extension</param>
        public void SaveAssociatedApplication(string progId, string ext)
        {
            string baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + ext;
            using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(baseKey + @"\DefaultViewer"))
            {
                rk.SetValue("ProgId", progId);
            }
        } 
        public AppDescription GetAssociatedApplication(string ext)
        {
            AppDescription appDescription;
            string baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + ext;
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(baseKey + @"\DefaultViewer"))
            {
                if(rk==null)
                {
                    appDescription = GetDefaultApplication(ext);
                }
                else
                {
                    string progId = rk.GetValue("ProgId").ToString();
                    appDescription = GetDefaultApplication(progId);
                }
            }
            return appDescription;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportEngineerTool.Models {
    class Installation {
        public class InstallInformation {
            public string InstallFolder { get; set; }
            public string DataStore { get; set; }
            public string ImageStore { get; set; }


            public InstallInformation() {
                InstallFolder = GetInstallFolderLocation();
                DataStore = GetDataStoreLocation();
                //TODO: Search for imagestore. May Opt to grab from database. Shouldn't do that in Constructor I think though..
               // ImageStore = GetImageStoreLocation();

            }

            public string GetInstallFolderLocation() {
                return LocatePath("OpenAsset_Install");
            }
            public string GetDataStoreLocation() {
                return LocatePath("OpenAsset_Data");
            }

            //TODO: Decide where we're going to get this information from. DB? Or manually search for it on the hard disks.
            public string GetImageStoreLocation() {
                return String.Empty;
            }

            public string LocatePath(string targetFolder) {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                List<string> possibleFolders = new List<string>();

                foreach (var driveInfo in allDrives) {
                    if (Directory.Exists(Path.Combine(driveInfo.RootDirectory.FullName, targetFolder))) {
                        possibleFolders.Add(Path.Combine(driveInfo.RootDirectory.FullName, targetFolder));
                    }
                }
                if (possibleFolders.Count >= 1)
                    if (possibleFolders.Count == 1) {
                        return possibleFolders.First();
                    }
                    else {
                        List<DirectoryInfo> installFolders = new List<DirectoryInfo>();
                        if (possibleFolders.Count > 1) {
                            foreach (var installFolder in possibleFolders) {
                                installFolders.Add(new DirectoryInfo(installFolder));
                                installFolders.Sort((x, y) => y.LastWriteTime.CompareTo(x.LastWriteTime));

                            }
                            return installFolders.First().ToString();
                        }
                    }
                return "No Installation Folder Found";
            }
        }
    }
}

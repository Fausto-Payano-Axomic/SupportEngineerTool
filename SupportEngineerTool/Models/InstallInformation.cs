using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace SupportEngineerTool.Models {
    public class InstallInformation {
        public string InstallFolder { get; set; }
        public string DataStore { get; set; }
        public string ImageStore { get; set; }
        public OpenAssetConfigurationFile ApacheConfigFile { get; set; }


        public InstallInformation() {
            ApacheConfigFile = new OpenAssetConfigurationFile();
            this.InstallFolder = ApacheConfigFile.CodeBase;
            this.DataStore = ApacheConfigFile.DataPath;
            this.ImageStore = GetImageStoreLocation().Replace(@"\",@"/");
            Log.Logger.Information($"Install information located: Installation Folder: {this.InstallFolder}, " +
                                   $"DataStore {this.DataStore} -" +
                                   $"Image Store {this.ImageStore}");
        }

        public string GetImageStoreLocation() {
            return LocatePath("OpenAsset_Images");
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

        public void RefreshConfigFile() {
            ApacheConfigFile.ReadConfigurationFile();
            this.ImageStore = GetImageStoreLocation().Replace(@"\", @"/");
        }
    }
}

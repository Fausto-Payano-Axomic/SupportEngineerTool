using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SupportEngineerTool.Annotations;

namespace SupportEngineerTool.Models {
    public class InstallInformationModel : INotifyPropertyChanged {
        private string _codeBase;
        private string _dataStore;
        private string _imageStore;

        public string CodeBase {
            get { return _codeBase; }
            set {
                _codeBase = value;
                NotifyPropertyChanged();
            }
        }
        public string DataStore {
            get { return _dataStore; }
            set {
                _dataStore = value;
                NotifyPropertyChanged();
            }
        }
        public string ImageStore {
            get { return _imageStore; }
            set {
                _imageStore = value;
                NotifyPropertyChanged();
            }
        }


        public OpenAssetConfigurationFile ApacheConfigFile { get; set; }
        public InstallInformationModel() {
            ApacheConfigFile = new OpenAssetConfigurationFile();
            this.CodeBase = ApacheConfigFile.CodeBase;
            this.DataStore = ApacheConfigFile.DataPath;
            this.ImageStore = GetImageStoreLocation().Replace(@"\",@"/");
            Log.Logger.Information($"Install information located: Installation Folder: {this.CodeBase}, " +
                                   $"DataStore {this.DataStore} -" +
                                   $"Image Store {this.ImageStore}");
        }
        public string GetImageStoreLocation() {
            return LocatePath("OpenAsset_Images");
        }

        /// <summary>
        /// Search drives for particular folder. One level deep.
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
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

        //TODO: Rework this into a command and possibly done by a service to reduce coupling.
        public void RefreshConfigFile() {
            ApacheConfigFile.ReadConfigurationFile();
            this.CodeBase = ApacheConfigFile.CodeBase;
            this.DataStore = ApacheConfigFile.DataPath;
            this.ImageStore = GetImageStoreLocation().Replace(@"\", @"/");
        }

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}

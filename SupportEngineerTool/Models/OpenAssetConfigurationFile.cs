using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SupportEngineerTool.Annotations;

//TODO: Implement the ports that the Apache file is listening on. Removed for now because I cannot figure out how to properly transfer over the collection to the viewmodel so that it updates.

namespace SupportEngineerTool.Models {
    public class OpenAssetConfigurationFile : INotifyPropertyChanged {
        public string codeBase { get; set; }
        public string dataPath { get; set; }
       // public List<string> openPorts { get; set; }
        public string databaseName { get; set; }

        public string CodeBase {
            get { return codeBase; }
            set {
                codeBase = value;
                NotifyPropertyChanged();
            }

        }
        public string DataPath {
            get { return dataPath; }
            set {
                dataPath = value;
                NotifyPropertyChanged();
            }
        }

        public string DatabaseName {
            get { return databaseName; }
            set {
                databaseName = value;
                NotifyPropertyChanged();
            }
        }
        //TODO Update notifycollection changed? Or can I still use PropertyChanged.

       /* public ObservableCollection<string> OpenPorts {get { return openPorts; }
            set[]
            
        }*/


        public OpenAssetConfigurationFile(string filePath = "C:/Apache2/conf/OpenAsset.conf") {
            ReadConfigurationFile(filePath);
        }
        public void ReadConfigurationFile(string filePath = "C:/Apache2/conf/OpenAsset.conf") {

            try {
                var contents = File.ReadAllLines(filePath);
                foreach (var line in contents) {
                    if (line != null) {
                        /* if (line.Contains("Listen")) {
                             this.OpenPorts.Add(ParseLine(line));
                         }*/
                        if (line.Contains("OpenAsset_Install_Path")) {
                            this.CodeBase = ParseLine(line);
                        }
                        else if (line.Contains("OpenAsset_Data_Path")) {
                            this.DataPath = ParseLine(line);
                        }
                        else if (line.Contains("OpenAsset_Database_Name")) {
                            this.DatabaseName = ParseLine(line);
                        }
                    }
                }
            }
            catch (Exception readConfigFileException) {
                Log.Logger.Error(
                    $"Error reading configuration file located at {filePath}. The exception is as below: \n {readConfigFileException.Message}");
            }

        }
        private string ParseLine(string line) {
            string replacedDebug = (line.Replace(" ", ","));

            string[] stringContainer = replacedDebug.Split(new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);
            if (stringContainer.Length < 2 && stringContainer.Contains("Listen")) {
                return stringContainer[1];
            }
            else {

                return stringContainer[2];
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

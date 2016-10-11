using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SupportEngineerTool.Annotations;


namespace SupportEngineerTool.Models {

    //This class may not need to notify anyone that its property changed. Could probably remove that unless its implemented as part of the ApacheViewModel.

    public class OpenAssetConfigurationFile : INotifyPropertyChanged {
        private string _codeBase { get; set; }
        private string _dataPath { get; set; }
       //public List<string> openPorts { get; set; }
        private string _databaseName { get; set; }

        #region Public_Properties
        public string CodeBase {
            get { return _codeBase; }
            set {
                _codeBase = value;
                NotifyPropertyChanged();
            }

        }
        public string DataPath {
            get { return _dataPath; }
            set {
                _dataPath = value;
                NotifyPropertyChanged();
            }
        }

        public string DatabaseName {
            get { return _databaseName; }
            set {
                _databaseName = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
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
            CoverAnyNullOrEmpty(this);

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

        /// <summary>
        /// Utilizes reflection to review whether we have empty Members that may not have been initialized or found in the OpenAsset config file.
        /// </summary>
        /// <param name="obj"></param>
        private void CoverAnyNullOrEmpty(object obj) {
            foreach (PropertyInfo property in obj.GetType().GetProperties()) {
                if (property.PropertyType == typeof(string) && !property.Name.Contains("_")) {
                    if(string.IsNullOrEmpty((string)property.GetValue(obj))) {
                        property.SetValue(obj, "No Information Available", null);
                    }
                }
            }
        }
           

        #region NotifyPropertyChanged
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
        #endregion
    }
}

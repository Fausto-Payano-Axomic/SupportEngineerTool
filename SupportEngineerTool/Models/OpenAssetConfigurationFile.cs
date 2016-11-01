﻿using System;
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

//TODO: Change this class to a thread-safe singleton so it can be accesed by the Apache viewmodel as well, easily.
namespace SupportEngineerTool.Models {
    public class OpenAssetConfigurationFile {
        public string CodeBase { get; set; }
        public string DataPath { get; set; }
        public string DatabaseName { get; set; }
        public string SslCertificateAuthority { get; set; }
        public string SslCertificateFile { get; set; }
        public string SslCertificateKey { get; set; }

        public List<string> Ports = new List<string>();


        public OpenAssetConfigurationFile(string filePath = "C:/Apache2/conf/OpenAsset.conf") {
            ReadConfigurationFile(filePath);
        }
        public void ReadConfigurationFile(string filePath = "C:/Apache2/conf/OpenAsset.conf") {

            //Perhaps looping through a dictionary may be a better approach here for each line, for less copy pasta.
            try {
                var contents = File.ReadAllLines(filePath);
                foreach (var line in contents) {
                    if (line == null && !(line.StartsWith("#"))) continue;

                    if (line.Contains("SSLCertificateAuthorityCA")) {
                        this.SslCertificateAuthority = ParseLine(line);
                    }
                    else if (line.Contains("SSLCertificateFile")) {
                        this.SslCertificateFile = ParseLine(line);
                    }
                    else if (line.Contains("SSLCertificateKeyFile")) {
                        this.SslCertificateKey = ParseLine(line);
                    }
                    else if (line.Contains("Listen")) {
                        this.Ports.Add(line.Split(' ')[1]);
                    }
                    else if (line.Contains("OpenAsset_Install_Path")) {
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

            /* Need to make sure that the new additional OR check actually 
             * doesn't produce unwanted results. Breakpoint should be set here and tested
             * thoroughly.
            */

            if (stringContainer.Length < 2 && (stringContainer.Contains("Listen")) || (stringContainer.Contains("SSL"))) {
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
                    if (string.IsNullOrEmpty((string)property.GetValue(obj))) {
                        property.SetValue(obj, "No Information Available", null);
                    }
                }
            }
        }
    }
}

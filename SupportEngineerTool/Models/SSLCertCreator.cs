using System.IO;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using SupportEngineerTool.HelperClasses;
using SupportEngineerTool.Properties;


namespace SupportEngineerTool.Models {
    public class SslCertCreator  //Implements the simpleton design pattern...like me.   *singleton
    {

        public string Pfx { get; set; }
        public const string OpenSsl = @"c:\Apache\bin\openssl.exe";
        public const string OaConfFile = @"C:\Apache\conf\OpenAsset.conf";
        public const string HttpdVHostFile = @"C:\Apache24\conf\extra\httpd-vhost.conf";
        private OpenAssetConfigurationFile _openAssetConfig;
        public string CodeBase { get; set; }
        public string DataFolder { get; set; }
        public string SslCertificateAuthorityCaPath { get; set; }
        public string SslCertificateFilePath { get; set; }
        public string SslCertificateKeyFilePath { get; set; }
        public string NonSslPort { get; set; }
        public bool _sslPortActive;

        public SslCertCreator() {
            this._openAssetConfig = ViewModelLocator.InstallationCardViewModel.installInfo.ApacheConfigFile;
            UpdateConfigContents();
        }
        public void ProcessClientPfxFile() {
            ExtractPrivateKeyFromClientPfxFile();
            RemovePassPhraseFromPrivateKey();
            ExtractCertFromClientPfx();
        }
        private void UpdateConfigContents() {
            this.CodeBase = _openAssetConfig.CodeBase;
            this.DataFolder = _openAssetConfig.DataPath;
            this.SslCertificateAuthorityCaPath = _openAssetConfig.SslCertificateAuthority;
            this.SslCertificateFilePath = _openAssetConfig.SslCertificateFile;
            this.SslCertificateKeyFilePath = _openAssetConfig.SslCertificateKey;
        }

        //Think this is completed in EasyUpdater
        private void ConfigureOpenAssetConfFile() {
            /*
             * 1. OpenAsset.conf to <filename>_<date and time>.conf.bak
             * 2. Download OpenAsset conf file from cloud
             * 3. Update Listening ports, codebase, and OpenAsset_Data folder paths using data from 
             *    old file.
             */
        }
        //For new apache httpd-vhost.conf file only. 

        #region GenerateMethods
        /// <summary>
        /// Generates a self signed .cert file.
        /// </summary>
        public void GenerateSelfSignedSslCert() {
            ProcessStartInfo startInfo = new ProcessStartInfo(OpenSsl, "req - newkey rsa: 2048 - nodes - keyout server.key - x509 - days 730 -out server.cert");
            Process.Start(startInfo);
        }
        /// <summary>
        /// Generates a .csr file using OpenSSL.exe
        /// </summary>
        public void GenerateCsr() {
            ProcessStartInfo startInfo = new ProcessStartInfo(OpenSsl, "req -new -newkey rsa:2048 -nodes -keyout server.key -out server.csr");
            Process.Start(startInfo);
        }
        #endregion
        #region PFXFileMethods

        /// <summary>
        /// Starts OpenSSL.exe and generates new .pem file.
        /// </summary>
        private void ExtractPrivateKeyFromClientPfxFile() {
            ProcessStartInfo getPrivateKey = new ProcessStartInfo(OpenSsl, "pkcs12 -in " + Pfx + " -nocerts -out original_priv.pem");
            Process.Start(getPrivateKey);
        }
        //Starts OpenSLL.exe and removes password from .pem file.
        private void RemovePassPhraseFromPrivateKey() {
            ProcessStartInfo removePassphrase = new ProcessStartInfo(OpenSsl, "rsa -in original_priv.pem -out priv.pem");
            Process.Start(removePassphrase);
        }
        /// <summary>
        /// Extracts cert from Pfx file.
        /// </summary>
        private void ExtractCertFromClientPfx() {
            ProcessStartInfo getPrivPub = new ProcessStartInfo(OpenSsl, "pkcs12 -in " + Pfx + " -out privpub.pem");
            ProcessStartInfo extractCert = new ProcessStartInfo(OpenSsl, "x509 -inform pem -outform pem -in privpub.pem -pubkey -out pub.pem");
            Process.Start(getPrivPub);
            Process.Start(extractCert);
#if DEBUG
            MessageBox.Show("Pfx processing complete.");
#endif
        }
        #endregion
    }
}

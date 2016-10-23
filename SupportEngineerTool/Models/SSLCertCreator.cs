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


namespace SupportEngineerTool.Models
{
    public class SSLCertCreator  //Implements the simpleton design pattern...like me.   *singleton
    { 

        public static string PFX = null;  //There can only be one instance of this variable.

        private SSLCertCreator SSLObject = null;

        string _openSSL = string.Empty;
        string _oaConfFile = string.Empty;
        string _httpdVHostFile = string.Empty;
        string _codeBase = string.Empty;
        string _dataFolder = string.Empty;
        string _sslCertificateAuthorityCAPath = string.Empty;
        string _sslCertificateFilePath = string.Empty;
        string _sslCertificateKeyFilePath = string.Empty;
        string _nonSSLPort = string.Empty;
        bool _sslPortActive;

        private SSLCertCreator()
        {
             _openSSL = @"c:\Apache\bin\openssl.exe";
             _oaConfFile = @"C:\Apache\conf\OpenAsset.conf";
             _httpdVHostFile = @"C:\Apache24\conf\extra\httpd-vhost.conf";
        }
       
        public SSLCertCreator GetSSLCertCreator() 
        {
            if (SSLObject == null)
            {
                SSLObject = new SSLCertCreator();
                return SSLObject;
            }
            else
            {
                return SSLObject;
            }      
        }

        public void CheckExistingApache2Configuration()
        {
            bool found = false;
            found = File.Exists(_oaConfFile) ? true : false;

            //TO DO: Check if ssl is configured in old apache: Parse and grab paths to current cert and keys
            if (found)
            {
                StreamReader file = new StreamReader(_oaConfFile);
                string line;
           
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("SSLCertificateAuthorityCA"))
                    {
                        string[] certificateAuthorityRow = line.Split(' ');
                        _sslCertificateAuthorityCAPath = certificateAuthorityRow[1];
                    }
                    else if (line.Contains("SSLCertificateFile"))
                    {
                        string[] certificateFileRow = line.Split(' ');
                        _sslCertificateFilePath = certificateFileRow[1];
                    }
                    else if (line.Contains("SSLCertificateKeyFile"))
                    {
                        string[] sslKeyRow = line.Split(' ');
                        _sslCertificateKeyFilePath = sslKeyRow[1];
                    }
                    else if (line.Contains("OpenAsset_Install_Path"))
                    {
                        string[] codeBaseRow = line.Split(' ');
                        _codeBase = codeBaseRow[2];
                    }
                    else if (line.Contains("OpenAsset_Data_Path"))
                    {
                        string[] dataFolderRow = line.Split(' ');
                        _dataFolder = dataFolderRow[2];
                    }

                    if (line.Contains("Listen") && line.Contains("443") && (line.Contains("#") == false))
                    {
                        //This means port 443 is listening and ssl is enabled.  
                        _sslPortActive = true;
                    }

                    if (line.Contains("Listen") && line.Contains("80") && (line.Contains("#") == false))
                    { 
                        _nonSSLPort = "80";
                    }
                    else if (line.Contains("Listen") && line.Contains("81") && (line.Contains("#") == false))
                    {
                        _nonSSLPort = "81";
                    }
                }
            } 
        }

        private void ConfigureOpenAssetConfFile()
        {
            /*
             * 1. OpenAsset.conf to <filename>_<date and time>.conf.bak
             * 2. Download OpenAsset conf file from cloud
             * 3. Update Listening ports, codebase, and OpenAsset_Data folder paths using data from 
             *    old file.
             */
        }
        //For new apache httpd-vhost.conf file only. 
        private void ConfigureApache24VHostsFile()
        {
            string NewProxyPassURL = CreateApacheRedirectEntry();   
            string text = File.ReadAllText(_httpdVHostFile);
            text = text.Replace("http://<Servers_IP>:8080/", NewProxyPassURL);
            //replace the Listen <port> entry with Listen 443
            text = text.Replace("<Listen port>", "443");
            File.WriteAllText(text, _httpdVHostFile);
        }

        public void GenerateSelfSignedSslCert()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_openSSL, "req - newkey rsa: 2048 - nodes - keyout server.key - x509 - days 730 -out server.cert");
            Process.Start(startInfo);
        }

        public void GenerateCsr()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_openSSL, "req -new -newkey rsa:2048 -nodes -keyout server.key -out server.csr");
            Process.Start(startInfo);
        }

        public void ProcessClientPfxFile()
        {
            ExtractPrivateKeyFromClientPFXFile();
            RemovePassPhraseFromPrivateKey();
            ExtractCertFromClientPFX();
        }

        private string CreateApacheRedirectEntry()
        {
            string prefix = "http://";
            string port = ":8080";
            string redirectURL = String.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string myIP = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
            redirectURL = prefix + myIP + port + "/";
            return redirectURL;
        }


        private void ExtractPrivateKeyFromClientPFXFile()
        {
            ProcessStartInfo getPrivateKey = new ProcessStartInfo(_openSSL, "pkcs12 -in " + PFX + " -nocerts -out original_priv.pem");
            Process.Start(getPrivateKey); 
        }

        private void RemovePassPhraseFromPrivateKey()
        {
            ProcessStartInfo removePassphrase = new ProcessStartInfo(_openSSL, "rsa -in original_priv.pem -out priv.pem");
            Process.Start(removePassphrase);
        }

        private void ExtractCertFromClientPFX()
        {
            ProcessStartInfo getPrivPub = new ProcessStartInfo(_openSSL, "pkcs12 -in " + PFX + " -out privpub.pem");
            ProcessStartInfo extractCert = new ProcessStartInfo(_openSSL, "x509 -inform pem -outform pem -in privpub.pem -pubkey -out pub.pem");
            Process.Start(getPrivPub);
            Process.Start(extractCert);
        }

        private void MoveApacheFilesToFinalLocation()
        {
            /*TO DO: 
            1. create a directory called backup in both the extra/conf and Apache2/conf folders if it doesn't exist
            2. before making the move operation to the extra/conf directory, check if a file with the same name  
               exists. If so rename the old file to <filename>_<insert date and time> and move it into the backup directory.
            3. Move the new httpd-vhosts file into its new home.
            */
        }

        

 /****************** Im gonna put this in a separate class called Apache24Installer.***********************/
        private void DownloadAndInstallApache24 ()
        {
            /*TO DO:
             * 1. Check if system is 32 or 64 bit.
             * 2. Check and Make sure update KB2999226 is installed
             * !!! The rest happens IF AND ONLY IF that update is installed otherwise there is no point to any further !!!
             * 3. Download Apache24 
             * 4. Unzip and extract
             * 5. Copy to C:\ drive
             * 6. download and Run the vcredist_x64.exe (64-bit) or vcredist_x86.exe (32 bit)
             * 7. Install the apache24 service
             * 8. Check if the new service exists
             * 9. The SSLCertCreator will handle the rest 
             */
        }
    }
}

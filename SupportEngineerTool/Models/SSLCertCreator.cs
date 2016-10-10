using System.IO;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Net.Sockets;

namespace SupportEngineerTool.Models
{
    class SSLCertCreator
    {
         public static string pfxFile;
         public static string openSSL = @"c:\Apache\bin\openssl.exe";
         public static string oaConfFile = @"C:\Apache\conf\OpenAsset.conf";
         public static string httpdVHostFile = @"C:\Apache24\conf\extra\httpd-vhost.conf";
        //check openasset conf for any ssl configuration
        public static void CheckForExistingOpenAssetConf()
        {
            bool found = false;

            found = File.Exists(oaConfFile) ? true : false;

            //TO DO: Check if ssl is configured in old apache: Parse and grab paths to current cert and keys
            if (found)
            {
                StreamReader file = new StreamReader(oaConfFile);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("SSLCertificateAuthorityCA"))
                    {
                        string[] certificateAuthorityRow = line.Split(' ');
                        string certificateAuthorityPath = certificateAuthorityRow[1];
                    }

                    if (line.Contains("SSLCertificateFile"))
                    {
                        string[] certificateFileRow = line.Split(' ');
                        string certificateAuthorityPath = certificateFileRow[1];
                    }

                    if (line.Contains("SSLCertificateKeyFile"))
                    {
                        string[] sslKeyRow = line.Split(' ');
                        string sslKeyPath = sslKeyRow[1];
                    }
                }
            } 
        }

        private static string CreateApacheRedirectEntry()
        {
            string prefix = "http://";
            string port = ":8080";
            string redirectURL = String.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string myIP = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
            redirectURL = prefix + myIP + port + "/";
            return redirectURL;
        }
        //For new apache httpd-vhost.conf file only. 
        private static void ReplaceProxyByPassEntryInApache()
        {
            string NewProxyPassURL = CreateApacheRedirectEntry();   

            string text = File.ReadAllText(httpdVHostFile);
            text = text.Replace("some text", NewProxyPassURL);
            File.WriteAllText("http://<Servers_IP>:8080/", httpdVHostFile);
        }

        public static void GenerateSelfSignedSslCert()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(openSSL, "req - newkey rsa: 2048 - nodes - keyout server.key - x509 - days 730 -out server.cert");
            Process.Start(startInfo);
        }

        public static void GenerateCsr()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(openSSL, "req -new -newkey rsa:2048 -nodes -keyout server.key -out server.csr");
            Process.Start(startInfo);
        }

        public static void ProcessClientPfxFile()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
        }
        
    }
}

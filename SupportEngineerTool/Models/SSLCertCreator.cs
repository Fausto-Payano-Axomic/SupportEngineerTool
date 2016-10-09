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
         private static string pfxFile;
         private static string openSSL = @"c:\Apache\bin\openssl.exe";

        //check openasset conf for any ssl configuration
        public static void CheckForExistingOpenAssetConf()
        {
            bool found = false;
            string oaConfFile = @"C:\Apache\conf\OpenAsset.conf";

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
        //Need ip address when updating httpd-vhost file redirect entry in Apache24 conf folder
        private static string GetMyIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string myIP = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();

            return myIP;
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

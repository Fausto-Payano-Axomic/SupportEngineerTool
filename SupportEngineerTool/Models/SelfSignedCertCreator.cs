using System.IO;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace SupportEngineerTool.Models
{
    class SelfSignedCertCreator
    {
        
        //check openasset conf for any ssl configuration
        public static void CheckForExistingOpenAssetConf()
        {
            bool response = false;
            string oaConfFile = "@C:\\Apache\\conf\\OpenAsset.conf";

            response = File.Exists(oaConfFile) ? true : false;

            //TO DO: Check if ssl is configured in old apache: Parse and grab paths to current cert and keys

           
        }
        
     
        
        public static void generateSelfSignedSSLCert()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"c:\\Apache\\bin\\openssl.exe", "req - newkey rsa: 2048 - nodes - keyout server.key - x509 - days 730 -out server.cert");
            Process.Start(startInfo);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SupportEngineerTool.Models;

namespace SupportEngineerTool.Services {
    static class ApacheConfigureService {

        private static void ConfigureApache24VHostsFile(SslCertCreator sslCertCreator) {
            string NewProxyPassURL = CreateApacheRedirectEntry();
            string text = File.ReadAllText(SslCertCreator.HttpdVHostFile);
            text = text.Replace("http://<Servers_IP>:8080/", NewProxyPassURL);
            //replace the Listen <port> entry with Listen 443
            text = text.Replace("<Listen port>", "443");
            File.WriteAllText(text, SslCertCreator.HttpdVHostFile);
        }

        private static string CreateApacheRedirectEntry() {
            string prefix = "http://";
            string port = ":8080";
            string redirectURL = String.Empty;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string myIP =
                host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
            redirectURL = prefix + myIP + port + "/";
            return redirectURL;
        }
        private static void MoveApacheFilesToFinalLocation() {
            /*TODO: 
            1. create a directory called backup in both the extra/conf and Apache2/conf folders if it doesn't exist
            2. before making the move operation to the extra/conf directory, check if a file with the same name  
               exists. If so rename the old file to <filename>_<insert date and time> and move it into the backup directory.
            3. Move the new httpd-vhosts file into its new home.
            */
        }
        //This can be handled by EasyDownloader and we can use FileDownloader.cs to download
        private static void DownloadAndInstallApache24() {
            /*TODO:
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

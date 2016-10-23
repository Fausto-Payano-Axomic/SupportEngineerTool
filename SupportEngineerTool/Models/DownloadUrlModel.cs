using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using Serilog;
using SupportEngineerTool.Items;
using SupportEngineerTool.Properties;

namespace SupportEngineerTool.Models {
    public class DownloadUrlModel {
        private const string Url = @"https://dl.dropbox.com/s/sxzecgrxw28iyi0/DownloadUrls.xml?dl=0";
        public List<DownloadUrl> XmlOutput = new List<DownloadUrl>();
        public List<DownloadCategory> CategorizedList = new List<DownloadCategory>();
        public bool SuccessfullyDownloadedXml { get; set; }


        public DownloadUrlModel() {
            ParseXmlDownloadFile();
            PopulateCategoryList();
        }

        private void ParseXmlDownloadFile() {
            try {
                XmlDocument downloadDocument = new XmlDocument();
                downloadDocument.Load(Url);
                XmlElement root = downloadDocument.DocumentElement;
                if (root != null) {
                    var nodes = root.ChildNodes;
                    foreach (XmlNode node in nodes) {
                        foreach (XmlElement childElementNode in node) {

                            XmlOutput.Add(new DownloadUrl(node.Name, childElementNode.Name, childElementNode.InnerText));
                        }
                    }
                    SuccessfullyDownloadedXml = true;
                }
            }
            //TODO: Rework exception with serilog structured events.
            catch (WebException connectivityIssue) {
                Log.Logger.Error($"Internet connectivity may not be active or Dropbox may be down: StackException \n {connectivityIssue} \n \n " +
                                 $"InnerException: \n {connectivityIssue.Response}");
                SuccessfullyDownloadedXml = false;
            }
            catch (Exception xmlReadException) {
                Log.Logger.Error($"StackTrace: \n {xmlReadException}");
                SuccessfullyDownloadedXml = false;
            }

        }

        private void PopulateCategoryList() {
            
            foreach (var downloadUrl in XmlOutput) {
                if (CategorizedList.Any(x => x.CategoryName == downloadUrl.Category)) {
                    var item = CategorizedList.FirstOrDefault(x => x.CategoryName == downloadUrl.Category);
                    item?.ContentsOfCategory.Add(downloadUrl);
                }
                else {
                    try {
                        CategorizedList.Add(new DownloadCategory(downloadUrl.Category,
                            new DownloadUrl(downloadUrl.Category, downloadUrl.DisplayedName, downloadUrl.Link)));
                    }
                    catch (Exception categoryAddError) {
                        MessageBox.Show(categoryAddError.ToString());
                    }
                }

            }
        }
    }
}

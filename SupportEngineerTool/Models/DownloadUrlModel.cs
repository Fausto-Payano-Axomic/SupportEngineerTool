﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using SupportEngineerTool.Items;
using SupportEngineerTool.Properties;

namespace SupportEngineerTool.Models {
    public class DownloadUrlModel {
        private const string Url = @"https://dl.dropbox.com/s/sxzecgrxw28iyi0/DownloadUrls.xml?dl=0";
        public List<DownloadUrl> XmlOutput = new List<DownloadUrl>();


        public DownloadUrlModel() {
            ParseXmlDownloadFile();
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
                }
            }
            //TODO: Rework exception with serilog structured events.
            catch (Exception xmlReadException) {
                MessageBox.Show($"Exception:{xmlReadException}");
            }

        }
    }
}

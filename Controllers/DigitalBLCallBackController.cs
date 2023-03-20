using Microsoft.Web.Http;
using Msc.Agency.DigitalBL.Business;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace Msc.Agency.DigitalBL.Controllers
{

    [ApiVersion("1")]
    [RoutePrefix("v{version:apiVersion}/digitalblcallback")]
    public class DigitalBLCallBackController : ApiController
    {
        [HttpPost]
        [Route("receivefile")]
        [SwaggerResponse(statusCode: 201, type: typeof(bool), description: "Created")]
        public async Task<IHttpActionResult> UploadFile()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                //var path = Path.Combine(HttpContext.Current.Server.MapPath("~/uploads"), fileName);
                var path = Path.Combine(ConfigurationManager.AppSettings["Uploads"], fileName);
                file.SaveAs(path);
                string res = string.Empty;
                string transactionIds = string.Empty;
                string fileType = string.Empty;
                string pdffile = string.Empty;
                string InstructionId = string.Empty;
                string ConnectionString = "India";
                if (Path.GetExtension(file.FileName) == ".zip")
                {
                    Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
                    Dictionary <string,byte[]> PdfFILE = new Dictionary <string,byte[]>();
                    using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(file.InputStream))
                    {
                        Ionic.Zip.ZipEntry unZip = zip.Entries.First(x => x.FileName.EndsWith(".pdf"));
                        MemoryStream temper = new MemoryStream();
                        unZip.Extract(temper);
                        PdfFILE.Add("0",temper.ToArray());
                        Ionic.Zip.ZipEntry toZip = zip.Entries.First(x => x.FileName.EndsWith(".xml"));
                        MemoryStream temp = new MemoryStream();
                        toZip.Extract(temp);
                        files.Add("0", temp.ToArray());
                        res = Encoding.ASCII.GetString(files["0"]);
                        pdffile = Convert.ToBase64String(PdfFILE["0"]);
                    }
                }
                else {
                    MemoryStream temp = new MemoryStream();
                    file.InputStream.CopyTo(temp);
                    res = Encoding.ASCII.GetString(temp.ToArray());
                }
                if (res != string.Empty || pdffile != string.Empty) 
                {
                    var serializedXmlNode = RemoveAllNamespaces(res);
                    try
                    {
                        var FM = GetTransactionid(serializedXmlNode, "ForwardedMessage");
                        var OM = GetTransactionid(serializedXmlNode, "OutboundMessage");
                        transactionIds = FM != "" ? FM : OM;
                        fileType = FM != "" ? "ForwardedMessage" : "OutboundMessage";
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(serializedXmlNode);
                        if (fileType == "ForwardedMessage")
                        {
                            XmlNodeList xnList = xml.SelectNodes(fileType + "/TraNotifications/TraNotification/Instruction");
                            foreach (XmlNode xn in xnList)
                            {
                                InstructionId = xn.InnerText;
                            }
                            if (InstructionId == "Surrender")
                            {

                                ConnectionString = "Angola";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await DigitalBLBusiness.LogUpdation(serializedXmlNode, transactionIds, fileType, ConnectionString,pdffile);

                    }
                }
            }

            return Ok(true);

        }

        string GetTransactionid(string xmlDoc, string tag)
        {
            string transactionId = string.Empty;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlDoc);
            XmlNodeList xnList = xml.SelectNodes("/" + tag + "/MessageHeader/Transaction");
            foreach (XmlNode xn in xnList)
            {
                transactionId =  xn["TransactionId"].InnerText;
            }
            return transactionId;
        }

        //string LoadXml(string  res)
        //{
        //    var xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(res);
        //    //xmlDocument.RemoveChild(xmlDocument.FirstChild.FirstChild);
        //    //// var x = xmlDocument.ChildNodes[1];
        //    var x = RemoveAllNamespaces(res);
        //    var serializedXmlNode = JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented, true);
        //    return serializedXmlNode;
        //}

        //string GetReplaced(string serializedXmlNode)
        //{
        //    return serializedXmlNode.Replace("ConnectionModule:", "ConnectionModule");

        //}

        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));
            return xmlDocumentWithoutNs.ToString();
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xmlDocumentWithoutNs.ToString());
            //XElement root = new XElement("root", doc);
            //return root.ToString();
           // return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);

        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

    }
}

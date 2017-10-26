using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MTNCInfo
{
    public partial class PostWeb1 : System.Web.UI.Page
    {
        private string msisdn = "";
        private string amount = "";
        private string token = "";
        private string fileContent = "";
        private string key = string.Empty;
        private Xml rspBody = new Xml();
        SyncOrderEntities db = new SyncOrderEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            string msgtext = "";
            //dbDataContext dbDataContext = new dbDataContext();
            try
            {
                AppUtil.serviceId = (Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : AppUtil.serviceId;
                if (string.IsNullOrEmpty(Request.QueryString["searchkey"]))
                {
                    msgtext = "Empty ID";
                }
                else
                {
                    this.key = Request.QueryString["searchkey"].ToString();
                    if (string.IsNullOrEmpty(Request.QueryString["phone"]))
                    {
                        msgtext = "Empty Phone";
                    }
                    else
                    {
                        this.msisdn = Request.QueryString["phone"].ToString();
                        if (string.IsNullOrEmpty(Request.QueryString["tk"]))
                        {
                            msgtext = "Empty Token";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Request.QueryString["amount"]))
                            {
                                msgtext = "Empty amount";
                            }
                            else
                            {
                                AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == AppUtil.serviceId).TestMode.GetValueOrDefault();
                                token=Request.QueryString["tk"].ToString();
                                amount = Request.QueryString["amount"].ToString().ToLower();
                                string file = Server.MapPath("~\\XMLFile1.xml");
                                //httpWebRequest = (HttpWebRequest)WebRequest.Create(AppUtil.chargingURL);
                                //httpWebRequest.Method = "POST";
                                //httpWebRequest.ContentType = "text/xml";
                                //StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                                fileContent = GetTextFromXMLFile(file, this.key);
                                //streamWriter.WriteLine(fileContent);
                                //streamWriter.Close();
                                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                                string memStream = streamReader.ReadToEnd();
                                string memStreamStr = memStream;
                                string ListKeyVals = "";
                                string ListKeys = "";
                                string[] allKeys = httpWebResponse.Headers.AllKeys;
                                for (int i = 0; i < allKeys.Length; i++)
                                {
                                    string keyVals = allKeys[i];
                                    ListKeys += keyVals;
                                    ListKeyVals += httpWebResponse.Headers.GetValues(keyVals);
                                }
                                string codeStatus = httpWebResponse.StatusCode.ToString();
                                streamReader.Close();
                                XElement xElement = XElement.Parse(memStream);
                                //"http://schemas.xmlsoap.org/soap/envelope/";
                                //"http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local";
                                XElement xElement2 = (
                                    from iel in
                                        (
                                            from el in xElement.Descendants()
                                            where el.Name.Namespace == "http://schemas.xmlsoap.org/soap/envelope/"
                                            select el).Descendants<XElement>()
                                    where iel.Name.Namespace == "http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local"
                                    select iel).FirstOrDefault<XElement>();
                                if (xElement2 != null && xElement2.Name.LocalName == "chargeAmountResponse")
                                {
                                    memStream = "OK";
                                }
                                else
                                {
                                    memStream = codeStatus;
                                }
                                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
							{
								this.fileContent,
								memStreamStr,
								memStream,
								this.key
							});
                                msgtext = memStream;
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                AppUtil.LogFileWrite(ex.ToString());

                using (Stream responseStream = ex.Response.GetResponseStream())
                {
                    using (StreamReader streamReader2 = new StreamReader(responseStream))
                    {
                        msgtext = streamReader2.ReadToEnd();
                    }
                }
                string text6 = msgtext;
                XElement xElement3 = XElement.Parse(msgtext);
                XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
                XElement xElement4 = (
                    from iel in
                        (
                            from el in xElement3.Descendants()
                            where el.Name.Namespace == "http://schemas.xmlsoap.org/soap/envelope/"
                            select el).Descendants(ns + "Fault")
                    select iel).Descendants("faultcode").FirstOrDefault<XElement>();
                if (xElement4 != null && xElement4.Name.LocalName != "")
                {
                    msgtext = xElement4.Value;
                }
                else
                {
                    msgtext = "WEBERROR";
                }
                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
				{
					this.fileContent,
					text6,
					msgtext,
					this.key
				});
            }
            catch (Exception ex2)
            {
                AppUtil.LogFileWrite(ex2.ToString());
                msgtext = "Exception:<br/>" + ex2.Message.ToString() + "<br/>Trace Error" + ex2.StackTrace.ToString();
                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
				{
					this.fileContent,
					msgtext,
					"Exception",
					this.key
				});
            }
            finally
            {
                if (httpWebRequest != null)
                {
                    httpWebRequest.GetRequestStream().Close();
                }
                if (httpWebResponse != null)
                {
                    httpWebResponse.GetResponseStream().Close();
                }
                //if (dbDataContext != null)
                //{
                //    dbDataContext.Dispose();
                //}
                base.Response.Write(msgtext);
            }
        }
        private string GetTextFromXMLFile(string file, string refKey)
        {
            StreamReader streamReader = new StreamReader(file);
            string xmlRequest = streamReader.ReadToEnd();
            streamReader.Close();
            
            return string.Format(xmlRequest, new object[]
			{
				AppUtil.SPID,
				AppUtil.MD5Hashed(AppUtil.SPID + AppUtil.Password + DateTime.Now.ToString(AppUtil.TimeFormat)),
				AppUtil.serviceId,
				DateTime.Now.ToString(AppUtil.TimeFormat),
				this.msisdn,
				this.amount,
				refKey,
                this.token
			});
        }
        private string GetTextFromXMLFile()
        {
            string path = base.Server.MapPath("~\\XMLBad.xml");
            StreamReader streamReader = new StreamReader(path);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            return result;
        }
        public Stream ToStream(string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
        private Xml XMLed(string value)
        {
            Xml result = new Xml();
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(value);
            }
            catch
            {
                XmlDocument xmlDocument2 = new XmlDocument();
                xmlDocument2.LoadXml(string.Format("<root>{0}</root>", value));
            }
            return result;
        }
    }
}
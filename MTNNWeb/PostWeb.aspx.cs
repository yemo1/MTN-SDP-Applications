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
using System.Xml.Serialization;
using System.Web.Services.Protocols;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;


namespace MTNNWeb
{
    public partial class PostWeb : System.Web.UI.Page
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
            string msgResp = "";
            try
            {
                //http://368795-web2/MTNNWeb/PostWeb.aspx?svcID=234012000004575&searchkey=144&phone=2348030643007&amount=1000&tk=2
                AppUtil.serviceId = (Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : AppUtil.serviceId;
                if (string.IsNullOrEmpty(Request.QueryString["searchkey"]))
                {
                    msgResp = "Empty ID";
                }
                else
                {
                    this.key = Request.QueryString["searchkey"].ToString();
                    if (string.IsNullOrEmpty(Request.QueryString["phone"]))
                    {
                        msgResp = "Empty Phone";
                    }
                    else
                    {
                        this.msisdn = Request.QueryString["phone"].ToString();
                        if (string.IsNullOrEmpty(Request.QueryString["tk"]))
                        {
                            msgResp = "Empty Token";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Request.QueryString["amount"]))
                            {
                                msgResp = "Empty amount";
                            }
                            else
                            {
                                //CASVC.AmountChargingService 
                                AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == AppUtil.serviceId).TestMode.GetValueOrDefault();

                                token = Request.QueryString["tk"].ToString();
                                amount = Request.QueryString["amount"].ToString().ToLower();
                                //string file = Server.MapPath("~\\XMLFile1.xml");
                                //httpWebRequest = (HttpWebRequest)WebRequest.Create(AppUtil.chargingURL);
                                //httpWebRequest.Method = "POST";
                                //httpWebRequest.ContentType = "text/xml";
                                //StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                                //fileContent = GetTextFromXMLFile(file, this.key);

                                string memStreamStr = "", memStream = "";

                                RequestSOAPHeader authentication = new RequestSOAPHeader();
                                string timestamp = AppUtil.GetTimeStamp();
                                authentication.spId = AppUtil.SPID;
                                authentication.spPassword = AppUtil.MD5Hashed(AppUtil.SPID + AppUtil.Password + timestamp);
                                authentication.serviceId = AppUtil.serviceId;
                                authentication.timeStamp = timestamp;
                                //authentication.oauth_token = token;
                                authentication.OA = msisdn;

                                CASVC.AmountChargingService cvc = new CASVC.AmountChargingService();
                                cvc.authCred = authentication;

                                CASVC.chargeAmount casg = new CASVC.chargeAmount();
                                //AppUtil.LogFileWrite(amount);
                                casg.charge = new CASVC.ChargingInformation();
                                casg.charge.amount = Convert.ToDecimal(Convert.ToInt32(amount));
                                casg.charge.amountSpecified = true;
                                casg.charge.description = "charge";
                                casg.charge.currency = "NGN";
                                //casg.charge.code = "566"; not require for now
                                casg.endUserIdentifier = "+" + msisdn;
                                casg.referenceCode = key;

                                CASVC.chargeAmountResponse res = new CASVC.chargeAmountResponse();


                                fileContent = SerialiseRequest(casg);
                                //AppUtil.LogFileWrite(fileContent);
                                res = cvc.chargeAmount(casg);
                                if (res != null)
                                {
                                    memStream = "OK";
                                    memStreamStr = SerialiseResponse(res);
                                }

                                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
							{
								fileContent,
								memStreamStr,
								memStream,
								key
							});

                                msgResp = memStream;
                            }
                        }
                    }
                }
            }
            catch (SoapException sex)
            {
                AppUtil.LogFileWrite(sex.ToString());
                // The variable 'sex' can access the exception's information.
                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
							{
								fileContent,
								sex.Detail.InnerXml,
								sex.Code.Name,
								key
							});
            }
            catch (WebException wex)
            {
                AppUtil.LogDBWrite(wex.ToString(), "Response");

                using (Stream responseStream = wex.Response.GetResponseStream())
                {
                    using (StreamReader streamReader2 = new StreamReader(responseStream))
                    {
                        msgResp = streamReader2.ReadToEnd();
                    }
                }
                string text6 = msgResp;
                XElement xElement3 = XElement.Parse(msgResp);
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
                    msgResp = xElement4.Value;
                }
                else
                {
                    msgResp = "WEBERROR";
                }
                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
				{
					this.fileContent,
					text6,
					msgResp,
					this.key
				});
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
                msgResp = "Exception:<br/>" + ex.Message.ToString() + "<br/>Trace Error" + ex.StackTrace.ToString();
                db.Database.ExecuteSqlCommand("UPDATE ChargingTransaction SET Request={0},Response={1},Status={2} WHERE ID={3}", new object[]
				{
					this.fileContent,
					msgResp,
					"Exception",
					this.key
				});
            }
            finally
            {
                if (db != null)
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                        db.Database.Connection.Close();

                    db.Dispose();
                }
                Response.Write(msgResp);
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

        private string SerialiseRequest(CASVC.chargeAmount cvc)
        {
            if (cvc != null)
            {

                //MemoryStream mem = new MemoryStream();
                //// Serializes a class named XYZ as a SOAP message.
                //XmlTypeMapping myTypeMapping = (new SoapReflectionImporter().ImportTypeMapping(typeof(CASVC.chargeAmount)));
                //XmlSerializer mySerializer = new XmlSerializer(myTypeMapping);
                //mySerializer.Serialize(mem, cvc); StreamReader memr = new StreamReader(mem);
                //return memr.ReadToEnd();

                using (MemoryStream Stream = new MemoryStream())
                {
                    SoapFormatter Serializer = new SoapFormatter();
                    Serializer.Serialize(Stream, cvc);
                    Stream.Flush();
                    return UTF8Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
                }

            }
            else return "";
        }
        private string SerialiseResponse(CASVC.chargeAmountResponse cvc)
        {
            if (cvc != null)
            {

                //MemoryStream mem = new MemoryStream();
                //// Serializes a class named XYZ as a SOAP message.
                //XmlTypeMapping myTypeMapping = (new SoapReflectionImporter().ImportTypeMapping(typeof(CASVC.chargeAmount)));
                //XmlSerializer mySerializer = new XmlSerializer(myTypeMapping);
                //mySerializer.Serialize(mem, cvc); StreamReader memr = new StreamReader(mem);
                //return memr.ReadToEnd();

                using (MemoryStream Stream = new MemoryStream())
                {
                    SoapFormatter Serializer = new SoapFormatter();
                    Serializer.Serialize(Stream, cvc);
                    Stream.Flush();
                    return UTF8Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
                }

            }
            else return "";
        }
    }
}
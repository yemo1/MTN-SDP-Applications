using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace MTNCChat
{
    public partial class TextPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Type[] types = new Type[] { typeof(DSync.NamedParameter) };
            XmlSerializer reader = new XmlSerializer(typeof(SyncOrderRequest));
            System.IO.StreamReader file = new System.IO.StreamReader(Server.MapPath("XMLFileTest.xml"));


            SyncOrderRequest overview = new SyncOrderRequest();
            overview = (SyncOrderRequest)reader.Deserialize(file);



            DSync.DataSyncSoapClient dcl = new DSync.DataSyncSoapClient();
            DataSync b = new DataSync();

            DSync.NamedParameterList dv = new DSync.NamedParameterList();

            DSync.NamedParameter bi= new DSync.NamedParameter();

            foreach (NamedParameter it in overview.extensionInfo.item)
            {
                bi = new DSync.NamedParameter();
                bi.key=it.key;bi.value=it.value;
                dv.Add(bi);
            }
            DSync.UserID du =new DSync.UserID();
            du.ID = overview.userID.ID; du.type = overview.userID.type;

            DSync.SyncResponse resp = dcl.syncOrderRelation(du, overview.spID, overview.productID, overview.serviceID, overview.serviceList,
                overview.updateType, overview.updateTime, overview.updateDesc, overview.effectiveTime, overview.expiryTime, dv);
            string rsp = resp.result.ToString();
            Response.Write(resp.result);
            Response.Write("<br/>");
            Response.Write(rsp);
            file.Close();
            file.Dispose();

            //XmlSerializer serializer = new XmlSerializer(typeof(ConsultarSituacaoLoteRpsResposta), "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd");
            //var obj1 = (ConsultarSituacaoLoteRpsResposta)serializer.Deserialize(new StringReader(xml1));
            //var obj2 = (ConsultarSituacaoLoteRpsResposta)serializer.Deserialize(new StringReader(xml2));
        }
    }
    public class ConsultarSituacaoLoteRpsResposta
    {
        public int NumeroLote { set; get; }
        public int Situacao { set; get; }
        public List<MensagemRetorno> ListaMensagemRetorno { get; set; }
    }
    public class MensagemRetorno
    {
        public string Codigo { set; get; }
        public string Mensagem { set; get; }
        public string Correcao { set; get; }
    }

}
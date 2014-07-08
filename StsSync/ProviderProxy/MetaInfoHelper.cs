using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;

namespace ProviderProxy
{
    public class MetaInfoHelper
    {
        public const string METAINFO = "MetaInfo";
        public static XmlNodeList GetMetaInfoFields(XmlElement xelMethod,XmlNamespaceManager nsm)
        {
            return xelMethod.SelectNodes("s:Field[@Name='" + METAINFO + "']", nsm);
        }
        public static XmlElement GetMetaInfoXml(XmlElement xelMethod, XmlNamespaceManager nsm)
        {
            XmlDocument xDoc = new XmlDocument(nsm.NameTable);
            XmlElement xelMetaInfo = xDoc.CreateElement(METAINFO);

            XmlNodeList metaInfoFields = GetMetaInfoFields(xelMethod, nsm);

            foreach (XmlElement xelField in metaInfoFields)
            {
                xelMetaInfo.AppendChild(xDoc.ImportNode(xelField, true));
            }

            return xelMetaInfo;
        }
        public static XmlElement GetMetaInfoXmlFromDataRow(DataRow dr)
        {
            XmlDocument xDoc = new XmlDocument();

            if (dr == null || dr[METAINFO] == null || dr[METAINFO] == DBNull.Value)
                return null;

            try
            {
                xDoc.LoadXml(dr[METAINFO].ToString());
            }
            catch(XmlException exml) { return null; }

            return xDoc.DocumentElement;
        }

        public static void AddMetaInfoFromMetaInfoXml(XmlElement parentNode, XmlElement xmlMetaInfo){
            XmlAttributeCollection xAtts = parentNode.Attributes;
            XmlNodeList xnlFields = xmlMetaInfo.ChildNodes;
            foreach(XmlElement xelField in xnlFields){
                Field fld = null;
                if (xelField.HasAttribute("Property"))
                {
                    fld = new Field(xelField.GetAttribute("Property"));
                    fld.IsPropBagField = true;
                }
                else
                {
                    fld = new Field(xelField.GetAttribute("Name"));
                }
                if(!parentNode.HasAttribute(fld.OWSName))
                    parentNode.SetAttribute(fld.OWSName,xelField.InnerText.Trim());
            }
        }

        public static string GetMetaInfoColName(StringDictionary fieldNameMapping)
        {
            string metaInfoColName = METAINFO;
            if (fieldNameMapping.ContainsValue(METAINFO))
            {
                foreach (string key in fieldNameMapping.Keys)
                {
                    if (fieldNameMapping[key] == METAINFO)
                        return key;
                }
            }

            return metaInfoColName;
        }

    }
}

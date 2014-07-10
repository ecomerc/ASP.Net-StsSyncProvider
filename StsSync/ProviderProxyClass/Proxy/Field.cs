using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ProviderProxy
{
    public class Field
    {
        private const string FIELD_FIELDNAME = "Name";
        private const string FIELD_FIELDDISPLAYNAME = "DisplayName";
        //private const string FIELD_FIELDINTERNALNAME = "StaticName";
        private const string FIELD_FIELDCOLNAME = "ColName";
        private const string FIELD_FIELDREADONLY = "ReadOnly";
        private const string FIELD_FIELDTYPE = "Type";

        public bool IsPropBagField = false;


        private Dictionary<string, object> m_attributes;
        public bool ReadOnly{
            get { return GetAttribute<bool>(FIELD_FIELDREADONLY, false); }
        }
        public string Type
        {
            get { return GetAttribute(FIELD_FIELDTYPE, "Text"); }
        }
        public string ColName
        {
            get { return GetAttribute(FIELD_FIELDCOLNAME).ToString(); }
        }
        public string Name
        {
            get { return GetAttribute(FIELD_FIELDNAME).ToString(); }
        }
        public string DisplayName
        {
            get { return GetAttribute(FIELD_FIELDDISPLAYNAME).ToString(); }
        }

        public object Value;
        public string OWSName
        {
            get
            {
                return "ows_" + (IsPropBagField?"MetaInfo_":string.Empty).ToString() + Name;
            }
        }

        public Field()
        {

        }
        public Field(string fieldName):this()
        {
            SetAttribute(FIELD_FIELDNAME, fieldName);
        }
        public Field(string fieldName, string fieldDisplayName):this( fieldName)
        {
            SetAttribute(FIELD_FIELDDISPLAYNAME, fieldDisplayName);
            //SetAttribute(FIELD_FIELDINTERNALNAME, fieldInternalName);
        }
        public Field(string fieldName, string fieldDisplayName, string fieldColName):this( fieldName,  fieldDisplayName)
        {
            SetAttribute(FIELD_FIELDCOLNAME, fieldColName);   
        }
        public Field(XmlElement xelFieldElement)
        {
            if (xelFieldElement.LocalName.ToLower() == "field")
            {
                foreach (XmlAttribute xAttrib in xelFieldElement.Attributes)
                {
                    SetAttribute(xAttrib.LocalName, xAttrib.Value);
                }
            }
        }
        public string FieldDefXml
        {
            get{
                StringBuilder sXml = new StringBuilder();
                sXml.Append("<Field xmlns=\"http://schemas.microsoft.com/sharepoint/soap/\"");
                foreach (string attrib in m_attributes.Keys)
                {
                    sXml.Append(" " + attrib + "=\"");
                    sXml.Append(GetAttribute(attrib));
                    sXml.Append("\"");
                }
                sXml.Append(" />");
                return sXml.ToString();
            }
        }
        public XmlElement GetFieldDefXml(XmlDocument contextDoc)
        {
            XmlElement fieldEl = contextDoc.CreateElement("Field", "http://schemas.microsoft.com/sharepoint/soap/");
            foreach (string attrib in m_attributes.Keys)
            {
                fieldEl.SetAttribute(attrib,GetAttribute<string>(attrib,string.Empty));
            }
            return fieldEl;
        }
        public string FieldValXml
        {
            get
            {
                return OWSName + "=\"" + Value + "\"";
            }
        }

        internal void SetAttribute(string name, object value){
            if (m_attributes == null)
                m_attributes = new Dictionary<string, object>();

            if (m_attributes.ContainsKey(name))
            {
                m_attributes[name] = value;
            }
            else
            {
                m_attributes.Add(name, value);
            }
        }
        public object GetAttribute(string sAttributeName, object objValueIfNull)
        {
            if (m_attributes == null)
                return objValueIfNull;

            return m_attributes.ContainsKey(sAttributeName) ? 
                (m_attributes[sAttributeName] == null?objValueIfNull:m_attributes[sAttributeName]) : 
                objValueIfNull;
        }
        public object GetAttribute(string sAttributeName)
        {
            return GetAttribute(sAttributeName, null);
        }

        /// <summary>
        /// Cannot return null because T is non-nullable.
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="sAttributeName">Name of the attribute to get</param>
        /// <param name="tValueIfNull">Value of type T to return if the attribute is missing or null</param>
        /// <returns>Value of attribute casted to type T</returns>
        public T GetAttribute<T>(string sAttributeName, T tValueIfNull)
        {
            object val = GetAttribute(sAttributeName);

            bool valIsNull = (null == val);

            T tVal = valIsNull ? tValueIfNull : _DoTypeParse<T>(val);

            return tVal;
        }

        public T GetValue<T>()
        {
            return _DoTypeParse<T>(Value);
        }
        public string GetValue()
        {
            if (null == Value) return null;

            return Value.ToString();
        }
        public bool GetBoolValue()
        {
            return GetValue<bool>();
        }
        public DateTime GetDateTimeValue()
        {
            return GetValue<DateTime>();
        }

        private T _DoTypeParse<T>(object val)
        {
            if (val is T)
                return (T)val;

            switch (typeof(T).Name.ToLower())
            {
                case "bool":
                case "boolean":
                    if (string.IsNullOrEmpty(val.ToString()))
                        return (T)(object)false;

                    return (T)(object)bool.Parse(val.ToString());
                case "datetime":
                    return (T)(object)DateTime.Parse(val.ToString());
                case "guid":
                    return (T)(object)new Guid(val.ToString());
                case "double":
                    return (T)(object)double.Parse(val.ToString());
                case "single":
                    return (T)(object)Single.Parse(val.ToString());
                case "int":
                    return (T)(object)int.Parse(val.ToString());
                case "long":
                    return (T)(object)long.Parse(val.ToString());
                case "short":
                    return (T)(object)short.Parse(val.ToString());
                case "int32":
                    return (T)(object)Int32.Parse(val.ToString());
                case "int64":
                    return (T)(object)Int64.Parse(val.ToString());
                case "byte":
                    return (T)(object)byte.Parse(val.ToString());
                default:
                    return (T)val;
            }

        }

        
                

    }
    public class FieldDictionary : Dictionary<string, Field> { };
}

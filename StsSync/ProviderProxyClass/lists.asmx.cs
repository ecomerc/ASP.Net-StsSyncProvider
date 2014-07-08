using System;
using System.Collections;
using System.ComponentModel;
//using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;

namespace ProviderProxy {
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://schemas.microsoft.com/sharepoint/soap/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService, IListsSoap {
        public static readonly Guid WEB_ID = new Guid("{409F0498-67EA-4120-B5AC-A7FE87828967}");

        [WebMethod()]
        public string ATestMethod() {

            //ProviderManager pm = new ProviderManager();
            //IProvider prov = pm.GetProvider(new Guid("{7765B84F-6D32-4d31-B28E-6BC615D2F187}"));
            //ChangeKey ck = new ChangeKey("1; 1; 0ac863f6-3a07-4d5e-bd07-78bebb0a34f1; 633033246547900000; 217 ");
            //ChangeKey ck = new ChangeKey(1, 1, Guid.NewGuid(), DateTime.Now, 23); 
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendLine("ChangeEventType = " + ck.ChangeEventType.ToString());
            //sb.AppendLine("ChangeNumber = " + ck.ChangeNumber.ToString());
            //sb.AppendLine("ChangeObjectType = " + ck.ChangeObjectType.ToString());
            //sb.AppendLine("ChangeTime = " + ck.ChangeTime.ToString());
            //sb.AppendLine("ObjectGuid = " + ck.ObjectGuid.ToString());
            //sb.AppendLine("ChangeKey = " + ck.ToString());

            return null;
        }

        static XmlDocument _doc = null;
        static XmlNamespaceManager _nsm = null;
        static XmlNamespaceManager _snsm = null;
        static XmlNamespaceManager snsm {
            get {
                if (_snsm == null) { object throwaway = _Doc; }
                return _snsm;
            }
        }
        private static XmlDocument _Doc {
            get {
                if (_doc == null) {
                    _doc = new XmlDocument();
                    _nsm = new XmlNamespaceManager(_doc.NameTable);
                    _nsm.AddNamespace(string.Empty, "http://schemas.microsoft.com/sharepoint/soap/");
                    _snsm = new XmlNamespaceManager(_doc.NameTable);
                    _snsm.AddNamespace("s", _nsm.DefaultNamespace);
                }

                return _doc;
            }


        }
        #region IListsSoap Members



        public System.Xml.XmlNode GetList(string listName) {
            //List name should correspond to the provider ID if the stssync link was constructed correctly.
            Guid listId = new Guid(listName); //we'll just crash if it's not a Guid
            XmlElement listElement = _Doc.CreateElement("List", _nsm.DefaultNamespace);
            ProviderManager pm = new ProviderManager();
            IProvider prov = pm.GetIProvider(listId);
            IProvider iProv = pm.GetIProvider(listId);



            ListsHelper.FillListAttributes(listName, listElement, prov, this.Context.Request.ApplicationPath);

            XmlElement fieldsElement = NewElement(listElement, "Fields", null, true);
            //listElement.AppendChild(fieldsElement);

            List<Field> fields = ListsHelper.GetFieldsForListType(iProv.GetProviderType(listId));

            foreach (Field field in fields) {
                //Works, but I'm going to try something else.
                //XmlElement fieldElement = _Doc.CreateElement("Field", listElement.NamespaceURI);

                //XmlDocument fDoc = new XmlDocument();
                //fDoc.LoadXml(field.FieldDefXml);
                //foreach (XmlAttribute attrib in fDoc.DocumentElement.Attributes)
                //{
                //    fieldElement.Attributes.Append((XmlAttribute)_Doc.ImportNode(attrib.Clone(),true));
                //}

                //fieldsElement.AppendChild(fieldElement);

                fieldsElement.AppendChild(field.GetFieldDefXml(_Doc));

            }

            /*
           <RegionalSettings>
            <Language>1033</Language>
            <Locale>1033</Locale>
            <AdvanceHijri>0</AdvanceHijri>
            <CalendarType>1</CalendarType>
            <Time24>False</Time24>
            <TimeZone>300</TimeZone>
            <SortOrder>2070</SortOrder>
            <Presence>True</Presence>
          </RegionalSettings>
          <ServerSettings>
            <ServerVersion>12.0.0.6219</ServerVersion>
            <RecycleBinEnabled>True</RecycleBinEnabled>
            <ServerRelativeUrl>/sites/stssync</ServerRelativeUrl>
          </ServerSettings>
             */

            XmlElement regionalSettingsNode = NewElement(listElement, "RegionalSettings", null, true);
            NewElement(regionalSettingsNode, "Language", "1033");
            NewElement(regionalSettingsNode, "Locale", "1033");
            NewElement(regionalSettingsNode, "AdvanceHijri", "0");
            NewElement(regionalSettingsNode, "CalendarType", "1");
            NewElement(regionalSettingsNode, "Time24", "False");
            NewElement(regionalSettingsNode, "TimeZone", "300");
            NewElement(regionalSettingsNode, "SortOrder", "2070");
            NewElement(regionalSettingsNode, "Presence", "True");

            XmlElement serverSettings = NewElement(listElement, "ServerSettings", null, true);
            NewElement(serverSettings, "ServerVersion", "12.0.0.6219");
            NewElement(serverSettings, "RecycleBinEnabled", "True");
            NewElement(serverSettings, "ServerRelativeUrl", this.Context.Request.ApplicationPath);



            return listElement;

        }

        private XmlElement NewElement(XmlElement parentElement, string nodeName, string innerXml) {
            XmlElement retElement = NewElement(parentElement, nodeName, null, true);
            if (innerXml != null) {
                retElement.InnerText = innerXml;
            }
            return retElement;
        }
        private XmlElement NewElement(XmlElement parentElement, string nodeName, XmlElement refNode, bool appearBefore) {
            XmlElement retElement = NewElement(parentElement, nodeName);
            if (refNode != null) {
                if (appearBefore)
                    parentElement.InsertBefore(retElement, refNode);
                else
                    parentElement.InsertAfter(retElement, refNode);
            } else {
                parentElement.AppendChild(retElement);
            }
            return retElement;
        }
        private XmlElement NewElement(XmlElement parentElement, string nodeName) {
            return parentElement.OwnerDocument.CreateElement(nodeName, parentElement.NamespaceURI);
        }


        public System.Xml.XmlNode GetListAndView(string listName, string viewName) {
            throw new NotImplementedException();
        }

        public void DeleteList(string listName) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode AddList(string listName, string description, int templateID) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode AddListFromFeature(string listName, string description, Guid featureID, int templateID) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode UpdateList(string listName, System.Xml.XmlNode listProperties, System.Xml.XmlNode newFields, System.Xml.XmlNode updateFields, System.Xml.XmlNode deleteFields, string listVersion) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetListCollection() {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetListItems(string listName, string viewName, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, string rowLimit, System.Xml.XmlNode queryOptions, string webID) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetListItemChanges(string listName, System.Xml.XmlNode viewFields, string since, System.Xml.XmlNode contains) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetListItemChangesSinceToken(string listName, string viewName, System.Xml.XmlNode query, System.Xml.XmlNode viewFields, string rowLimit, System.Xml.XmlNode queryOptions, string changeToken, System.Xml.XmlNode contains) {
            //List name should correspond to the provider ID if the stssync link was constructed correctly.
            const char sTab = '\t';
            System.Diagnostics.Debug.Print("GetListItemChangesSinceToken called with params:\n" +
                sTab + "listName = " + ((null == listName) ? string.Empty : listName).ToString() + "\n" +
                sTab + "viewName = " + ((null == viewName) ? string.Empty : viewName).ToString() + "\n" +
                sTab + "query = " + ((query == null) ? string.Empty : query.OuterXml.ToString()).ToString() + "\n" +
                sTab + "viewFields = " + ((null == viewFields) ? string.Empty : viewFields.OuterXml.ToString()).ToString() + "\n" +
                sTab + "rowLimit = " + ((null == rowLimit) ? string.Empty : rowLimit.ToString()).ToString() + "\n" +
                sTab + "queryOptions = " + ((null == queryOptions) ? string.Empty : queryOptions.OuterXml.ToString()).ToString() + "\n" +
                sTab + "changeToken = " + ((null == changeToken) ? string.Empty : changeToken).ToString() + "\n" +
                sTab + "contains = " + ((contains == null) ? string.Empty : contains.OuterXml.ToString()).ToString() + "\n=======================================");
            ProviderManager pm = new ProviderManager();
            Guid listId = new Guid(listName); //we'll just crash if it's not a Guid
            IProvider iProv = pm.GetIProvider(listId);

            ChangeKey changeKey = null;
            if (changeToken != null)
                changeKey = new ChangeKey(changeToken);
            IEnumerable<object> changes = iProv.GetUpdatesSinceToken(listId, changeKey);
            ListType listType = iProv.GetProviderType(listId);
            StringDictionary fieldMappings = iProv.GetFieldMappingsForListType(listId, listType);

            int intRowLimit = (null == rowLimit) ? 0 : Int32.Parse(rowLimit);
            int startRow = 0;
            string requestThread = null;
            if (queryOptions != null) {
                XmlElement xelPaging = (XmlElement)queryOptions.SelectSingleNode("//s:Paging", snsm);
                string sPosNext = (xelPaging != null) ? xelPaging.GetAttribute("ListItemCollectionPositionNext") : null;
                if (sPosNext != null && sPosNext.IndexOf("&") > -1) {
                    string[] sPosNextParts = sPosNext.Split('&');
                    requestThread = sPosNextParts[0];
                    startRow = (sPosNext != null) ? Int32.Parse(sPosNextParts[1]) : 0;
                }
            }


            IEnumerable<object> wssDS = changes;
            try {
                object cacheValue = null;

                if (requestThread != null) {
                    cacheValue = this.Context.Cache.Get(requestThread);
                }

                if (cacheValue != null) {
                    wssDS = (IEnumerable<object>)cacheValue;
                } else {
                    if (wssDS != null) {
                        requestThread = Guid.NewGuid().ToString("N");
                        if (fieldMappings.Count == 0) {
                            //wssDS = changes;
                        } else {
                            wssDS = DataSetMapper.MapCustomDSToWSSDS(changes, listType.ToString(), fieldMappings, listType);
                        }
                        this.Context.Cache.Insert(requestThread, wssDS, null, DateTime.MaxValue, new TimeSpan(0, 10, 0));
                    }
                }
            } catch {
                Debug.WriteLine("error");
            }
            /*
            <listitems MinTimeBetweenSyncs='0' RecommendedTimeBetweenSyncs='180' MaxBulkDocumentSyncSize='500' AlternateUrls='http://initechwss/' EffectivePermMask='FullMask' xmlns:s='uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882'
     xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882'
     xmlns:rs='urn:schemas-microsoft-com:rowset'
     xmlns:z='#RowsetSchema'>
            */

            XmlElement listItems = _Doc.CreateElement("listitems", _nsm.DefaultNamespace);
            listItems.SetAttribute("MinTimeBetweenSyncs", "0");
            listItems.SetAttribute("RecommendedTimeBetweenSyncs", "180");
            listItems.SetAttribute("MaxBulkDocumentSyncSize", "500");
            listItems.SetAttribute("EffectivePermMask", "FullMask");
            listItems.SetAttribute("xmlns:s", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
            listItems.SetAttribute("xmlns:dt", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");
            listItems.SetAttribute("xmlns:rs", "urn:schemas-microsoft-com:rowset");
            listItems.SetAttribute("xmlns:z", "#RowsetSchema");

            XmlElement xelChanges = NewElement(listItems, "Changes", null, true);
            xelChanges.SetAttribute("LastChangeToken", new ChangeKey(1, 3, iProv.ID, DateTime.Now, 1).ToString());


            XmlElement xelDataNode = ListsHelper.DataSetToDataNode(listItems, wssDS, listType, intRowLimit, startRow, requestThread);
            if (xelDataNode.HasAttribute("ListItemCollectionPositionNext")) {
                xelChanges.SetAttribute("MoreChanges", "TRUE");
            }

            return listItems;

        }

        public System.Xml.XmlNode UpdateListItems(string listName, System.Xml.XmlNode updates) {
            XmlElement xelResults = _Doc.CreateElement("Results", _nsm.DefaultNamespace);

            ProviderManager pm = new ProviderManager();
            Guid listId = new Guid(listName); //we'll just crash if it's not a Guid
            IProvider iProv = pm.GetIProvider(listId);

            ListType listType = iProv.GetProviderType(iProv.ID);
            StringDictionary fieldListMapping = iProv.GetFieldMappingsForListType(iProv.ID, listType);
            List<Field> fieldList = ListsHelper.GetFieldsForListType(listType);
            FieldDictionary fieldDictionary = ListsHelper.GetFieldDictionaryForListType(listType);

            XmlElement xelUpdates = (XmlElement)updates;

            string onError = xelUpdates.GetAttribute("OnError");
            string listVersion = xelUpdates.GetAttribute("ListVersion");
            string viewName = xelUpdates.GetAttribute("ViewName");

            XmlNodeList xnlMethods = xelUpdates.SelectNodes("//s:Method", snsm);

            Type dataType = iProv.GetEmptyDataSet(iProv.ID);
            PropertyInfo[] properties = dataType.GetProperties();
            //DataSet wssDS = DataSetMapper.MapCustomDSToWSSDS(updatesDS, listType.ToString(), iProv.GetFieldMappingsForListType(prov.ID, listType), listType);
            //DataTable emptyDT = emptyDS.Tables[0];
            XmlElement xelZRow = null;
            foreach (XmlElement xelMethod in xnlMethods) {
                XmlElement xelResult = NewElement(xelResults, "Result", null, true);
                string sMethodID = xelMethod.GetAttribute("ID");
                string sMethod = xelMethod.GetAttribute("Cmd");
                xelResult.SetAttribute("ID", sMethodID + "," + sMethod);

                uint errCode = 0;
                string errText = null;
                object resultDR;

                try {
                    XmlNodeList xnlFields = xelMethod.SelectNodes("s:Field[@Name != 'MetaInfo']", snsm);
                    XmlElement xelMetaInfo = MetaInfoHelper.GetMetaInfoXml(xelMethod, snsm);
                    XmlElement xelIDField = (XmlElement)xelMethod.SelectSingleNode("s:Field[@Name='ID']", snsm);
                    int id = 0;
                    string sID = xelIDField.InnerText;
                    if (sID.ToLower() != "new") {
                        try //to get the ID -- won't be there for the "New" method.
                        {
                            id = int.Parse(xelIDField.InnerText);
                        } catch (FormatException ex) {
                            id = 0;
                        }
                    }
                    object updateDR = null;
                    Type objectType = iProv.GetEmptyDataSet(iProv.ID);
                    //PropertyInfo[] objectProperties = objectType.GetProperties();

                    //PropertyInfo propertyInfo = ship.GetType().GetProperty("Latitude");

                    switch (sMethod.ToLower()) {
                        case "update":

                        case "new":
                            if (sMethod.ToLower() == "update") {
                                updateDR = iProv.GetSingleRow(iProv.ID, id);
                            } else { //new

                                //DataTable dt = iProv.GetEmptyDataSet(iProv.ID).Tables[0];
                                updateDR = Activator.CreateInstance(objectType);
                                //dt.Rows.Add(updateDR);
                            }



                            if (updateDR != null) {
                                foreach (XmlElement updateField in xnlFields) {

                                    string wssName = updateField.GetAttribute("Name");
                                    string fieldName = DataSetMapper.GetCustomFieldName(wssName, fieldListMapping);

                                    PropertyInfo propertyInfo = objectType.GetProperty(fieldName);

                                    if (propertyInfo != null) {
                                        if (fieldDictionary[wssName].ReadOnly)
                                            continue; //don't update readonly fields
                                        //Type typeOfColumn = emptyDT.Columns[fieldName].DataType;
                                        object fieldValue = DataSetMapper.GetTypedValue(propertyInfo.PropertyType, updateField.InnerText);
                                        //updateDR[fieldName] = fieldValue;
                                        propertyInfo.SetValue(updateDR, Convert.ChangeType(fieldValue, propertyInfo.PropertyType), null);
                                    } else {
                                        xelMetaInfo.AppendChild(xelMetaInfo.OwnerDocument.ImportNode(updateField, true));
                                    }

                                }

                                if (xelMetaInfo != null) {
                                    PropertyInfo propertyInfo = objectType.GetProperty(MetaInfoHelper.GetMetaInfoColName(fieldListMapping));

                                    if (propertyInfo != null) {
                                        propertyInfo.SetValue(updateDR, Convert.ChangeType(xelMetaInfo.OuterXml, propertyInfo.PropertyType), null);
                                    }
                                }

                            }

                            resultDR = iProv.Update(iProv.ID, updateDR);

                            xelZRow = ListsHelper.CreateZRow(xelResult, fieldList, fieldListMapping, resultDR);


                            if (xelMetaInfo != null) {
                                MetaInfoHelper.AddMetaInfoFromMetaInfoXml(xelZRow, xelMetaInfo);
                            }



                            xelResult.AppendChild(xelZRow);
                            break;
                        case "delete":
                            iProv.Delete(iProv.ID, updateDR);
                            break;
                    }


                } catch (ApplicationException ex) {
                    errCode = 0x8007DA7E;
                    errText = ex.Message;
                }

                XmlElement xelErrorCode = NewElement(xelResult, "ErrorCode", xelZRow, true);
                string sErrCode = errCode.ToString("X").PadLeft(8, '0');
                xelErrorCode.InnerText = "0x" + sErrCode;

                if (errCode > 0) {
                    XmlElement xelErrorText = NewElement(xelResult, "ErrorText", errText);
                }


            }

#if DEBUG
            string resultsFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");

            System.IO.File.WriteAllText(resultsFilePath, xelResults.OuterXml);

            System.Text.StringBuilder debugInfo = new System.Text.StringBuilder();
            debugInfo.Append("<html><body>");
            debugInfo.Append("<table>");
            debugInfo.Append("<tr><td>string listName</td><td>" + listName + "</td></tr>");
            debugInfo.Append("<tr><td>System.Xml.XmlNode updates</td><td>" + HttpUtility.HtmlEncode(updates.OuterXml) + "</td></tr>");
            debugInfo.Append("</table>");
            debugInfo.Append("<a href='" + resultsFilePath + "'>Results</a>");
            debugInfo.Append("</body></html>");
            System.IO.File.WriteAllText(System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".html"), debugInfo.ToString());
#endif
            return xelResults;



        }

        public System.Xml.XmlNode AddDiscussionBoardItem(string listName, byte[] message) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetVersionCollection(string strlistID, string strlistItemID, string strFieldName) {
            throw new NotImplementedException();
        }

        public string AddAttachment(string listName, string listItemID, string fileName, byte[] attachment) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetAttachmentCollection(string listName, string listItemID) {
            throw new NotImplementedException();
        }

        public void DeleteAttachment(string listName, string listItemID, string url) {
            throw new NotImplementedException();
        }

        public bool CheckOutFile(string pageUrl, string checkoutToLocal, string lastmodified) {
            throw new NotImplementedException();
        }

        public bool UndoCheckOut(string pageUrl) {
            throw new NotImplementedException();
        }

        public bool CheckInFile(string pageUrl, string comment, string CheckinType) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetListContentTypes(string listName, string contentTypeId) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode GetListContentType(string listName, string contentTypeId) {
            throw new NotImplementedException();
        }

        public string CreateContentType(string listName, string displayName, string parentType, System.Xml.XmlNode fields, System.Xml.XmlNode contentTypeProperties, string addToView) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode UpdateContentType(string listName, string contentTypeId, System.Xml.XmlNode contentTypeProperties, System.Xml.XmlNode newFields, System.Xml.XmlNode updateFields, System.Xml.XmlNode deleteFields, string addToView) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode DeleteContentType(string listName, string contentTypeId) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode UpdateContentTypeXmlDocument(string listName, string contentTypeId, System.Xml.XmlNode newDocument) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode UpdateContentTypesXmlDocument(string listName, System.Xml.XmlNode newDocument) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode DeleteContentTypeXmlDocument(string listName, string contentTypeId, string documentUri) {
            throw new NotImplementedException();
        }

        public System.Xml.XmlNode ApplyContentTypeToList(string webUrl, string contentTypeId, string listName) {
            throw new NotImplementedException();
        }

        #endregion
    }
}

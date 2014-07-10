using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Text;

namespace ProviderProxy
{
    public static class ListsHelper
    {
        public static StringList GetFieldNamesForListType(ListType listType)
        {
            StringList fields = new StringList();
            string[][] fieldInfo = GetFieldInfoForListType(listType);
            if (fieldInfo != null)
            {
                int indexOfName = LIST_FIELD_NAMES.ToList<string>().IndexOf("Name");
                int fieldCount = fieldInfo.GetUpperBound(0);
                for (int i = 0; i <= fieldCount; i++)
                {
                    string[] fieldrow = fieldInfo[i];
                    fields.Add(fieldrow[indexOfName]); //Name
                }
            }

            return fields;

        }

        public static string GetStsUrl(Guid id, string siteName, Uri baseUrl) {
            ProviderManager pm = new ProviderManager();
            // var providers = pm.GetAllIProviders();
            StringBuilder sb = new StringBuilder();
            IProvider provider = pm.GetIProvider(id);
            ListType listType = provider.GetProviderType(id);

            sb.Append("stssync://sts/?ver=1.1");
            sb.Append("&type=" + listType.ToString().ToLower());
            sb.Append("&cmd=add-folder");
            sb.Append("&base-url=" + StsEncode(baseUrl.AbsoluteUri));
            sb.Append("&list-url=" + StsEncode("/Lists/id" + id.ToString("N") + "/"));
            sb.Append("&guid=" + StsEncode(provider.ID.ToString("B")));
            sb.Append("&site-name=" + StsEncode(siteName));
            sb.Append("&list-name=" + StsEncode(provider.Name));
            return sb.ToString();
        }

        private static string StsEncode(string input) {
            string output = input;
            output = HttpUtility.UrlEncode(output);
            output = output.Replace("+", "%20");
            output = output.Replace("-", "%2D");
            return output;
        }

        public static XmlElement DataSetToDataNode(XmlElement parentElement, IEnumerable<object> wssDS, ListType listType, string requestThread)
        {
            return DataSetToDataNode(parentElement, wssDS, listType, Int32.MaxValue,0,requestThread );
        }

        public static XmlElement DataSetToDataNode(XmlElement parentElement, IEnumerable<object> wssDS,ListType listType,  int rowLimit,int startRow,string requestThread)
        {
            const int MAX_ROWS = 10000;
            
            XmlElement dataNode = parentElement.OwnerDocument.CreateElement("rs","data", "urn:schemas-microsoft-com:rowset");
            parentElement.AppendChild(dataNode);
            
            List<Field> fieldList = GetFieldsForListType(listType);

            
            int rowCount = Math.Min(MAX_ROWS, rowLimit); //ensure we don't go over hardcoded limit
            rowCount = Math.Min(wssDS.Count(), rowCount); //don't go over the number of rows in the set
            //rowCount = Math.Max((rowCount - startRow),0); //back out the rows we're skipping but ensure at least 0

            dataNode.SetAttribute("ItemCount", rowCount.ToString() );
            System.Diagnostics.Debug.Print("DataSetToDataNode: rowCount = " + rowCount.ToString());
            System.Diagnostics.Debug.Print("DataSetToDataNode: startRow = " + startRow.ToString());

            foreach (var item in wssDS.Skip(startRow).Take(rowCount).Select(x => x)) {
                XmlElement zRow = CreateZRow(dataNode, fieldList, null, item);
                dataNode.AppendChild(zRow);
            }

            if ((rowCount + startRow) < (wssDS.Count()))
            {
                dataNode.SetAttribute("ListItemCollectionPositionNext", requestThread + "&" + (rowCount + startRow).ToString());
            }
            return dataNode;
        }

        public static XmlElement CreateZRow(XmlElement dataNode, List<Field> fieldList, StringDictionary fieldMappings, object dr) {
            XmlElement zRow = dataNode.OwnerDocument.CreateElement("z", "row", "#RowsetSchema");

            foreach (Field fld in fieldList) {

                string attribName = fld.OWSName;
                string fieldName = fld.Name;

                if (fieldName == "MetaInfo")
                    continue;

                if (fieldMappings != null) {
                    if (fieldMappings.ContainsValue(fieldName)) {
                        foreach (string key in fieldMappings.Keys) {
                            if (fieldMappings[key] == fieldName) {
                                fieldName = key;
                                break;
                            }
                        }
                    }
                }
                if (dr is System.Dynamic.ExpandoObject) {
                    var drd = dr as IDictionary<string, object>;
                    if (drd.ContainsKey(fieldName) && drd[fieldName] != null) {
                        fld.Value = drd[fieldName];
                        zRow.SetAttribute(attribName, fld.GetValue().Trim());
                    } else {
                        zRow.SetAttribute(attribName, string.Empty);
                    }
                } else {
                    PropertyInfo pi = dr.GetType().GetProperty(fieldName);
                    if (pi != null) {
                        fld.Value = pi.GetValue(dr, null);
                        //dr[fieldName];
                        zRow.SetAttribute(attribName, fld.GetValue().Trim());
                    } else {
                        zRow.SetAttribute(attribName, string.Empty);
                    }
                }
            }
            return zRow;
        }

        private static string[][] MergeArrays(string[][] array1, string[][] array2)
        {
            List<string[]> listOfArrays = new List<string[]>();
            listOfArrays.AddRange(array1);
            listOfArrays.AddRange(array2);
            return (string[][])listOfArrays.ToArray<string[]>();
        }

        private static string[][] GetFieldInfoForListType(ListType listType)
        {

            switch (listType)
            {
                case ListType.Contacts:
                    return MergeArrays(COMMON_FIELDS,CONTACT_FIELDS);

                case ListType.Events:
                    return MergeArrays(COMMON_FIELDS,CALENDAR_FIELDS);

                case ListType.Tasks:
                    return MergeArrays(COMMON_FIELDS,TASK_FIELDS);
            }
            return null;
        }
        public static List<Field> GetFieldsForListType(ListType listType)
        {
            string[][] fieldInfo = GetFieldInfoForListType(listType);

            int countOfFields = fieldInfo.GetUpperBound(0) + 1;

            List<Field> fieldList = new List<Field>();

            for (int i = 0; i < countOfFields; i++)
            {
                Field field = new Field();
                string[] fieldrow = fieldInfo[i];
                for (int j = 0; j < LIST_FIELD_NAMES.Length; j++)
                {
                    field.SetAttribute(LIST_FIELD_NAMES[j], fieldrow[j]);
                }

                fieldList.Add(field);
            }

            return fieldList;
        }
        public static FieldDictionary GetFieldDictionaryForListType(ListType listType)
        {
            List<Field> fieldList = GetFieldsForListType(listType);
            FieldDictionary fieldDic = new FieldDictionary();
            foreach (Field fld in fieldList)
            {
                fieldDic.Add(fld.Name, fld);
            }
            return fieldDic;
        }

        internal static void FillListAttributes(string listName, System.Xml.XmlElement listElement, IProvider prov, string appPath)
        {
            listElement.SetAttribute("DocTemplateUrl", "");
            //listElement.SetAttribute("DefaultViewUrl", "/CustomOlProvider07/Lists/id5fd3bf7abe02474fadbbd32a0bb3c869/AllItems.aspx");
            listElement.SetAttribute("MobileDefaultViewUrl", "");
            listElement.SetAttribute("ID", listName);
            listElement.SetAttribute("Title", prov.Name);
            listElement.SetAttribute("Description", "");
            //listElement.SetAttribute("ImageUrl", "/_layouts/images/itcontct.gif");
            listElement.SetAttribute("Name", listName);
            listElement.SetAttribute("BaseType", "0");
            listElement.SetAttribute("FeatureId", "00bfea71-7e6d-4186-9ba8-c047ac750105");
            listElement.SetAttribute("Created", "20080114 07:43:40");
            listElement.SetAttribute("Modified", "20080114 07:43:40");
            listElement.SetAttribute("LastDeleted", "20080114 07:43:40");
            listElement.SetAttribute("Version", "0");
            listElement.SetAttribute("Direction", "none");
            listElement.SetAttribute("ThumbnailSize", "");
            listElement.SetAttribute("WebImageWidth", "");
            listElement.SetAttribute("WebImageHeight", "");
            listElement.SetAttribute("Flags", "536875008");
            listElement.SetAttribute("ItemCount", "0");
            listElement.SetAttribute("AnonymousPermMask", "0");
            listElement.SetAttribute("RootFolder", appPath.TrimEnd('/') + "/Lists/id" + prov.ID.ToString("N"));
            listElement.SetAttribute("ReadSecurity", "1");
            listElement.SetAttribute("WriteSecurity", "1");
            listElement.SetAttribute("Author", "1");
            listElement.SetAttribute("EventSinkAssembly", "");
            listElement.SetAttribute("EventSinkClass", "");
            listElement.SetAttribute("EventSinkData", "");
            listElement.SetAttribute("EmailInsertsFolder", "");
            listElement.SetAttribute("EmailAlias", "");
            listElement.SetAttribute("WebFullUrl", appPath);
            listElement.SetAttribute("WebId", WebService1.WEB_ID.ToString("D"));
            listElement.SetAttribute("SendToLocation", "");
            listElement.SetAttribute("ScopeId", "2c64513b-085a-4dec-8811-4f82e8df465f");
            listElement.SetAttribute("MajorVersionLimit", "0");
            listElement.SetAttribute("MajorWithMinorVersionsLimit", "0");
            listElement.SetAttribute("WorkFlowId", "");
            listElement.SetAttribute("HasUniqueScopes", "False");
            listElement.SetAttribute("AllowDeletion", "True");
            listElement.SetAttribute("AllowMultiResponses", "False");
            listElement.SetAttribute("EnableAttachments", "True");
            listElement.SetAttribute("EnableModeration", "False");
            listElement.SetAttribute("EnableVersioning", "False");
            listElement.SetAttribute("Hidden", "False");
            listElement.SetAttribute("MultipleDataList", "False");
            listElement.SetAttribute("Ordered", "False");
            listElement.SetAttribute("ShowUser", "True");
            listElement.SetAttribute("EnableMinorVersion", "False");
            listElement.SetAttribute("RequireCheckout", "False");

            switch (prov.GetProviderType(prov.ID))
            {
                case ListType.Contacts:
                    listElement.SetAttribute("ServerTemplate", "105");
                    break;
                case ListType.Events:
                    listElement.SetAttribute("ServerTemplate", "106");
                    break;
                case ListType.Tasks:
                    listElement.SetAttribute("ServerTemplate", "107");
                    break;
            }


        }

        //TODO: Change this to use the <Field> elements instead. I think it will be faster and also more readable.

        /*
    {ID,Name,StaticName,DisplayName,Type,ReadOnly}
         */
        static readonly string[] LIST_FIELD_NAMES = { "ID", "Name", "StaticName", "DisplayName", "Type", "ReadOnly" };
        static readonly string[][] COMMON_FIELDS = new string[][]{
            new string[] {"{1d22ea11-1e32-424e-89ab-9fedbadb6ce1}", "ID", "ID", "ID", "Counter", "TRUE"}, 
            new string[] {"{03e45e84-1992-4d42-9116-26f756012634}", "ContentTypeId", "ContentTypeId", "Content Type ID", "ContentTypeId", "TRUE"}, 
            new string[] {"{c042a256-787d-4a6f-8a8a-cf6ab767f12d}", "ContentType", "ContentType", "Content Type", "Text", "TRUE"}, 
            new string[] {"{28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f}", "Modified", "Modified", "Modified", "DateTime", "TRUE"}, 
            new string[] {"{8c06beca-0777-48f7-91c7-6da68bc07b69}", "Created", "Created", "Created", "DateTime", "TRUE"}, 
            new string[] {"{1df5e554-ec7e-46a6-901d-d85a3881cb18}", "Author", "Author", "Created By", "User", "TRUE"}, 
            new string[] {"{d31655d1-1d5b-4511-95a1-7a09e9b75bf2}", "Editor", "Editor", "Modified By", "User", "TRUE"}, 
            new string[] {"{26d0756c-986a-48a7-af35-bf18ab85ff4a}", "_HasCopyDestinations", "_HasCopyDestinations", "Has Copy Destinations", "Boolean", "TRUE"}, 
            new string[] {"{6b4e226d-3d88-4a36-808d-a129bf52bccf}", "_CopySource", "_CopySource", "Copy Source", "Text", "TRUE"}, 
            new string[] {"{d4e44a66-ee3a-4d02-88c9-4ec5ff3f4cd5}", "owshiddenversion", "owshiddenversion", "owshiddenversion", "Integer", "TRUE"}, 
            new string[] {"{f1e020bc-ba26-443f-bf2f-b68715017bbc}", "WorkflowVersion", "WorkflowVersion", "Workflow Version", "Integer", "TRUE"}, 
            new string[] {"{7841bf41-43d0-4434-9f50-a673baef7631}", "_UIVersion", "_UIVersion", "UI Version", "Integer", "TRUE"}, 
            new string[] {"{dce8262a-3ae9-45aa-aab4-83bd75fb738a}", "_UIVersionString", "_UIVersionString", "Version", "Text", "TRUE"}, 
            new string[] {"{67df98f4-9dec-48ff-a553-29bece9c5bf4}", "Attachments", "Attachments", "Attachments", "Attachments", null}, 
            new string[] {"{fdc3b2ed-5bf2-4835-a4bc-b885f3396a61}", "_ModerationStatus", "_ModerationStatus", "Approval Status", "ModStat", "TRUE"}, 
            new string[] {"{34ad21eb-75bd-4544-8c73-0e08330291fe}", "_ModerationComments", "_ModerationComments", "Approver Comments", "Note", "TRUE"}, 
            new string[] {"{503f1caa-358e-4918-9094-4a2cdc4bc034}", "Edit", "Edit", "Edit", "Computed", "TRUE"}, 
            new string[] {"{b1f7969b-ea65-42e1-8b54-b588292635f2}", "SelectTitle", "SelectTitle", "Select", "Computed", "TRUE"}, 
            new string[] {"{50a54da4-1528-4e67-954a-e2d24f1e9efb}", "InstanceID", "InstanceID", "Instance ID", "Integer", "TRUE"}, 
            new string[] {"{ca4addac-796f-4b23-b093-d2a3f65c0774}", "Order", "Order", "Order", "Number", null}, 
            new string[] {"{ae069f25-3ac2-4256-b9c3-15dbc15da0e0}", "GUID", "GUID", "GUID", "Guid", "TRUE"}, 
            new string[] {"{de8beacf-5505-47cd-80a6-aa44e7ffe2f4}", "WorkflowInstanceID", "WorkflowInstanceID", "Workflow Instance ID", "Guid", "TRUE"}, 
            new string[] {"{94f89715-e097-4e8b-ba79-ea02aa8b7adb}", "FileRef", "FileRef", "URL Path", "Lookup", "TRUE"}, 
            new string[] {"{56605df6-8fa1-47e4-a04c-5b384d59609f}", "FileDirRef", "FileDirRef", "Path", "Lookup", "TRUE"}, 
            new string[] {"{173f76c8-aebd-446a-9bc9-769a2bd2c18f}", "Last_x0020_Modified", "Last_x0020_Modified", "Modified", "Lookup", "TRUE"}, 
            new string[] {"{998b5cff-4a35-47a7-92f3-3914aa6aa4a2}", "Created_x0020_Date", "Created_x0020_Date", "Created", "Lookup", "TRUE"}, 
            new string[] {"{30bb605f-5bae-48fe-b4e3-1f81d9772af9}", "FSObjType", "FSObjType", "Item Type", "Lookup", "TRUE"}, 
            new string[] {"{ba3c27ee-4791-4867-8821-ff99000bac98}", "PermMask", "PermMask", "Effective Permissions Mask", "Computed", "TRUE"}, 
            new string[] {"{8553196d-ec8d-4564-9861-3dbe931050c8}", "FileLeafRef", "FileLeafRef", "Name", "File", null}, 
            new string[] {"{4b7403de-8d94-43e8-9f0f-137a3e298126}", "UniqueId", "UniqueId", "Unique Id", "Lookup", "TRUE"}, 
            new string[] {"{c5c4b81c-f1d9-4b43-a6a2-090df32ebb68}", "ProgId", "ProgId", "ProgId", "Lookup", "TRUE"}, 
            new string[] {"{dddd2420-b270-4735-93b5-92b713d0944d}", "ScopeId", "ScopeId", "ScopeId", "Lookup", "TRUE"}, 
            new string[] {"{39360f11-34cf-4356-9945-25c44e68dade}", "File_x0020_Type", "File_x0020_Type", "File Type", "Text", "TRUE"}, 
            new string[] {"{4ef1b78f-fdba-48dc-b8ab-3fa06a0c9804}", "HTML_x0020_File_x0020_Type", "HTML_x0020_File_x0020_Type", "HTML File Type", "Computed", "TRUE"}, 
            new string[] {"{3c6303be-e21f-4366-80d7-d6d0a3b22c7a}", "_EditMenuTableStart", "_EditMenuTableStart", "Edit Menu Table Start", "Computed", "TRUE"}, 
            new string[] {"{2ea78cef-1bf9-4019-960a-02c41636cb47}", "_EditMenuTableEnd", "_EditMenuTableEnd", "Edit Menu Table End", "Computed", "TRUE"}, 
            new string[] {"{9d30f126-ba48-446b-b8f9-83745f322ebe}", "LinkFilenameNoMenu", "LinkFilenameNoMenu", "Name", "Computed", "TRUE"}, 
            new string[] {"{5cc6dc79-3710-4374-b433-61cb4a686c12}", "LinkFilename", "LinkFilename", "Name", "Computed", "TRUE"}, 
            new string[] {"{081c6e4c-5c14-4f20-b23e-1a71ceb6a67c}", "DocIcon", "DocIcon", "Type", "Computed", "TRUE"}, 
            new string[] {"{105f76ce-724a-4bba-aece-f81f2fce58f5}", "ServerUrl", "ServerUrl", "Server Relative URL", "Computed", "TRUE"}, 
            new string[] {"{7177cfc7-f399-4d4d-905d-37dd51bc90bf}", "EncodedAbsUrl", "EncodedAbsUrl", "Encoded Absolute URL", "Computed", "TRUE"}, 
            new string[] {"{7615464b-559e-4302-b8e2-8f440b913101}", "BaseName", "BaseName", "File Name", "Computed", "TRUE"}, 
            new string[] {"{687c7f94-686a-42d3-9b67-2782eac4b4f8}", "MetaInfo", "MetaInfo", "Property Bag", "Lookup", null}, 
            new string[] {"{43bdd51b-3c5b-4e78-90a8-fb2087f71e70}", "_Level", "_Level", "Level", "Integer", "TRUE"}, 
            new string[] {"{c101c3e7-122d-4d4d-bc34-58e94a38c816}", "_IsCurrentVersion", "_IsCurrentVersion", "Is Current Version", "Boolean", "TRUE"}, 

        };
        static readonly string[][] CONTACT_FIELDS = new string[][]{
            new string[] {"{fa564e0f-0c70-4ab9-b863-0177e6ddd247}", "Title", "Title", "Last Name", "Text", null}, 
            new string[] {"{bc91a437-52e7-49e1-8c4e-4698904b2b6d}", "LinkTitleNoMenu", "LinkTitleNoMenu", "Last Name", "Computed", "TRUE"}, 
            new string[] {"{82642ec8-ef9b-478f-acf9-31f7d45fbc31}", "LinkTitle", "LinkTitle", "Last Name", "Computed", "TRUE"}, 
            new string[] {"{fdc8216d-dabf-441d-8ac0-f6c626fbdc24}", "LastNamePhonetic", "LastNamePhonetic", "Last Name Phonetic", "Text", null}, 
            new string[] {"{4a722dd4-d406-4356-93f9-2550b8f50dd0}", "FirstName", "FirstName", "First Name", "Text", null}, 
            new string[] {"{ea8f7ca9-2a0e-4a89-b8bf-c51a6af62c73}", "FirstNamePhonetic", "FirstNamePhonetic", "First Name Phonetic", "Text", null}, 
            new string[] {"{475c2610-c157-4b91-9e2d-6855031b3538}", "FullName", "FullName", "Full Name", "Text", null}, 
            new string[] {"{fce16b4c-fe53-4793-aaab-b4892e736d15}", "Email", "Email", "E-mail Address", "Text", null}, 
            new string[] {"{038d1503-4629-40f6-adaf-b47d1ab2d4fe}", "Company", "Company", "Company", "Text", null}, 
            new string[] {"{034aae88-6e9a-4e41-bc8a-09b6c15fcdf4}", "CompanyPhonetic", "CompanyPhonetic", "Company Phonetic", "Text", null}, 
            new string[] {"{c4e0f350-52cc-4ede-904c-dd71a3d11f7d}", "JobTitle", "JobTitle", "Job Title", "Text", null}, 
            new string[] {"{fd630629-c165-4513-b43c-fdb16b86a14d}", "WorkPhone", "WorkPhone", "Business Phone", "Text", null}, 
            new string[] {"{2ab923eb-9880-4b47-9965-ebf93ae15487}", "HomePhone", "HomePhone", "Home Phone", "Text", null}, 
            new string[] {"{2a464df1-44c1-4851-949d-fcd270f0ccf2}", "CellPhone", "CellPhone", "Mobile Phone", "Text", null}, 
            new string[] {"{9d1cacc8-f452-4bc1-a751-050595ad96e1}", "WorkFax", "WorkFax", "Fax Number", "Text", null}, 
            new string[] {"{fc2e188e-ba91-48c9-9dd3-16431afddd50}", "WorkAddress", "WorkAddress", "Address", "Note", null}, 
            new string[] {"{6ca7bd7f-b490-402e-af1b-2813cf087b1e}", "WorkCity", "WorkCity", "City", "Text", null}, 
            new string[] {"{ceac61d3-dda9-468b-b276-f4a6bb93f14f}", "WorkState", "WorkState", "State/Province", "Text", null}, 
            new string[] {"{9a631556-3dac-49db-8d2f-fb033b0fdc24}", "WorkZip", "WorkZip", "ZIP/Postal Code", "Text", null}, 
            new string[] {"{3f3a5c85-9d5a-4663-b925-8b68a678ea3a}", "WorkCountry", "WorkCountry", "Country/Region", "Text", null}, 
            new string[] {"{a71affd2-dcc7-4529-81bc-2fe593154a5f}", "WebPage", "WebPage", "Web Page", "URL", null}, 
            new string[] {"{9da97a8a-1da5-4a77-98d3-4bc10456e700}", "Comments", "Comments", "Notes", "Note", null}       };
        static readonly string[][] CALENDAR_FIELDS = new string[][]{
            new string[] {"{fa564e0f-0c70-4ab9-b863-0177e6ddd247}", "Title", "Title", "Title", "Text", ""}, 
            new string[] {"{bc91a437-52e7-49e1-8c4e-4698904b2b6d}", "LinkTitleNoMenu", "LinkTitleNoMenu", "Title", "Computed", "TRUE"}, 
            new string[] {"{82642ec8-ef9b-478f-acf9-31f7d45fbc31}", "LinkTitle", "LinkTitle", "Title", "Computed", "TRUE"}, 
            new string[] {"{288f5f32-8462-4175-8f09-dd7ba29359a9}", "Location", "Location", "Location", "Text", ""}, 
            new string[] {"{64cd368d-2f95-4bfc-a1f9-8d4324ecb007}", "EventDate", "EventDate", "Start Time", "DateTime", ""}, 
            new string[] {"{2684f9f2-54be-429f-ba06-76754fc056bf}", "EndDate", "EndDate", "End Time", "DateTime", ""}, 
            new string[] {"{9da97a8a-1da5-4a77-98d3-4bc10456e700}", "Description", "Description", "Description", "Note", ""}, 
            new string[] {"{7d95d1f4-f5fd-4a70-90cd-b35abc9b5bc8}", "fAllDayEvent", "fAllDayEvent", "All Day Event", "AllDayEvent", ""}, 
            new string[] {"{f2e63656-135e-4f1c-8fc2-ccbe74071901}", "fRecurrence", "fRecurrence", "Recurrence", "Recurrence", ""}, 
            new string[] {"{08fc65f9-48eb-4e99-bd61-5946c439e691}", "WorkspaceLink", "WorkspaceLink", "Workspace", "CrossProjectLink", ""}, 
            new string[] {"{5d1d4e76-091a-4e03-ae83-6a59847731c0}", "EventType", "EventType", "Event Type", "Integer", ""}, 
            new string[] {"{63055d04-01b5-48f3-9e1e-e564e7c6b23b}", "UID", "UID", "UID", "Guid", ""}, 
            new string[] {"{dfcc8fff-7c4c-45d6-94ed-14ce0719efef}", "RecurrenceID", "RecurrenceID", "Recurrence ID", "DateTime", ""}, 
            new string[] {"{b8bbe503-bb22-4237-8d9e-0587756a2176}", "EventCanceled", "EventCanceled", "Event Canceled", "Boolean", ""}, 
            new string[] {"{4d54445d-1c84-4a6d-b8db-a51ded4e1acc}", "Duration", "Duration", "Duration", "Integer", ""}, 
            new string[] {"{d12572d0-0a1e-4438-89b5-4d0430be7603}", "RecurrenceData", "RecurrenceData", "RecurrenceData", "Note", ""}, 
            new string[] {"{6cc1c612-748a-48d8-88f2-944f477f301b}", "TimeZone", "TimeZone", "TimeZone", "Integer", ""}, 
            new string[] {"{c4b72ed6-45aa-4422-bff1-2b6750d30819}", "XMLTZone", "XMLTZone", "XMLTZone", "Note", ""}, 
            new string[] {"{9b2bed84-7769-40e3-9b1d-7954a4053834}", "MasterSeriesItemID", "MasterSeriesItemID", "MasterSeriesItemID", "Integer", ""}, 
            new string[] {"{881eac4a-55a5-48b6-a28e-8329d7486120}", "Workspace", "Workspace", "WorkspaceUrl", "URL", ""}};
        static readonly string[][] TASK_FIELDS = new string[][]{
            new string[] {"{fa564e0f-0c70-4ab9-b863-0177e6ddd247}", "Title", "Title", "Title", "Text", ""}, 
            new string[] {"{bc91a437-52e7-49e1-8c4e-4698904b2b6d}", "LinkTitleNoMenu", "LinkTitleNoMenu", "Title", "Computed", "TRUE"}, 
            new string[] {"{82642ec8-ef9b-478f-acf9-31f7d45fbc31}", "LinkTitle", "LinkTitle", "Title", "Computed", "TRUE"}, 
            new string[] {"{a8eb573e-9e11-481a-a8c9-1104a54b2fbd}", "Priority", "Priority", "Priority", "Choice", ""}, 
            new string[] {"{c15b34c3-ce7d-490a-b133-3f4de8801b76}", "Status", "Status", "Status", "Choice", ""}, 
            new string[] {"{d2311440-1ed6-46ea-b46d-daa643dc3886}", "PercentComplete", "PercentComplete", "% Complete", "Number", ""}, 
            new string[] {"{53101f38-dd2e-458c-b245-0c236cc13d1a}", "AssignedTo", "AssignedTo", "Assigned To", "User", ""}, 
            new string[] {"{50d8f08c-8e99-4948-97bf-2be41fa34a0d}", "TaskGroup", "TaskGroup", "Task Group", "User", ""}, 
            new string[] {"{7662cd2c-f069-4dba-9e35-082cf976e170}", "Body", "Body", "Description", "Note", ""}, 
            new string[] {"{64cd368d-2f95-4bfc-a1f9-8d4324ecb007}", "StartDate", "StartDate", "Start Date", "DateTime", ""}, 
            new string[] {"{cd21b4c2-6841-4f9e-a23a-738a65f99889}", "DueDate", "DueDate", "Due Date", "DateTime", ""}};
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProviderProxy;
using System.Data;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

namespace AdventureWorksProvider {
    public class Contacts : IProvider {

        #region IProvider Members

        public Guid ID {
            get {
                Debug.WriteLine(HttpContext.Current.Request.Path);
                return new Guid("7765B84F-6D32-4d31-B28E-6BC615D2F187");
            }
        }
        public string Name {
            get {
                return "AdventureWorks Contacts";
            }
        }

        public Type GetEmptyDataSet(Guid ProviderID) {
            /*ContactsDS ds = new ContactsDS();
            ds.Contact.TableName = GetProviderType(ProviderID).ToString();
            return ds;*/
            return Type.GetType("TMP.ContactObject");
        }



        public void Delete(Guid ProviderID, object deleteRow) {
            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();

            DataRow dr = ((TMP.ContactObject)deleteRow).ToDataRow(cta);
            dr.Delete();
            cta.Adapter.Update(new DataRow[] { dr });
            return;
        }

        public object Update(Guid ProviderID, object updateRow) {
            var ur = (TMP.ContactObject)updateRow;
            //ListType listType = GetProviderType(ProviderID);
            //string tableName = listType.ToString();

            //DateTime updateTime = DateTime.Now;
            //updateRow["ModifiedDate"] = updateTime;
            ur.ModifiedDate = DateTime.Now;


            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();
            cta.Adapter.Update(new DataRow[] { ur.ToDataRow(cta) });


            return ur;

        }

        public IEnumerable<object> GetUpdatesSinceToken(Guid ProviderID, ChangeKey changeKey) {
            DataSet retDS = new DataSet();
            ListType listType = GetProviderType(ProviderID);
            string tableName = listType.ToString();

            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();
            ContactsDS.ContactDataTable cdt = null;
            if (changeKey != null) {
                DateTime changeTime = changeKey.ChangeTime;
                changeTime = changeTime.AddMilliseconds(-1 * changeTime.Millisecond).AddSeconds(-1);
                cdt = cta.GetDataByModifiedDate(changeTime.AddSeconds(-1));
            } else {
                cdt = cta.GetData();
            }

            cdt.TableName = tableName;

            retDS.Tables.Add(cdt);

            var ret = new List<TMP.ContactObject>();
            foreach (DataRow item in retDS.Tables[0].Rows) {
                ret.Add(TMP.ContactObject.GetFrom(item));
            }

            return ret;
        }

        public ListType GetProviderType(Guid ProviderID) {
            return ListType.Contacts;
        }

        public StringDictionary GetFieldMappingsForListType(Guid ProviderID, ListType listType) {
            StringDictionary fieldNames = new StringDictionary();

            fieldNames.Add("ContactID", "ID");
            fieldNames.Add("Title", "JobTitle");
            fieldNames.Add("LastName", "Title");
            fieldNames.Add("EmailAddress", "Email");
            fieldNames.Add("Phone", "WorkPhone");
            fieldNames.Add("rowguid", "GUID");
            fieldNames.Add("ModifiedTime", "Modified");
            fieldNames.Add("AdditionalData", "MetaInfo");

            return fieldNames;
        }

        public object GetSingleRow(Guid ProviderID, int id) {
            ContactsDS contactsDS = new ContactsDS();

            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();

            cta.FillByContactID(contactsDS.Contact, id);
            if (contactsDS.Contact.Rows.Count > 0) {
                TMP.ContactObject co = new TMP.ContactObject();
                co.SetPropertiesFrom(contactsDS.Contact.Rows[0]);
                return co;
            }



            return null;


        }

        #endregion
    }
}

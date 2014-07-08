using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProviderProxy;
using System.Data;
using System.Collections.Specialized;

namespace AdventureWorksProvider
{
    public class Contacts:IProvider
    {

        #region IProvider Members

        public DataSet GetEmptyDataSet(Guid ProviderID)
        {
            ContactsDS ds = new ContactsDS();
            ds.Contact.TableName = GetProviderType(ProviderID).ToString();
            return ds;
        }

        public DataRow Update(Guid ProviderID, DataRow updateRow)
        {
            //ListType listType = GetProviderType(ProviderID);
            //string tableName = listType.ToString();

            DateTime updateTime = DateTime.Now;
            updateRow["ModifiedDate"] = updateTime;


            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();
            cta.Adapter.Update(new DataRow[]{updateRow});


            return updateRow;

        }

        public DataSet GetUpdatesSinceToken(Guid ProviderID, ChangeKey changeKey)
        {
            DataSet retDS = new DataSet();
            ListType listType = GetProviderType(ProviderID);
            string tableName = listType.ToString();

            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();
            ContactsDS.ContactDataTable cdt = null;
            if (changeKey != null)
            {
                DateTime changeTime = changeKey.ChangeTime;
                changeTime = changeTime.AddMilliseconds(-1 * changeTime.Millisecond).AddSeconds(-1);
                cdt = cta.GetDataByModifiedDate(changeTime.AddSeconds(-1));
            }
            else
            {
                cdt = cta.GetData();
            }

            cdt.TableName = tableName;

            retDS.Tables.Add(cdt);

            return retDS;
        }

        public ListType GetProviderType(Guid ProviderID)
        {
            return ListType.Contacts;
        }

        public StringDictionary GetFieldMappingsForListType(Guid ProviderID, ListType listType)
        {
            StringDictionary fieldNames = new StringDictionary();

            fieldNames.Add("ContactID","ID");
            fieldNames.Add("Title", "JobTitle");
            fieldNames.Add("LastName", "Title");
            fieldNames.Add("EmailAddress", "Email");
            fieldNames.Add("Phone", "WorkPhone");
            fieldNames.Add("rowguid", "GUID");
            fieldNames.Add("ModifiedTime", "Modified");
            fieldNames.Add("AdditionalData", "MetaInfo");

            return fieldNames;
        }

        public DataRow GetSingleRow(Guid ProviderID, int id)
        {
            ContactsDS contactsDS = new ContactsDS();

            ContactsDSTableAdapters.ContactTableAdapter cta = new AdventureWorksProvider.ContactsDSTableAdapters.ContactTableAdapter();
            
            cta.FillByContactID(contactsDS.Contact, id);
            if (contactsDS.Contact.Rows.Count > 0)
            {
                return contactsDS.Contact.Rows[0];
            }

            return null;

            
        }

        #endregion
    }
}

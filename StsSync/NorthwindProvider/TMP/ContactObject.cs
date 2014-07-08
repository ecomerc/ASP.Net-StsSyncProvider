using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorksProvider.TMP {
    public class ContactObject : DataTableBackend {


        public int ContactID { get; set; }

        public bool NameStyle { get; set; }


        public string Title { get; set; }


        public string FirstName { get; set; }


        public string MiddleName { get; set; }


        public string LastName { get; set; }


        public string Suffix { get; set; }


        public string EmailAddress { get; set; }


        public int EmailPromotion { get; set; }


        public string Phone { get; set; }


        public string PasswordHash { get; set; }


        public string PasswordSalt { get; set; }


        public string AdditionalContactInfo { get; set; }


        public System.Guid rowguid { get; set; }


        public System.DateTime ModifiedDate { get; set; }

        public string AdditionalData { get; set; }


    }



    /*
     * This is polyfill to make the sample work with DataTables and DataRows (do use the entity framework instead, much easier)
     */
    public abstract class DataTableBackend {
        public void SetPropertiesFrom(DataRow row) {
            // enumerate the public properties of the object:
            foreach (PropertyInfo property in this.GetType().GetProperties()) {
                // does the property name appear as a column in the table?
                if (row.Table.Columns.Contains(property.Name)) {
                    // get the data-column:
                    DataColumn column = row.Table.Columns[property.Name];

                    // get the value of the column from the row:
                    object value = row[column];

                    // set the value on the property:
                    if (!(value is DBNull))
                        property.SetValue(this, Convert.ChangeType(value, property.PropertyType), null);

                }
            }
        }

        public static TMP.ContactObject GetFrom(DataRow row) {
            var co = new TMP.ContactObject();
            co.SetPropertiesFrom(row);
            return co;
        }

        public DataRow ToDataRow(ContactsDSTableAdapters.ContactTableAdapter cta) {
            PropertyInfo[] properties = this.GetType().GetProperties();
            var ds = new DataSet();
            cta.Adapter.FillSchema(ds, SchemaType.Mapped);
            DataRow dr = ds.Tables[0].NewRow();

            foreach (PropertyInfo pi in properties) {
                dr[pi.Name] = pi.GetValue(this, null);
            }
            return dr;
        }
    }

}

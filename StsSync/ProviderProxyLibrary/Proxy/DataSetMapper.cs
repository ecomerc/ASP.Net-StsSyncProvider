using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Xml.Linq;
using System.Reflection;
using System.Dynamic;

namespace ProviderProxy
{
    public static class DataSetMapper
    {
        public static IEnumerable<object> MapCustomDSToWSSDS(IEnumerable<Object> customDS,
            string tableName,
            StringDictionary fieldNameMappings,
            ListType listType)
        {
            List<dynamic> ret = new List<dynamic>();

            StringList wssFields = ListsHelper.GetFieldNamesForListType(listType);
            string sListType = listType.ToString();
            if (customDS.Count() > 0) {
                PropertyInfo[] properties = customDS.ElementAt(0).GetType().GetProperties();

                foreach (var item in customDS) {
                    var newObj = new ExpandoObject() as IDictionary<string, Object>;
                    foreach (PropertyInfo pi in properties) {
                        var columnName = pi.Name;
                        if (fieldNameMappings.ContainsKey(columnName) &&
                            wssFields.Contains(fieldNameMappings[columnName])) {
                            columnName = fieldNameMappings[columnName];
                        }
                        newObj.Add(columnName,pi.GetValue(item, null));
                    }
                    ret.Add(newObj);

                }





                return ret;
            } else {
                return customDS;
            }
        }

        public static string GetCustomFieldName(string sWSSName, StringDictionary fieldMappings)
        {
            foreach (string iCustomName in fieldMappings.Keys) 
                if (fieldMappings[iCustomName] == sWSSName) return iCustomName;

            return sWSSName;
        }

        public static Type GetTypeFromSqlType2
            (SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt: return typeof(Int64);  
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                case SqlDbType.Binary: return typeof(byte[]); 
                case SqlDbType.Bit: return typeof(bool); 
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Time:
                case SqlDbType.DateTimeOffset: return typeof(DateTime); 
                case SqlDbType.SmallMoney:
                case SqlDbType.Money:
                case SqlDbType.Decimal:return typeof(decimal); 
                case SqlDbType.Float: return typeof(float); 
                case SqlDbType.Int: return typeof(int); 
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                    return typeof(string); 
                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid); 
                case SqlDbType.TinyInt:
                    return typeof(byte); 
                case SqlDbType.Real:
                    return typeof(Single); 
                case SqlDbType.SmallInt:
                    return typeof(Int16); 
                default:
                    return typeof(object); 
            }
        }

        public static object GetTypedValue(Type dataType,string inputValue)
        {
            if (inputValue == null) return null;

            object objValue = null;
            if (dataType == typeof(string))
            {
                objValue = inputValue;
            }
            else if (dataType == typeof(int))
            {
                objValue = int.Parse(inputValue);
            }
            else if (dataType == typeof(DateTime))
            {
                objValue = DateTime.Parse(inputValue);
            }
            else if (dataType == typeof(decimal))
            {
                objValue = decimal.Parse(inputValue);
            }
            else if (dataType == typeof(bool))
            {
                objValue = bool.Parse(inputValue);
            }
            else if (dataType == typeof(double))
            {
                objValue = double.Parse(inputValue);
            }
            else if (dataType == typeof(Guid))
            {
                objValue = new Guid(inputValue);
            }
            else if (dataType == typeof(byte))
            {
                objValue = byte.Parse(inputValue);
            }
            else if (dataType == typeof(byte[]))
            {
                objValue = System.Text.Encoding.Unicode.GetBytes(inputValue);
            }
            return objValue;



            
        }
    }
   
    public class StringList : List<string> { };
}

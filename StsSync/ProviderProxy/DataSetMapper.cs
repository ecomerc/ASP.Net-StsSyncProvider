using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Xml.Linq;

namespace ProviderProxy
{
    public static class DataSetMapper
    {
        public static DataSet MapCustomDSToWSSDS(DataSet customDS,
            string tableName,
            StringDictionary fieldNameMappings,
            ListType listType)
        {
            DataSet dsGeneric = new DataSet();
            string sListType = listType.ToString();
            dsGeneric.Tables.Add(customDS.Tables[tableName].Copy());
            DataTable dtGeneric = dsGeneric.Tables[0];
            dtGeneric.TableName = listType.ToString();

            StringList wssFields = ListsHelper.GetFieldNamesForListType(listType);

            DataColumnCollection customDTColumns = dtGeneric.Columns;
            int colCount = customDTColumns.Count;

            //reorder columns if necessary.
            foreach (string wssColName in fieldNameMappings.Values)
            {
                if (customDTColumns.Contains(wssColName))
                {
                    //set it to the last position. last position is 0 because we loop through backwards below.
                    customDTColumns[wssColName].SetOrdinal(colCount-1);
                }
            }

            for(int i = colCount-1;i>=0;i--)
            {
                DataColumn dc = customDTColumns[i];
                //If a column has a mapping, ensure it's a valid name value
                if(fieldNameMappings.ContainsKey(dc.ColumnName) &&
                    wssFields.Contains(fieldNameMappings[dc.ColumnName]))
                {
                    dc.ColumnName = fieldNameMappings[dc.ColumnName];
                }

                //if the column name ends up being something we don't recognize, delete it
                if (!wssFields.Contains(dc.ColumnName))
                {
                    dtGeneric.Columns.Remove(dc.ColumnName);
                }
             }

            return dsGeneric;
        }

        public static string GetCustomFieldName(string sWSSName, StringDictionary fieldMappings)
        {
            foreach (string iCustomName in fieldMappings.Keys) 
                if (fieldMappings[iCustomName] == sWSSName) return iCustomName;

            return sWSSName;
        }

        public static Type GetTypeFromSqlType(SqlDbType sqlType)
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

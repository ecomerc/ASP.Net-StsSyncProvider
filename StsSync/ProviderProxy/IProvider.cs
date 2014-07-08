using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Specialized;

namespace ProviderProxy
{
    public interface IProvider
    {
        DataSet GetEmptyDataSet(Guid ProviderID);
        DataRow GetSingleRow(Guid ProviderID, int id);
        DataRow Update(Guid ProviderID, DataRow updateRow);
        DataSet GetUpdatesSinceToken(Guid ProviderID, ChangeKey changeKey);
        ListType GetProviderType(Guid ProviderID);
        StringDictionary GetFieldMappingsForListType(Guid ProviderID, ListType listType);
    }
}

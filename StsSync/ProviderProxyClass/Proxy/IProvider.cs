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
        Guid ID {get; }
        string Name { get;  }

        ListType GetProviderType(Guid ProviderID);
        StringDictionary GetFieldMappingsForListType(Guid ProviderID, ListType listType);
        Type GetEmptyDataSet(Guid ProviderID);
        object GetSingleRow(Guid ProviderID, int id);
        object Update(Guid ProviderID, object updateRow);
        void Delete(Guid ProviderID, object deleteRow);
        IEnumerable<object> GetUpdatesSinceToken(Guid ProviderID, ChangeKey changeKey);
    }

}

using System;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ProviderProxy {
    public class ProviderManager  {

        private static Dictionary<Guid, IProvider> _providers = new Dictionary<Guid, IProvider>();

        public static void RegisterProvider(IProvider provider) {
            _providers.Add(provider.ID, provider);

            Debug.WriteLine(ListsHelper.GetFieldsForListType(ListType.Calendar));

            foreach (var item in ListsHelper.GetFieldsForListType(ListType.Calendar)) {
                Debug.WriteLine(item.Name + " : " + item.OWSName + " : " + item.Type + " : " + item.ReadOnly);

            }
        }

        internal IProvider GetIProvider(Guid id) {
            //Dictionary<Guid, Provider> providers = (Dictionary<Guid, Provider>)ConfigurationSettings.GetConfig("ProviderProxy/Providers");
            if (_providers.ContainsKey(id))
                return (IProvider)_providers[id];

            return null;
        }

        /*internal Provider GetProvider(Guid id) {
            Dictionary<Guid, Provider> providers = (Dictionary<Guid, Provider>)ConfigurationSettings.GetConfig("ProviderProxy/Providers");
            if (providers.ContainsKey(id))
                return providers[id];

            return null;
        }
         */

        public IEnumerable<IProvider> GetAllIProviders() {
            return _providers.Values;
        }

    }

    /*
    public class Provider : System.Collections.Specialized.StringDictionary {
        const string ATTRIB_TYPE = "type";
        const string ATTRIB_ID = "id";
        const string ATTRIB_ASSEMBLY = "assembly";
        const string ATTRIB_NAME = "name";

        static Hashtable _providerObjects = new Hashtable();

        public object GetIProvider() {
            string hashName = this[ATTRIB_ID];
            if (_providerObjects.ContainsKey(hashName))
                return _providerObjects[hashName];


            if (this.ContainsKey(ATTRIB_ASSEMBLY) &&
                this.ContainsKey(ATTRIB_TYPE)) {
                Assembly assm = GetAssembly();
                Type providerType = assm.GetType(this[ATTRIB_TYPE], false, false);
                Type iProv = providerType.GetInterface("IProvider");
                if (null != iProv && iProv.IsInterface) {
                    ConstructorInfo constInfo = providerType.GetConstructor(new Type[] { });
                    object provObj = constInfo.Invoke(new object[] { });
                    if (provObj is IProvider) {
                        _providerObjects.Add(hashName, provObj);
                        return provObj;
                    }
                }
            }
            return null;
        }

        public Guid ID {
            get {
                if (this.ContainsKey(ATTRIB_ID)) {
                    return new Guid(this[ATTRIB_ID]);
                } else {
                    return Guid.Empty;
                }
            }
        }

        public string Name {
            get {
                if (this.ContainsKey(ATTRIB_NAME)) {
                    return this[ATTRIB_NAME];
                } else {
                    return string.Empty;
                }
            }
        }

        internal Assembly GetAssembly() {
            if (this.ContainsKey(ATTRIB_ASSEMBLY)) {
                return GetAssembly(this[ATTRIB_ASSEMBLY]);
            }
            return null;
        }
        internal static Assembly GetAssembly(string assemblyName) {
            return Assembly.Load(assemblyName);
        }
    }
     * */

}

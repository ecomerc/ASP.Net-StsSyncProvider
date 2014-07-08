using System;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace ProviderProxy
{
    public class ProviderManager: IConfigurationSectionHandler
    {

        internal IProvider GetIProvider(Guid id)
        {
            Dictionary<Guid, Provider> providers = (Dictionary<Guid, Provider>)ConfigurationSettings.GetConfig("ProviderProxy/Providers");
            if (providers.ContainsKey(id))
                return (IProvider)providers[id].GetIProvider();

            return null;
        }
        internal Provider GetProvider(Guid id)
        {
            Dictionary<Guid, Provider> providers = (Dictionary<Guid, Provider>)ConfigurationSettings.GetConfig("ProviderProxy/Providers");
            if (providers.ContainsKey(id))
                return providers[id];

            return null;
        }
        internal List<Provider> GetAllProviders()
        {
            List<Provider> allProviders = new List<Provider>();
            Dictionary<Guid, Provider> providers = (Dictionary<Guid, Provider>)ConfigurationSettings.GetConfig("ProviderProxy/Providers");
            foreach (Guid providerID in providers.Keys)
            {
                allProviders.Add(providers[providerID]);
            }
            return allProviders;
        }
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            System.Collections.Generic.Dictionary<Guid, Provider> _providers = new Dictionary<Guid, Provider>();
            foreach (XmlNode child in section.ChildNodes)
            {
                if (XmlNodeType.Element == child.NodeType && child.LocalName == "Provider")
                {
                    Provider prov = new Provider();
                    foreach (XmlAttribute attrib in child.Attributes)
                    {
                        prov.Add(attrib.Name.ToLower(), attrib.Value);
                    }
                    _providers.Add(prov.ID, prov);
                }
            }
            return _providers;
        }

        #endregion
    }
    internal class Provider:System.Collections.Specialized.StringDictionary 
    {
        const string ATTRIB_TYPE = "type";
        const string ATTRIB_ID = "id";
        const string ATTRIB_ASSEMBLY = "assembly";
        const string ATTRIB_NAME = "name";

        static Hashtable _providerObjects = new Hashtable();

        internal object GetIProvider()
        {
            string hashName = this[ATTRIB_ID];
            if (_providerObjects.ContainsKey(hashName))
                return _providerObjects[hashName];


            if (this.ContainsKey(ATTRIB_ASSEMBLY) &&
                this.ContainsKey(ATTRIB_TYPE))
            {
                Assembly assm = GetAssembly();
                Type providerType = assm.GetType(this[ATTRIB_TYPE], false, false);
                Type iProv = providerType.GetInterface("IProvider");
                if (null != iProv && iProv.IsInterface)
                {
                    ConstructorInfo constInfo = providerType.GetConstructor(new Type[] { });
                    object provObj = constInfo.Invoke(new object[] { });
                    if (provObj is IProvider)
                    {
                        _providerObjects.Add(hashName, provObj);
                        return provObj;
                    }
                }
            }
            return null;
        }

        internal Guid ID
        {
            get
            {
                if (this.ContainsKey(ATTRIB_ID))
                {
                    return new Guid(this[ATTRIB_ID]);
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        internal string Name
        {
            get
            {
                if (this.ContainsKey(ATTRIB_NAME))
                {
                    return this[ATTRIB_NAME];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        internal Assembly GetAssembly()
        {
            if (this.ContainsKey(ATTRIB_ASSEMBLY))
            {
                return GetAssembly(this[ATTRIB_ASSEMBLY]);
            }
            return null;
        }
        internal static Assembly GetAssembly(string assemblyName)
        {
                return Assembly.Load(assemblyName);
        }
    }

}

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ProviderProxy
{
    public class ChangeKey
    {
        private string _key;
        public ChangeKey(string key)
        {
            _key = key;
        }
        public ChangeKey(int changeObjectType, int changeEventType, Guid objectGuid, DateTime timeOfChange, int changeNumber)
        {
            _key = changeObjectType.ToString() + ";" +
                changeEventType.ToString() + ";" +
                objectGuid.ToString("D") + ";" +
                timeOfChange.Ticks.ToString() + ";" +
                changeNumber.ToString();
        }
        public override string ToString()
        {
            return _key;
        }
        public string[] KeyParts
        {
            get
            {
                return _key.Split(';');
            }
        }
        public int ChangeObjectType
        {
            get
            {
                return int.Parse(KeyParts[0]);
            }
        }
        public int ChangeEventType
        {
            get
            {
                return int.Parse(KeyParts[1]);
            }
        }
        public Guid ObjectGuid
        {
            get
            {
                return new Guid(KeyParts[2]);
            }
        }
        public DateTime ChangeTime
        {
            get
            {
                return new DateTime(long.Parse(KeyParts[3]));
            }
        }
        public int ChangeNumber
        {
            get
            {
                return int.Parse(KeyParts[4]);
            }
        }
        

    }
}

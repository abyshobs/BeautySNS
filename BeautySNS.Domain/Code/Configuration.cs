using BeautySNS.Domain.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Code
{
    public class Configuration : IConfiguration
    {
        private static object getAppSetting(Type expectedType, string key)
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            if (value == null)
            {
                throw new Exception(string.Format("AppSetting: {0} i not configured.", key));
            }
            try
            {
                if (expectedType.Equals(typeof(int)))
                {
                    return int.Parse(value);
                }
                if (expectedType.Equals(typeof(string)))
                {
                    return value;
                }
                throw new Exception("Type not supported.");
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("Config key:{0} was expected to be of type {1} but was not.", key, expectedType), ex);
            }
        }

        public string RootURL
        {
            get { return getAppSetting(typeof(string), "RootURL").ToString(); }
        }
    }
}

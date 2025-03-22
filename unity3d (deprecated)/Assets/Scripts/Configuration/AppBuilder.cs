using Microsoft.Extensions.Configuration;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class AppBuilder
    {
        private static IConfiguration _configuration = null;

        private static readonly string _jsonFilePath =
#if UNITY_STANDALONE
                "appsettings.standalone"
#elif UNITY_ANDROID
                "appsettings.android"
#elif UNITY_IOS
                "appsettings.ios"
#endif
                ;

        public static IConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                var jsonFile = Resources.Load<TextAsset>(_jsonFilePath);
                using MemoryStream stream = new MemoryStream(jsonFile.bytes);
                var builder = new ConfigurationBuilder().AddJsonStream(stream);

                _configuration = builder.Build();
            }

            return _configuration;
        }
    }
}

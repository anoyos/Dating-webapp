using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace james.Helpers.General
{
    public class Common
    {
        
        public static string Decrypt(string input, string key)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        public static string Serialize(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None });
            return result;
        }

        public static object Deserialize(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static T Deserialize<T>(string json, IsoDateTimeConverter dateformat)
        {
            return JsonConvert.DeserializeObject<T>(json, dateformat);
        }
        public static string Base64ToFile(string basePath, string base64, string extension)
        {
            try
            {
                if (string.IsNullOrEmpty(base64)) { return null; };


                string filename = Guid.NewGuid().ToString() + extension;

                var folder = Path.Combine(basePath, "img", "upload");           

                string path = Path.Combine(folder, filename);

                byte[] audioBytes = Convert.FromBase64String(base64);

                File.WriteAllBytes(path, audioBytes);

                return  filename;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal static double? GetDistance(string lat1_str, string lng1_str, string lat2_str, string lng2_str)
        {
            double lat1, lng1, lat2, lng2;
            if (double.TryParse(lat1_str,out lat1) && double.TryParse(lng1_str, out lng1) 
                && double.TryParse(lat2_str, out lat2) && double.TryParse(lng2_str, out lng2))
            {

               return DistanceBetweenPlaces(lat1, lng1, lat2, lng2);
            }
            return null;
        }
        const double PIx = Math.PI;
        const double RADIO = 6378.16;
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }
        public static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            double R = 6371; // km

            double sLat1 = Math.Sin(Radians(lat1));
            double sLat2 = Math.Sin(Radians(lat2));
            double cLat1 = Math.Cos(Radians(lat1));
            double cLat2 = Math.Cos(Radians(lat2));
            double cLon = Math.Cos(Radians(lon1) - Radians(lon2));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            return dist;
        }
    }
}

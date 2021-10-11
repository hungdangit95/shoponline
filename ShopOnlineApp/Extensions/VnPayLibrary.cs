using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace ShopOnlineApp.Extensions
{
    public class VnPayLibrary
    {
        private readonly HttpContextAccessor _accessor;
        public VnPayLibrary(HttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        private SortedList<String, String> _requestData = new SortedList<String, String>(new VnPayCompare());
        private SortedList<String, String> _responseData = new SortedList<String, String>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }
        public  string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
                //    ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
        public void AddResponseData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }

        #region Request

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(kv.Key + "=" + HttpUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();
            string rawData = GetRequestRaw();
            baseUrl += "?" + queryString;
           string vnp_SecureHash = Utils.Md5(vnp_HashSecret + rawData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;
            return baseUrl;
        }

        private string GetRequestRaw()
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(kv.Key + "=" + kv.Value + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion

        #region Response process

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseRaw();
            string myChecksum = Utils.Md5(secretKey + rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseRaw()
        {

            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(kv.Key + "=" + kv.Value + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion
    }

    public class Utils
    {
        public static string Md5(string sInput)
        {
            HashAlgorithm algorithmType = default(HashAlgorithm);
            ASCIIEncoding enCoder = new ASCIIEncoding();
            byte[] valueByteArr = enCoder.GetBytes(sInput);
            byte[] hashArray = null;
            // Encrypt Input string 
            algorithmType = new MD5CryptoServiceProvider();
            hashArray = algorithmType.ComputeHash(valueByteArr);
            //Convert byte hash to HEX
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashArray)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
       
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}

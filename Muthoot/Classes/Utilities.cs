using Muthoot.Models;
using ClosedXML.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Muthoot.Classes
{
    [Serializable]
    public class Utilities
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Utilities(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment env)
        {
            try
            {
                _httpContextAccessor = httpContextAccessor;
                _configuration = configuration;
                _env = env;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public string CallWebService(string URL)
        {
            try
            {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(URL);
                objRequest.Method = "GET";
                objRequest.KeepAlive = false;

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsMobileDevice()
        {
            try
            {
                bool blnResult = false;
                var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
                Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string device_info = string.Empty;
                if (OS.IsMatch(userAgent)) device_info = OS.Match(userAgent).Groups[0].Value;
                if (device.IsMatch(userAgent.Substring(0, 4))) device_info += device.Match(userAgent).Groups[0].Value;
                if (!string.IsNullOrEmpty(device_info)) blnResult = true;

                return blnResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteToFile(string process, string controller, string action, string message)
        {
            try
            {
                string log = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}|{controller}|{action}|{message}";
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", DateTime.Now.ToString("MMyyyy"), process);
                Directory.CreateDirectory(folderPath);
                string filePath = Path.Combine(folderPath, $"{process}_ErrorLog_{DateTime.Now:dd_MM_yyyy}.txt");
                File.AppendAllText(filePath, log + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Encrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e,
                    0x20, 0x4d, 0x65, 0x64,
                    0x76, 0x65, 0x64, 0x65, 0x76
                });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e,
                    0x20, 0x4d, 0x65, 0x64,
                    0x76, 0x65, 0x64, 0x65, 0x76
                });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExportExcel(DataTable dt, string FileName)
        {
            try
            {
                // Get relative folder path from appsettings.json config, e.g. "Reports/"
                string relativePath = _configuration["reportPath"];

                // Combine with physical path of wwwroot or content root
                string folderPath = Path.Combine(_env.WebRootPath, relativePath ?? "");

                // Make sure directory exists
                Directory.CreateDirectory(folderPath);

                // Full file path with file name
                string fullPath = Path.Combine(folderPath, FileName + ".xlsx");

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Data");
                    wb.SaveAs(fullPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExportCSV(DataTable dt, string FileName)
        {
            try
            {
                var response = _httpContextAccessor.HttpContext.Response;

                response.Clear();
                response.ContentType = "text/csv";
                response.Headers.Add("Content-Disposition", $"attachment; filename={FileName}.csv");

                var csvBuilder = new StringBuilder();

                foreach (DataColumn column in dt.Columns)
                    csvBuilder.Append(column.ColumnName + ",");

                csvBuilder.AppendLine();

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                        csvBuilder.Append(row[column.ColumnName].ToString().Replace(",", ";") + ",");
                    csvBuilder.AppendLine();
                }

                using (var writer = new StreamWriter(response.Body, Encoding.UTF8))
                {
                    writer.Write(csvBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            try
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                    dataTable.Columns.Add(prop.Name);
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                        values[i] = Props[i].GetValue(item, null);
                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get_ContentType(string extension)
        {
            try
            {
                switch (extension.ToLower())
                {
                    case ".pdf": return "application/pdf";
                    case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    case ".xls": return "application/vnd.ms-excel";
                    case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    case ".png": return "image/png";
                    case ".jpg": return "image/jpeg";
                    case ".gif": return "image/gif";
                    case ".bmp": return "image/bmp";
                    case ".zip": return "application/zip";
                    case ".xml": return "application/xml";
                    case ".txt": return "text/plain";
                    default: return "application/octet-stream";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ConvertListToDataTable(List<string[]> list)
        {
            try
            {
                DataTable table = new DataTable();
                int columns = list.Max(x => x.Length);
                for (int i = 0; i < columns; i++)
                {
                    table.Columns.Add($"Column{i + 1}");
                }

                foreach (var row in list)
                {
                    var newRow = table.NewRow();
                    for (int i = 0; i < row.Length; i++)
                    {
                        newRow[i] = row[i];
                    }
                    table.Rows.Add(newRow);
                }
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable JsonStringToDataTable(string jsonString)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
                List<string> ColumnsName = new List<string>();
                foreach (string jSA in jsonStringArray)
                {
                    string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                    foreach (string ColumnsNameData in jsonStringData)
                    {
                        try
                        {
                            int idx = ColumnsNameData.IndexOf(":");
                            string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                            if (!ColumnsName.Contains(ColumnsNameString))
                            {
                                ColumnsName.Add(ColumnsNameString);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                        }
                    }
                    break;
                }
                foreach (string AddColumnName in ColumnsName)
                {
                    dt.Columns.Add(AddColumnName);
                }
                foreach (string jSA in jsonStringArray)
                {
                    string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                    DataRow nr = dt.NewRow();
                    foreach (string rowData in RowData)
                    {
                        try
                        {
                            int idx = rowData.IndexOf(":");
                            string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                            string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                            nr[RowColumns] = RowDataString;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    dt.Rows.Add(nr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable ListToDataTable<T>(List<T> items)
        {
            try
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsValidEmailId(string InputEmail)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(InputEmail);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long ConvertToTS(DateTime datetime)
        {
            try
            {
                DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return (long)(datetime - sTime).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getDecryptedValue_dialer(string uniqueIdentifier, string sessionId, string Process)
        {
            try
            {

                string URL = "";

                if (Process =="PL")
                {
                    URL = System.Configuration.ConfigurationManager.AppSettings["PL_Dialer_Phone_Decrypt_URL"];
                }
                else if (Process == "GBZ")
                {

                    URL = System.Configuration.ConfigurationManager.AppSettings["GBZ_Dialer_Phone_Decrypt_URL"];
                }

                string endPointURL = URL + "?uniqueIdentifier=" + uniqueIdentifier;

                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(endPointURL);
                objRequest.Method = "GET";
                objRequest.KeepAlive = false;
                objRequest.Headers.Add("sessionId", sessionId);

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }

                dynamic data = JObject.Parse(result);
                return data.decryptedValue;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }



        public DateRange Get_Date_Range(string searchType, string stDate, string endDate)
        {
            try
            {
                DateRange _cls = new DateRange();

                if (searchType == null)
                {
                    _cls.fromDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                    _cls.toDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                }
                else if (searchType == "Today")
                {
                    _cls.fromDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                    _cls.toDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                }

                else if (searchType == "One Week")
                {
                    _cls.fromDate = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    _cls.toDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                }
                else if (searchType == "One Month")
                {
                    _cls.fromDate = DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy");
                    _cls.toDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                }
                else if (searchType == "Three Months")
                {
                    _cls.fromDate = DateTime.Today.AddDays(-90).ToString("dd/MM/yyyy");
                    _cls.toDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                }
                else if (searchType == "Six Months")
                {
                    _cls.fromDate = DateTime.Today.AddDays(-180).ToString("dd/MM/yyyy");
                    _cls.toDate = DateTime.Today.AddDays(0).ToString("dd/MM/yyyy");
                }
                else if (searchType == "Custom")
                {

                    _cls.fromDate = Convert.ToDateTime(stDate).ToString("dd/MM/yyyy");
                    _cls.toDate = Convert.ToDateTime(endDate).ToString("dd/MM/yyyy");
                }
                return _cls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

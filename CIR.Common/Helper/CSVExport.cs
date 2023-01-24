using CIR.Common.CustomResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace CIR.Common.Helper
{
    public class CSVExport
    {
        public IActionResult ExportToCsv([FromBody] object model)
        {
            if (model != null)
            {
                var stuff = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(model.ToString());
                var key = new List<string>();
                var data = new List<List<string>>();
                var data2 = new List<List<string>>();

                var count = 0;
                foreach (var item in stuff)
                {
                    var val = new List<string>();
                    foreach (var items in item)
                    {
                        if (count == 0)
                        {
                            key.Add(items.Key);
                        }
                        val.Add(items.Value);
                    }
                    count++;
                    data.Add(val);
                }
                data2.Add(key);
                data2.AddRange(data);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < data2.Count; i++)
                {
                    string[] u = (string[])data2[i].ToArray();
                    for (int j = 0; j < u.Length; j++)
                    {
                        sb.Append(u[j] + ',');
                    }
                    sb.Append("\r\n");
                }

                var bytesdata = System.IO.File.ReadAllBytes(sb.ToString());
                return new FileContentResult(bytesdata, "application/pdf");
            }
            else
            {
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
            }
        }
    }
}


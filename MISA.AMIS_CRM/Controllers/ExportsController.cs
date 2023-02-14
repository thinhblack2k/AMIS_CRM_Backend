using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS_CRM.Entities;
using MySqlConnector;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MISA.AMIS_CRM.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        readonly string _connectionString;
        MySqlConnection _connection;

        public ExportController()
        {
            // Khai báo database
            _connectionString = "Host=localhost; " +
                    "Port=3306; " +
                    "Database=MISA.QTKD.THINH; " +
                    "User Id=root; " +
                    "Password=1234; ";

            // Khởi tạo kết nối
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        [HttpGet]
        public async Task<IActionResult> GetExcelFiels(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Yield();
                // Thực thi lấy dữ liệu:
                string sqlCommand = "SELECT * FROM Leads";

                // Trả về dữ liệu cho client:
                var leads = _connection.Query<Lead>(sqlCommand).ToList();

                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    // Tạo một sheet mới
                    var workSheet = package.Workbook.Worksheets.Add("CRM LEADS");

                    // Mặc định chiều rộng
                    workSheet.DefaultColWidth = 10;

                    // create title
                    workSheet.Cells["A1:F1"].Merge = true;
                    workSheet.Cells["A1"].Value = "Danh sách tiềm năng";
                    workSheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["A1"].Style.Font.Bold = true;

                    // fill header
                    List<string> listHeader = new List<string>()
                    {
                        "A2","B2","C2","D2","E2","F2", "G2","H2","I2","J2","K2","L2","M2","N2","O2","P2","Q2","R2"
                    };
                    listHeader.ForEach(c =>
                    {
                        workSheet.Cells[c].Style.Font.Bold = true;
                        workSheet.Cells[c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    });
                    workSheet.Cells[listHeader[0]].Value = "#";
                    workSheet.Cells[listHeader[1]].Value = "LeadID";
                    workSheet.Cells[listHeader[2]].Value = "LeadCode";
                    workSheet.Cells[listHeader[3]].Value = "FirstName";
                    workSheet.Cells[listHeader[4]].Value = "LastName";
                    workSheet.Cells[listHeader[5]].Value = "Mobile";
                    workSheet.Cells[listHeader[6]].Value = "OfficeMobile";
                    workSheet.Cells[listHeader[7]].Value = "Email";
                    workSheet.Cells[listHeader[8]].Value = "OfficeEmail";
                    workSheet.Cells[listHeader[9]].Value = "CompanyName";
                    workSheet.Cells[listHeader[10]].Value = "Address";
                    workSheet.Cells[listHeader[11]].Value = "ProvinceName";
                    workSheet.Cells[listHeader[12]].Value = "DistrictName";
                    workSheet.Cells[listHeader[13]].Value = "WardName";
                    workSheet.Cells[listHeader[14]].Value = "LeadSourceName";
                    workSheet.Cells[listHeader[15]].Value = "BusinessTypeName";
                    workSheet.Cells[listHeader[16]].Value = "SectorName";
                    workSheet.Cells[listHeader[17]].Value = "Description";
                    //fill data
                    for (int i = 0; i < leads.Count; i++)
                    {
                        workSheet.Cells[i + 3, 1].Value = (i + 1).ToString();
                        workSheet.Cells[i + 3, 2].Value = leads[i].LeadID;
                        workSheet.Cells[i + 3, 3].Value = leads[i].LeadCode;
                        workSheet.Cells[i + 3, 4].Value = (leads[i].FirstName != null) ? leads[i].FirstName : "-";
                        workSheet.Cells[i + 3, 5].Value = leads[i].LastName != null ? leads[i].LastName : "-";
                        workSheet.Cells[i + 3, 6].Value = leads[i].Mobile != null ? leads[i].Mobile : "-";
                        workSheet.Cells[i + 3, 7].Value = leads[i].OfficeMobile != null ? leads[i].OfficeMobile : "-";
                        workSheet.Cells[i + 3, 8].Value = leads[i].Email != null ? leads[i].Email : "-";
                        workSheet.Cells[i + 3, 9].Value = leads[i].OfficeEmail != null ? leads[i].OfficeEmail : "-";
                        workSheet.Cells[i + 3, 10].Value = leads[i].CompanyName != null ? leads[i].CompanyName : "-";
                        workSheet.Cells[i + 3, 11].Value = leads[i].Address != null ? leads[i].Address : "-";
                        workSheet.Cells[i + 3, 12].Value = leads[i].ProvinceName != null ? leads[i].ProvinceName : "-";
                        workSheet.Cells[i + 3, 13].Value = leads[i].DistrictName != null ? leads[i].DistrictName : "-";
                        workSheet.Cells[i + 3, 14].Value = leads[i].WardName != null ? leads[i].WardName : "-";
                        workSheet.Cells[i + 3, 15].Value = leads[i].LeadSourceName != null ? leads[i].LeadSourceName : "-";
                        workSheet.Cells[i + 3, 16].Value = leads[i].BusinessTypeName != null ? leads[i].BusinessTypeName : "-";
                        workSheet.Cells[i + 3, 17].Value = leads[i].SectorName != null ? leads[i].SectorName : "-";
                        workSheet.Cells[i + 3, 18].Value = leads[i].Description != null ? leads[i].Description : "-";

                    }

                    // format column width
                    for (int i = 1; i < 19; i++)
                    {
                        workSheet.Column(i).AutoFit();
                    }

                    // format cell border
                    for (int i = 0; i < leads.Count; i++)
                    {
                        for (int j = 1; j < 19; j++)
                        {
                            workSheet.Cells[i + 2, j].Style.Font.Size = 10;
                            workSheet.Cells[i + 2, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 2, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 2, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 2, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }

                    package.Save();

                }
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Leads.xlsx");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(400, ex.Message);

            }
        }
    }
}
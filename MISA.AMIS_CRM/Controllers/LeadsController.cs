using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS_CRM.Entities;
using MISA.AMIS_CRM.Models;
using MySqlConnector;

namespace MISA.AMIS_CRM.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {

        // Khai báo thông tin database
        string _connectionString = "Host=localhost; " +
                    "Port=3306;" +
                    "Database=MISA.QTKD.THINH; " +
                    "User Id=root; " +
                    "Password=1234; ";
        //Khởi tạo kết nối
        MySqlConnection _connection;

        //Hàm khởi tạo ban đầu
        public LeadsController()
        {
            _connection = new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Lấy dữ liệu của tiềm năng
        /// </summary>
        /// <returns>danh sach tiềm năng</returns>
        /// CreatedBy: DHTHINH (03/08/20022)
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Thực thi lấy dữ liệu:
                var sqlCommand = "SELECT * FROM Leads";

                // Trả về dữ liệu cho client:
                var data = _connection.Query<object>(sqlCommand);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Lấy ra tiềm năng bằng id
        /// </summary>
        /// <param name="id">id của tiềm năng</param>
        /// <returns>danh sách tiềm năng</returns>
        [HttpGet("{id}")]
        public IActionResult GetLeadById(Guid id)
        {
            try
            {
                // Thực thi lấy dữ liệu:
                var sqlCommand = $"SELECT * FROM Leads WHERE LeadId = '{id.ToString()}'";

                // Thực hiện lấy dữ liệu:
                var lead = _connection.QueryFirstOrDefault<object>(sqlCommand);

                // Trả về dữ liệu cho client:
                return Ok(lead);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        /// <summary>
        /// Lấy ngẫu nhiên 1 mã tiềm năng
        /// </summary>
        /// CreatedBy: DHTHINH(04/08/2022)
        /// <returns></returns>
        [HttpGet("NewLeadCode")]
        public IActionResult GetNewLeadCode()
        {
            try
            {
                Random random = new Random();
                var numberRandom = random.Next(100, 1000);//biến numberRandom sẽ nhận có giá trị ngẫu nhiên trong khoảng 1 đến 100
                string Numrd_str = random.Next(100, 1000).ToString();//Chuyển giá trị ramdon về kiểu string
                var newCode = "TN-00000" + Numrd_str;
                return Ok(newCode);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi 
        /// </summary>
        /// <param name="lead">Đối tượng lead thêm vào</param>
        /// <returns>lead</returns>
        /// CreateBy: DHTHINH(04/08/2000)
        [HttpPost]
        public IActionResult Post(Lead lead)
        {
            try
            {
                // Khai báo các thông tin cần thiết
                var error = new ErrorService();
                var errorData = new Dictionary<string, string>();
                var errorMsgs = new List<string>(); //cách 2

                // Validate dữ liệu: trả về mã 400 (Bad request) kèm các thông tin lỗi cần thiết

                // Thông tin mã nhân viên phải bắt buộc nhập
                if (string.IsNullOrEmpty(lead.LeadCode))
                {
                    errorData.Add("LeadName", "Mã khách hàng tiềm năng không được phép để trống");
                }

                // Thông tin họ và tên phải bắt buộc nhập
                if (string.IsNullOrEmpty(lead.FirstName))
                {
                    errorData.Add("LeadName", "Họ và tên khách hàng tiềm năng không được phép để trống");
                }

                //Email phải đúng định dạng


                if (errorData.Count > 0)
                {
                    error.UserMsg = "Dữ liệu đầu vào không hợp lệ";
                    error.Data = errorData;
                    return BadRequest(error);
                }


                // Thực hiện thêm mới dữ liệu vào database
                var sqlCommand = "INSERT INTO Leads (LeadID,LeadCode,LastName,FirstName, Mobile, OfficeMobile , Email," +
                    " OfficeEmail, Address, TitleID, TitleName, AnnualRevenueID, SalutationID, SalutationName, DepartmentID," +
                    "DepartmentName, LeadSourceID, LeadSourceName, LeadTypeID, LeadTypeName, Zalo, CompanyName, TaxCode, " +
                    "BankAccount, EstablishDay, SectorID, SectorName, BankName, BusinessTypeID, BusinessTypeName, IndustryID," +
                    "IndustryName, CountryID, CountryName, DistrictID, DistrictName, Street, ProvinceID, ProvinceName, WardID," +
                    "WardName, ZipCode, Description, IsPublic, CreatedBy)" +
                        "VALUES (@LeadID,@LeadCode,@LastName,@FirstName, @Mobile, @OfficeMobile , @Email," +
                    " @OfficeEmail, @Address, @TitleID, @TitleName, @AnnualRevenueID, @SalutationID, @SalutationName, @DepartmentID," +
                    "@DepartmentName, @LeadSourceID, @LeadSourceName, @LeadTypeID, @LeadTypeName, @Zalo, @CompanyName, @TaxCode, " +
                    "@BankAccount, @EstablishDay, @SectorID, @SectorName, @BankName, @BusinessTypeID, @BusinessTypeName, @IndustryID," +
                    "@IndustryName, @CountryID, @CountryName, @DistrictID, @DistrictName, @Street, @ProvinceID, @ProvinceName, @WardID," +
                    "@WardName, @ZipCode, @Description, @IsPublic, @CreatedBy)";
                var leadId = Guid.NewGuid();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LeadID", leadId);
                parameters.Add("@LeadCode", lead.LeadCode);
                parameters.Add("@LastName", lead.LastName);
                parameters.Add("@FirstName", lead.FirstName);
                parameters.Add("@Mobile", lead.Mobile);
                parameters.Add("@OfficeMobile", lead.OfficeMobile);
                parameters.Add("@Email", lead.Email);
                parameters.Add("@OfficeEmail", lead.OfficeEmail);
                parameters.Add("@Address", lead.Address);
                parameters.Add("@TitleID", lead.TitleID);
                parameters.Add("@TitleName", lead.TitleName);
                parameters.Add("@AnnualRevenueID", lead.AnnualRevenueID);
                parameters.Add("@SalutationID", lead.SalutationID);
                parameters.Add("@SalutationName", lead.SalutationName);
                parameters.Add("@DepartmentID", lead.DepartmentID);
                parameters.Add("@DepartmentName", lead.DepartmentName);
                parameters.Add("@LeadSourceID", lead.LeadSourceID);
                parameters.Add("@LeadSourceName", lead.LeadSourceName);
                parameters.Add("@LeadTypeID", lead.LeadTypeID);
                parameters.Add("@LeadTypeName", lead.LeadTypeName);
                parameters.Add("@Zalo", lead.Zalo);
                parameters.Add("@CompanyName", lead.CompanyName);
                parameters.Add("@TaxCode", lead.TaxCode);
                parameters.Add("@BankAccount", lead.BankAccount);
                parameters.Add("@EstablishDay", lead.EstablishDay);
                parameters.Add("@SectorID", lead.SectorID);
                parameters.Add("@SectorName", lead.SectorName);
                parameters.Add("@BankName", lead.BankName);
                parameters.Add("@BusinessTypeID", lead.BusinessTypeID);
                parameters.Add("@BusinessTypeName", lead.BusinessTypeName);
                parameters.Add("@IndustryID", lead.IndustryID);
                parameters.Add("@IndustryName", lead.IndustryName);
                parameters.Add("@CountryID", lead.CountryID);
                parameters.Add("@CountryName", lead.CountryName);
                parameters.Add("@DistrictID", lead.DistrictID);
                parameters.Add("@DistrictName", lead.DistrictName);
                parameters.Add("@Street", lead.Street);
                parameters.Add("@ProvinceID", lead.ProvinceID);
                parameters.Add("@ProvinceName", lead.ProvinceName);
                parameters.Add("@WardID", lead.WardID);
                parameters.Add("@WardName", lead.WardName);
                parameters.Add("@ZipCode", lead.ZipCode);
                parameters.Add("@Description", lead.Description);
                parameters.Add("@IsPublic", lead.IsPublic);
                parameters.Add("@CreatedBy", lead.CreatedBy);

                var executeAddLead = _connection.Execute(sqlCommand, parameters);

                // Trả thông tin về cho client
                if (executeAddLead > 0)
                    return StatusCode(201);
                else
                    return Ok(executeAddLead);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Sửa thông tin tiềm năng theo id
        /// </summary>
        /// <param name="lead">Mã tiềm năng</param>
        /// <returns>Tiềm năng mới</returns>
        /// CreateBy: DHTHINH(08/08/2022)
        [HttpPut("{LeadID}")]
        public IActionResult Put(Guid LeadID, Lead lead)
        {

            // Khai báo các thông tin cần thiết
            var error = new ErrorService();
            var errorData = new Dictionary<string, string>();

            // Bước 2: Khởi tạo kết nối database

            // Bước 3: Thực hiện thêm mới dữ liệu vào database
            var sqlCommand = "UPDATE Leads SET FirstName = @FirstName,LastName = @LastName, Mobile = @Mobile," +
                "OfficeMobile = @OfficeMobile, Email = @Email, OfficeEmail = @OfficeEmail,Address = @Address, TitleID = @TitleID," +
                "TitleName = @TitleName, AnnualRevenueID = @AnnualRevenueID, SalutationID = @SalutationID, DepartmentID = @DepartmentID," +
                "DepartmentName = @DepartmentName, LeadSourceID = @LeadSourceID, LeadSourceName = @LeadSourceName, LeadTypeID = @LeadTypeID," +
                "LeadTypeName = @LeadTypeName, Zalo = @Zalo, CompanyName = @CompanyName, TaxCode = @TaxCode, BankAccount = @BankAccount," +
                "EstablishDay = @EstablishDay, SectorID = @SectorID, SectorName = @SectorName, BankName = @BankName, BusinessTypeID = @BusinessTypeID," +
                "BusinessTypeName = @BusinessTypeName, IndustryID = @IndustryID, IndustryName = @IndustryName, CountryID = @CountryID," +
                "CountryName = @CountryName, DistrictID = @DistrictID, DistrictName = @DistrictName, Street = @Street, ProvinceID = @ProvinceID," +
                "ProvinceName = @ProvinceName, WardID = @WardID, WardName = @WardName, ZipCode = @ZipCode, Description = @Description," +
                "IsPublic = @IsPublic, CreatedBy = @CreatedBy" +
                " WHERE LeadID = @LeadID";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@LeadID", LeadID);
            parameters.Add("@LeadCode", lead.LeadCode);
            parameters.Add("@FirstName", lead.FirstName);
            parameters.Add("@LastName", lead.LastName);
            parameters.Add("@Mobile", lead.Mobile);
            parameters.Add("@OfficeMobile", lead.OfficeMobile);
            parameters.Add("@Email", lead.Email);
            parameters.Add("@OfficeEmail", lead.OfficeEmail);
            parameters.Add("@Address", lead.Address);
            parameters.Add("@TitleID", lead.TitleID);
            parameters.Add("@TitleName", lead.TitleName);
            parameters.Add("@AnnualRevenueID", lead.AnnualRevenueID);
            parameters.Add("@SalutationID", lead.SalutationID);
            parameters.Add("@SalutationName", lead.SalutationName);
            parameters.Add("@DepartmentID", lead.DepartmentID);
            parameters.Add("@DepartmentName", lead.DepartmentName);
            parameters.Add("@LeadSourceID", lead.LeadSourceID);
            parameters.Add("@LeadSourceName", lead.LeadSourceName);
            parameters.Add("@LeadTypeID", lead.LeadTypeID);
            parameters.Add("@LeadTypeName", lead.LeadTypeName);
            parameters.Add("@Zalo", lead.Zalo);
            parameters.Add("@CompanyName", lead.CompanyName);
            parameters.Add("@TaxCode", lead.TaxCode);
            parameters.Add("@BankAccount", lead.BankAccount);
            parameters.Add("@EstablishDay", lead.EstablishDay);
            parameters.Add("@SectorID", lead.SectorID);
            parameters.Add("@SectorName", lead.SectorName);
            parameters.Add("@BankName", lead.BankName);
            parameters.Add("@BusinessTypeID", lead.BusinessTypeID);
            parameters.Add("@BusinessTypeName", lead.BusinessTypeName);
            parameters.Add("@IndustryID", lead.IndustryID);
            parameters.Add("@IndustryName", lead.IndustryName);
            parameters.Add("@CountryID", lead.CountryID);
            parameters.Add("@CountryName", lead.CountryName);
            parameters.Add("@DistrictID", lead.DistrictID);
            parameters.Add("@DistrictName", lead.DistrictName);
            parameters.Add("@Street", lead.Street);
            parameters.Add("@ProvinceID", lead.ProvinceID);
            parameters.Add("@ProvinceName", lead.ProvinceName);
            parameters.Add("@WardID", lead.WardID);
            parameters.Add("@WardName", lead.WardName);
            parameters.Add("@ZipCode", lead.ZipCode);
            parameters.Add("@Description", lead.Description);
            parameters.Add("@IsPublic", lead.IsPublic);
            parameters.Add("@CreatedBy", lead.CreatedBy);
            var executeFixLead = _connection.Execute(sqlCommand, parameters);

            // Bước 4: Trả thông tin về cho client
            if (executeFixLead > 0)
                return StatusCode(201);
            else
                return Ok(executeFixLead);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById (Guid id)
        {
            try
            {
                // Thực thi lấy dữ liệu:
                var sqlCommand = $"DELETE FROM Leads WHERE LeadId = '{id.ToString()}'";

                // Thực hiện lấy dữ liệu:
                var lead = _connection.QueryFirstOrDefault<Lead>(sqlCommand);

                // Trả về dữ liệu cho client:
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Đưa ra lỗi của phần mềm đang gặp phải
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>Thông báo lỗi</returns>
        /// CreatedBy DHTHINH(03/08/2022)
        private IActionResult HandleException(Exception ex)
        {
            var res = new
            {
                devMsg = "Error",
                userMsg = " Có lỗi xảy ra vui lòng liên hệ misa để được trợ giúp",
            };
            return StatusCode(500, ex.Message);
        }
    }
}

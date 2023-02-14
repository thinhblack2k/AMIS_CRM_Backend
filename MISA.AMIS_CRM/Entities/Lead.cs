namespace MISA.AMIS_CRM.Entities
{
    public class Lead
    {
        /// <summary>
        /// ID khách hàng tiềm năng
        /// </summary>
        public Guid LeadID { get; set; }

        /// <summary>
        /// Mã khách hàng tiềm năng
        /// </summary>
        public string LeadCode { get; set; }

        /// <summary>
        /// Họ khách hàng tiềm năng
        /// </summary>
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string? Mobile { get; set; }
        public string? OfficeMobile { get; set; }
        public string? Email { get; set; }
        public string? OfficeEmail { get; set; }
        public string? Address { get; set; }
        public int? TitleID { get; set; }
        public string? TitleName { get; set; }
        public int? AnnualRevenueID { get; set; }
        public string? AnnualRevenueName { get; set; }
        public int? SalutationID { get; set; }
        public string? SalutationName { get; set; }
        public int? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public int? LeadSourceID { get; set; }
        public string? LeadSourceName { get; set; }
        public int? LeadTypeID { get; set; }
        public string? LeadTypeName { get; set; }
        public string? Zalo { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxCode { get; set; }
        public string? BankAccount { get; set; }
        public DateTime? EstablishDay { get; set; }
        public int? SectorID { get; set; }
        public string? SectorName { get; set; }
        public string? BankName { get; set; }
        public int? BusinessTypeID { get; set; }
        public string? BusinessTypeName { get; set; }
        public int? IndustryID { get; set; }
        public string? IndustryName { get; set; }
        public int? CountryID { get; set; }
        public string? CountryName { get; set; }
        public int? DistrictID { get; set; }
        public string? DistrictName { get; set; }
        public string? Street { get; set; }
        public int? ProvinceID { get; set; }
        public string? ProvinceName { get; set; }
        public int? WardID { get; set; }
        public string? WardName { get; set; }
        public string? ZipCode { get; set; }
        public string? Description { get; set; }
        public Boolean IsPublic { get; set; }
        public string? CreatedBy { get; set; }

    }
}

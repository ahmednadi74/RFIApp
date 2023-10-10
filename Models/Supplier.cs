using RFIApp.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class Supplier
{  public Guid SupplierID { get; set; }
    public int LeanID { get; set; }
    [MaxLength(50)]
    [Display(Name ="LegalName")]
    public string? LegalName { get; set; }
    [MaxLength(50)]
    public string? TradingName { get; set; }
    [MaxLength(256)]
    public string? CompanyAddress { get; set; }
    [MaxLength(256)]
    public string? PostalAddress { get; set; }
    [MaxLength(20)]
    [DataType(DataType.PhoneNumber)]
    public string? TelephoneNumber { get; set; }
    [DataType(DataType.Url),MaxLength(50)]
    public string? WebAddress { get; set; }
    [MaxLength(50)]
    public string? PostCode { get; set; }
    [MaxLength(50)]
    public string? FaxNumber { get; set; }
    public string? ContactedMethod { get; set; }
    public TypeOfBusiness? TypeOfBusiness { get; set; }
    public int NoOfEmployees { get; set; }
    public bool IsLocked { get; set; }

    public bool EmployeesAreSubscribedToSocialSecurity { get; set; }
    public string ? FileAttachmentPath { get; set; }
    public string? FileAttachmentName { get; set; }


    public List<Contact> Contacts { get; set; }
    public List<BankDetails> BankDetails { get; set; }
    public List<ProductionInformation> ProductionInformation { get; set; }
}
public enum TypeOfBusiness
{
    [Description("Agent/Distributor")]
    AgentDistributor = 1,
    [Description("Manufacturer")]
    Manufacturer = 2,
    [Description("Trader")]
    Trader = 3,
    [Description("Other")]
    Other = 99
}
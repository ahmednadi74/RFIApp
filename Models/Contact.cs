using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class Contact
{  
    public Guid ContactID { get; set; }
    [Required, MaxLength(50)]
    public string ContactName { get; set; }
    [Required,DataType(DataType.PhoneNumber),MaxLength(20)]
    public string ContactTelNo { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    public ContactType ContactType { get; set; }
    public Guid SupplierID { get; set; }
    public Supplier Supplier { get; set; }
}
public enum ContactType
{
    [Description("MD/GM")]
    MD_GM = 1,
    [Description("SALES")]
    SALES = 2,
    [Description("Order Processing")]
    OrderProcessing = 3,
    [Description("Accounts")]
    Accounts = 50
}
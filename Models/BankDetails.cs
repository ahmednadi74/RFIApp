using System.ComponentModel.DataAnnotations;

public class BankDetails
{
    public Guid BankDetailsID { get; set; }

    [Required, MaxLength(50)]
    public string BankName { get; set; }

    [Required, MaxLength(50)]
    public string BeneficiaryName { get; set; }

    [Required]
    public string SwiftBIC { get; set; }

    [Required]
    public string BankAccountNo { get; set; }

    [Required, MaxLength(500)]
    public string BankAddress { get; set; }

    [Required, MaxLength(50)]
    public string IBAN { get; set; }

    [Required]
    public string AccountCurrency { get; set; }

    [Required, MaxLength(500)]
    public string PaymentTerm { get; set; }

    public Guid SupplierID { get; set; }

    public Supplier Supplier { get; set; }
}
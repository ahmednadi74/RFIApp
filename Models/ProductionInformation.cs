using System.ComponentModel.DataAnnotations;

public class ProductionInformation
{
    public Guid ProductionInformationID { get; set; }

    [Required, MaxLength(500)]
    public string Item { get; set; }

    [Required]
    public decimal ProductionCapacity { get; set; }

    [Required]
    public decimal ExportCapacity { get; set; }

    //[Required, MaxLength(500)]
    public string? ContactPerson { get; set; }

   // [Required, MaxLength(20)]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNo { get; set; }

    // [Required]
    
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string? EmailAddress { get; set; }

    public Guid SupplierID { get; set; }

    public Supplier Supplier { get; set; }
}
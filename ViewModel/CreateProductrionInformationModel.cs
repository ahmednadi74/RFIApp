using System.ComponentModel.DataAnnotations;

namespace RFIApp.ViewModel
{
    public class CreateProductrionInformationModel
    {
        public Guid ProductionInformationID { get; set; }

        [Required(ErrorMessage = "Please enter the Item!"), MaxLength(500)]

        public string Item { get; set; }

        [Required(ErrorMessage = "Please enter Production Capacity!"), Display(Name = "Production Capacity"), DataType(DataType.Currency)]
        public decimal ProductionCapacity { get; set; }
        
        [Required(ErrorMessage = "Please enter Export Capacity!"), Display(Name = "Export Capacity"), DataType(DataType.Currency)]
        public decimal ExportCapacity { get; set; }
        
        [Required(ErrorMessage = "Please enter Contact Person!"), Display(Name = "Contact Person"), MaxLength(500)]
        public string ContactPerson { get; set; }
        
        [Required, Display(Name = "Phone Number"), RegularExpression(@"^0\d{13}$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNo { get; set; }

        
        [Required(ErrorMessage = "Please enter Email!"), Display(Name = "Email Address ")]
        public string EmailAddress { get; set; }
        public bool IsDeleted { get; set; }



    }
}

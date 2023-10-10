using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RFIApp.ViewModel {
    public class SupplierViewModel {

        public SupplierViewModel() {
            BusinessTypes = new List<SelectListItem>();
            createBankDetails = new List<CreateBankDetailsModel>();
            createContactModels = new List<CreateContactModel>();
            createProductrion = new List<CreateProductrionInformationModel>();
        }

        public bool IsLocked { get; set; }  
        public Guid SupplierID { get; set; }
        
        [Required(ErrorMessage = "Please Enter Legal Name!"), MaxLength(50), Display(Name = "Legal Name")]
        public string LegalName { get; set; }

        [Required(ErrorMessage = "Please Enter Trading Name!"), MaxLength(50), Display(Name = "Trading Name")]
        public string TradingName { get; set; }

        [Required(ErrorMessage = "Please Enter Company Name!"), MaxLength(256), Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Postal Address!"), MaxLength(256), Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [Required(ErrorMessage = "Please Enter  Telephone Number!"), DataType(DataType.PhoneNumber), Display(Name = "Telephone Number"), RegularExpression(@"^0\d{13}$", ErrorMessage = "Not a valid Telephone Number")]
        public string TelephoneNumber { get; set; }

        [DataType(DataType.Url), MaxLength(500), Display(Name = "Web Address")]
        public string WebAddress { get; set; }

        [Required(ErrorMessage = "Post Code is Required"), Display(Name = "Post Code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Post Code")]
        public string PostCode { get; set; }
        [ Display(Name = "Fax Number")]
        public string  FaxNumber { get; set; }

        [Required, Display(Name = "Number of Employees ")]
        public int NoOfEmployees { get; set; }

        [Required,Display(Name = "Employees are subscribed to Social Security")]
        public bool EmployeesAreSubscribedToSocialSecurity { get; set; }
        [Display(Name ="Choose the attachment")]
        [Required]
        public IFormFile FileAttachment { get; set; }
        public string FileAttachmentPath { get; set; }
        public string? FileAttachmentName { get; set; }

        [Required]
        public TypeOfBusiness TypeOfBusiness { get; set; }
        public List<SelectListItem> BusinessTypes { get; set; }
        public List<CreateProductrionInformationModel> createProductrion { get; set; }
        public List<CreateBankDetailsModel> createBankDetails { get; set; }
        public List<CreateContactModel> createContactModels { get; set; }

        public List<SelectListItem> ContactTypeOptions { get; set; }

    }
}

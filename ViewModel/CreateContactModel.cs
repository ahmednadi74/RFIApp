using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RFIApp.ViewModel
{
    public class CreateContactModel
    {
        public Guid ContactID { get; set; }

        [Required]
        public ContactType ContactType { get; set; }

        public IEnumerable<SelectListItem> TypeOfContact { get; set; }

        [Required(ErrorMessage = "Please enter Contact Name!"), MaxLength(50), Display(Name = "Contact Name")]
        public string ContactName { get; set; } = null!;

        [Required, DataType(DataType.PhoneNumber), Display(Name = "Contact Tel No."), RegularExpression(@"^0\d{13}$", ErrorMessage = "Please enter a valid phone number")]
        public string ContactTelNo { get; set; }

        [Required(ErrorMessage = "Please enter Email!"), DataType(DataType.EmailAddress), Display(Name = "Email Address ")]
        public string EmailAddress { get; set; }

        public bool IsDeleted { get; set; }


    }
}

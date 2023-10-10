using System.ComponentModel.DataAnnotations;

namespace RFIApp.ViewModel
{
    public class CreateBankDetailsModel
    {
        public Guid BankDetailsID { get; set; }

        [Required(ErrorMessage = "Please Enter Bank Name!"), MaxLength(50), Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Please Enter BeneficiaryName!"), MaxLength(50), Display(Name = "Beneficiary Name")]
        public string BeneficiaryName { get; set; }

        [Required(ErrorMessage = "Please Enter SwiftBIC!"), Display(Name = "Swift(BIC)")]
        public string SwiftBIC { get; set; }

        [Required(ErrorMessage = "Please Enter BankAccountNo!"), Display(Name = "Bank Account No")]
        public string BankAccountNo { get; set; }

        [Required(ErrorMessage = "Please Enter Bank Address !"), MaxLength(500), Display(Name = "Bank Address")]
        public string BankAddress { get; set; }
        [Required(ErrorMessage = "Please Enter IBAN !")]
        public string IBAN { get; set; }
        [Required(ErrorMessage = "Please Enter Account Currency!"), Display(Name = "Account Currency")]
        public string AccountCurrency { get; set; }

        [Required(ErrorMessage = "Please Enter Payment Terms!"), MaxLength(500), Display(Name = "Payment Term")]
        public string PaymentTerm { get; set; }
        public bool IsDeleted { get; set; }

    }
}

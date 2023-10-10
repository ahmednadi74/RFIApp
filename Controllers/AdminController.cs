using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RFIApp.ViewModel;
using RFIApp.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RFIApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SupplierContext _context;

        public AdminController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager,
                              RoleManager<IdentityRole> roleManager, SupplierContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }
        public ActionResult Index(string? searchString, int page = 1)
        {

            var SupplierView = new SupplierViewModelDashbord
            {
                SupplierPerPage = 10,
                searchString = searchString,
                //Suppliers = _context.Suppliers.Where(s =>( s.LegalName.Contains(searchString) ||searchString==null)).Select(s => new GetAdminDashboard
                //{
                //    LeanID = s.LeanID,
                //    SupplierID = s.SupplierID,
                //    LegalName = s.LegalName,
                //    IsLocked = s.IsLocked,
                //    FileAttachmentName = s.FileAttachmentName ?? "",
                //    FileAttachmentPath = s.FileAttachmentPath ?? ""

                //})
                //   .ToList(),
                Suppliers = _context.Suppliers
                .Where(s => searchString == null || s.LegalName.Contains(searchString)||s.LeanID.ToString().Contains(searchString)||s.SupplierID.ToString().Contains(searchString))
                .Select(s => new GetAdminDashboard
                {
                    LeanID = s.LeanID,
                    SupplierID = s.SupplierID,
                    LegalName = s.LegalName,
                    IsLocked = s.IsLocked,
                    FileAttachmentName = s.FileAttachmentName ?? "",
                    FileAttachmentPath = s.FileAttachmentPath ?? ""
                })
                .ToList(),
                CurrentPage = page
            };
            //if (Request.IsAjaxRequest()) return PartialView(categories);
            //if (Request.IsAjaxRequest()) return PartialView(SupplierView);
          // if (this.HttpContext.Request.IsAjax()) return PartialView(SupplierView);
            return View(SupplierView);
        }
        public IActionResult CreateSupplier()
        {
           return View();
        }
        
        public IActionResult CreateSupplierPost()
        {
            Supplier supplier = new Supplier();
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            
            return Content(supplier.SupplierID.ToString());
        }
        public IActionResult EditSupplier(Guid SupplierID)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.SupplierID == SupplierID);
            if (supplier == null)
            {
                return NotFound();
            }
            var editSupplierDashboard = new EditSupplierDashbord
            {
                LeanID = supplier.LeanID,
                IsLocked = supplier.IsLocked,
                SupplierID=supplier.SupplierID,
                
            };
            return View(editSupplierDashboard);
        }

        [HttpPost]
        public async Task<IActionResult> EditSupplier(EditSupplierDashbord model)
        {
            if (ModelState.IsValid)
            {
                var supplier = _context.Suppliers.FirstOrDefault(s => s.SupplierID == model.SupplierID);
                if (supplier != null)
                { 
                    supplier.IsLocked = model.IsLocked;
                    supplier.LeanID = model.LeanID;
                }
                _context.SaveChanges();
                
              

            }
            //return View(model);
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit( Guid ID)
        {
            var SupplierGuid = _context.Suppliers.Find(ID);
            if (ID != Guid.Empty)
            {
                var viewModel = new SupplierViewModel
                {
                    BusinessTypes = Enum.GetValues(typeof(TypeOfBusiness))
                               .Cast<TypeOfBusiness>()
                               .Select(typeofBusiness => new SelectListItem
                               {
                                   Text = typeofBusiness.GetDescription(),
                                   Value = ((int)typeofBusiness).ToString()
                               }).ToList(),
                };
                var ContactType = Enum.GetValues(typeof(ContactType))
                               .Cast<ContactType>()
                               .Select(ContactType => new SelectListItem
                               {
                                   Text = ContactType.GetDescription(),
                                   Value = ((int)ContactType).ToString()
                               }).ToList();
                //viewModel.createContactModels[0].TypeOfContact = ContactType;
                viewModel.ContactTypeOptions = ContactType;
                viewModel.IsLocked = SupplierGuid.IsLocked;
                var SupplierData = _context.Suppliers.Find(ID);
                if (SupplierData == null)
                {
                    // return RedirectToAction("Edit");
                    return Json(new { Message = "Invalid Url " });
                }
                if (SupplierData.LegalName != null)
                {
                    viewModel.LegalName = SupplierData.LegalName;
                    viewModel.TypeOfBusiness = SupplierData.TypeOfBusiness ?? 0;
                    viewModel.TradingName = SupplierData.TradingName;
                    viewModel.CompanyAddress = SupplierData.CompanyAddress;
                    viewModel.PostalAddress = SupplierData.PostalAddress;
                    viewModel.TelephoneNumber = SupplierData.TelephoneNumber;
                    viewModel.WebAddress = SupplierData.WebAddress;
                    viewModel.PostCode = SupplierData.PostCode;
                    viewModel.FaxNumber = SupplierData.FaxNumber;
                    viewModel.EmployeesAreSubscribedToSocialSecurity = SupplierData.EmployeesAreSubscribedToSocialSecurity;
                    viewModel.NoOfEmployees = SupplierData.NoOfEmployees;
                    viewModel.IsLocked = SupplierData.IsLocked;
                    viewModel.FileAttachmentPath = SupplierData.FileAttachmentPath;
                    viewModel.FileAttachmentName = SupplierData.FileAttachmentName;
                }
                //Bank

                var supplierBankData = _context.BankDetails.Where(b => b.SupplierID == SupplierData.SupplierID).ToList();
                if (supplierBankData.Count() != 0)
                {
                    viewModel.createBankDetails = supplierBankData
                    .Select(b => new CreateBankDetailsModel
                    {
                        BankName = b.BankName,
                        BeneficiaryName = b.BeneficiaryName,
                        SwiftBIC = b.SwiftBIC,
                        BankAccountNo = b.BankAccountNo,
                        BankAddress = b.BankAddress,
                        IBAN = b.IBAN,
                        AccountCurrency = b.AccountCurrency,
                        PaymentTerm = b.PaymentTerm,
                        BankDetailsID = b.BankDetailsID,

                    }).ToList();
                }

                //contact

                var supplierConatctData = _context.Contacts.Where(c => c.SupplierID == SupplierData.SupplierID).ToList();
                if (supplierConatctData.Count() != 0)
                {
                    viewModel.createContactModels = supplierConatctData
                  .Select(c => new CreateContactModel
                  {
                      ContactType = c.ContactType,
                      ContactName = c.ContactName,
                      ContactTelNo = c.ContactTelNo,
                      EmailAddress = c.EmailAddress,
                      ContactID = c.ContactID,
                  }).ToList();
                }
                //production
                var supplierProductrionData = _context.ProductionInformation.Where(p => p.SupplierID == SupplierData.SupplierID).ToList();
                if (supplierProductrionData.Count() != 0)
                {
                    viewModel.createProductrion = supplierProductrionData
                   .Select(p => new CreateProductrionInformationModel
                   {
                       ProductionCapacity = p.ProductionCapacity,
                       ExportCapacity = p.ExportCapacity,
                       Item = p.Item,
                       ProductionInformationID = p.ProductionInformationID,

                   }).ToList();
                }
                viewModel.SupplierID = ID;
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(SupplierViewModel model, IFormFile file)
        {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    // Save the form data to the database
                    var supplier = new Supplier
                    {
                        SupplierID = model.SupplierID,
                        LegalName = model.LegalName,
                        TradingName = model.TradingName,
                        CompanyAddress = model.CompanyAddress,
                        PostalAddress = model.PostalAddress,
                        TelephoneNumber = model.TelephoneNumber,
                        WebAddress = model.WebAddress,
                        PostCode = model.PostCode,
                        FaxNumber = model.FaxNumber,
                        TypeOfBusiness = model.TypeOfBusiness,
                        NoOfEmployees = model.NoOfEmployees,
                        EmployeesAreSubscribedToSocialSecurity = model.EmployeesAreSubscribedToSocialSecurity,
                    };
                    if (model.FileAttachment != null)
                    {

                    }
                    // supplier.Contacts = new List<Contact>();
                    //foreach (var item in model.createContactModels)
                    for (int i = 0; i < model.createContactModels.Count; i++)
                    {
                        var item = model.createContactModels[i];
                        if (item.ContactID == Guid.Empty)
                        {
                            for (int j = i + 1; j < model.createContactModels.Count; j++)
                            {
                                var nextItem = model.createContactModels[j];

                                bool Isduplicated = (item.ContactType == nextItem.ContactType &&
                                                     item.ContactTelNo == nextItem.ContactTelNo &&
                                                     item.ContactName == nextItem.ContactName &&
                                                     item.EmailAddress == nextItem.EmailAddress);

                                if (Isduplicated) { return Json(new { success = false, message = "Data submitted Is Not Valid! Contact Data Submitted duplicated!" }); }
                            }
                        }
                        var existingContact = _context.Contacts.FirstOrDefault(bd => bd.ContactID == item.ContactID);
                        bool IsExisted = _context.Contacts.Where(a => a.SupplierID == model.SupplierID && a.ContactType == item.ContactType
                        && a.ContactName == item.ContactName && a.ContactTelNo == item.ContactTelNo && a.EmailAddress == item.EmailAddress).Any();
                        if (existingContact == null && item.IsDeleted == false && IsExisted == false)
                        {
                            _context.Contacts.Add(new Contact
                            {
                                SupplierID = supplier.SupplierID,
                                ContactType = item.ContactType,
                                ContactName = item.ContactName,
                                ContactTelNo = item.ContactTelNo,
                                EmailAddress = item.EmailAddress,
                            });

                        }
                        else if (existingContact != null && item.IsDeleted == false && IsExisted == false)
                        {
                            existingContact.ContactTelNo = item.ContactTelNo;
                            existingContact.ContactType = item.ContactType;
                            existingContact.ContactName = item.ContactName;
                            existingContact.EmailAddress = item.EmailAddress;
                        }
                        else if (existingContact != null && item.IsDeleted == true)
                        {
                            _context.Contacts.Remove(existingContact);
                        }
                        //else if (existingContact == null && item.IsDeleted == false && IsExisted == true)
                        //{
                        //    return Json(new { success = false, message = "Data submitted Is Not Valid! Contact Data Submitted duplicated!" }); 
                        //}
                    }
                    for (int i = 0; i < model.createBankDetails.Count; i++)
                    {
                        var bitem = model.createBankDetails[i];
                        if (bitem.BankDetailsID == Guid.Empty)
                        {
                            for (int j = i + 1; j < model.createBankDetails.Count; j++)
                            {
                                var nextItem = model.createBankDetails[j];

                                bool Isduplicated = (bitem.BankName == nextItem.BankName &&
                                                     bitem.BankAddress == nextItem.BankAddress &&
                                                     bitem.SwiftBIC == nextItem.SwiftBIC &&
                                                     bitem.PaymentTerm == nextItem.PaymentTerm &&
                                                     bitem.BeneficiaryName == nextItem.BeneficiaryName &&
                                                     bitem.BankAccountNo == nextItem.BankAccountNo &&
                                                     bitem.AccountCurrency == nextItem.AccountCurrency &&
                                                     bitem.IBAN == nextItem.IBAN
                                                     );

                                if (Isduplicated) { return Json(new { success = false, message = "Data submitted Is Not Valid! Bank  Data Submitted duplicated!" }); }
                            }
                        }
                        var existingBankDetails = _context.BankDetails.FirstOrDefault(bd => bd.BankDetailsID == bitem.BankDetailsID);
                        bool IsExisted = _context.BankDetails.Where(a => a.SupplierID == model.SupplierID && a.BankName == bitem.BankName
                        && a.BeneficiaryName == bitem.BeneficiaryName && a.SwiftBIC == bitem.SwiftBIC && a.BankAccountNo == bitem.BankAccountNo
                        && a.BankAddress == bitem.BankAddress && a.IBAN == bitem.IBAN && a.AccountCurrency == bitem.AccountCurrency &&
                        a.PaymentTerm == bitem.PaymentTerm).Any();

                        if (existingBankDetails == null && bitem.IsDeleted == false && IsExisted == false)
                        {
                            _context.BankDetails.Add(new BankDetails
                            {

                                SupplierID = supplier.SupplierID,
                                BankName = bitem.BankName,
                                BeneficiaryName = bitem.BeneficiaryName,
                                SwiftBIC = bitem.SwiftBIC,
                                BankAccountNo = bitem.BankAccountNo,
                                BankAddress = bitem.BankAddress,
                                IBAN = bitem.IBAN,
                                AccountCurrency = bitem.AccountCurrency,
                                PaymentTerm = bitem.PaymentTerm
                            });
                        }
                        else if (existingBankDetails != null && bitem.IsDeleted == false && IsExisted == false)
                        {
                            existingBankDetails.BankName = bitem.BankName;
                            existingBankDetails.BeneficiaryName = bitem.BeneficiaryName;
                            existingBankDetails.SwiftBIC = bitem.SwiftBIC;
                            existingBankDetails.BankAccountNo = bitem.BankAccountNo;
                            existingBankDetails.BankAddress = bitem.BankAddress;
                            existingBankDetails.IBAN = bitem.IBAN;
                            existingBankDetails.AccountCurrency = bitem.AccountCurrency;
                            existingBankDetails.PaymentTerm = bitem.PaymentTerm;

                        }
                        else if (existingBankDetails != null && bitem.IsDeleted == true)
                        {
                            _context.BankDetails.Remove(existingBankDetails);

                        }

                    }
                    for (int i = 0; i < model.createProductrion.Count; i++)
                    {
                        var Proitem = model.createProductrion[i];
                        if (Proitem.ProductionInformationID == Guid.Empty)
                        {
                            for (int j = i + 1; j < model.createProductrion.Count; j++)
                            {
                                var nextItem = model.createProductrion[j];

                                bool Isduplicated = (Proitem.Item == nextItem.Item &&
                                                     Proitem.ExportCapacity == nextItem.ExportCapacity &&
                                                     Proitem.ProductionCapacity == nextItem.ProductionCapacity

                                                     );

                                if (Isduplicated) { return Json(new { success = false, message = "Data submitted Is Not Valid! Production  Data Submitted duplicated!" }); }
                            }
                        }
                        var existingProducationInformation = _context.ProductionInformation.FirstOrDefault(p => p.ProductionInformationID == Proitem.ProductionInformationID);
                        bool IsExisted = _context.ProductionInformation.Where(p => p.SupplierID == model.SupplierID && p.Item == Proitem.Item
                      && p.ProductionCapacity == Proitem.ProductionCapacity && p.ExportCapacity == Proitem.ExportCapacity)
                      .Any();
                        if (existingProducationInformation == null && Proitem.IsDeleted == false && IsExisted == false)
                        {
                            _context.ProductionInformation.Add(new ProductionInformation
                            {
                                SupplierID = supplier.SupplierID,
                                Item = Proitem.Item,
                                ProductionCapacity = Proitem.ProductionCapacity,
                                ExportCapacity = Proitem.ExportCapacity,
                                ContactPerson = Proitem.ContactPerson,
                                PhoneNo = Proitem.PhoneNo,
                                EmailAddress = Proitem.EmailAddress,
                            }
                            );

                        }
                        else if (existingProducationInformation != null && Proitem.IsDeleted == false && IsExisted == false)
                        {
                            existingProducationInformation.Item = Proitem.Item;
                            existingProducationInformation.ProductionCapacity = Proitem.ProductionCapacity;
                            existingProducationInformation.ExportCapacity = Proitem.ExportCapacity;
                            existingProducationInformation.PhoneNo = Proitem.PhoneNo;
                            existingProducationInformation.ContactPerson = Proitem.ContactPerson;
                            existingProducationInformation.EmailAddress = Proitem.EmailAddress;
                        }
                        else if (existingProducationInformation != null && Proitem.IsDeleted == true)
                        {
                            _context.ProductionInformation.Remove(existingProducationInformation);

                        }

                    }
                    _context.Suppliers.Update(supplier);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                return Redirect(Url.Action("Edit", "Admin") + "?id=" + model.SupplierID);

            }
      


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var currentUser = await _userManager.FindByEmailAsync(user.UserName);
                    await _userManager.AddToRoleAsync(user, "Administration");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Admin");
        }

    }
}

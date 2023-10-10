using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RFIApp.Models;
using RFIApp.ViewModel;
using System;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Net.Mail;
using RFIApp.Helper;
using Microsoft.Extensions.Options;

namespace RFIApp.Controllers {
    public class SupplierController : Controller {
        private readonly SupplierContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IOptions<AttachmentSettings> attachmentOptions;
        private readonly AttachmentSettings attachmentSettings;
        public SupplierController(SupplierContext context, IWebHostEnvironment hostingEnvironment, IOptions<AttachmentSettings> Attachmentoptions, AttachmentSettings attachmentSettings) {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            attachmentOptions = Attachmentoptions;
            this.attachmentSettings = attachmentSettings;
        }
        public IActionResult Edit([FromQuery] Guid ID) {
            var SupplierGuid = _context.Suppliers.Find(ID);
                if (ID != Guid.Empty) {
                var viewModel = new SupplierViewModel {
                    BusinessTypes = Enum.GetValues(typeof(TypeOfBusiness))
                               .Cast<TypeOfBusiness>()
                               .Select(typeofBusiness => new SelectListItem {
                                   Text = typeofBusiness.GetDescription(),
                                   Value = ((int)typeofBusiness).ToString()
                               }).ToList(),
                };
                var ContactType = Enum.GetValues(typeof(ContactType))
                               .Cast<ContactType>()
                               .Select(ContactType => new SelectListItem {
                                   Text = ContactType.GetDescription(),
                                   Value = ((int)ContactType).ToString()
                               }).ToList();
                //viewModel.createContactModels[0].TypeOfContact = ContactType;
                viewModel.ContactTypeOptions = ContactType;
                viewModel.IsLocked = SupplierGuid.IsLocked;
                var SupplierData = _context.Suppliers.Find(ID);
                if (SupplierData == null) {
                    // return RedirectToAction("Edit");
                    return Json(new { Message = "Invalid Url " });
                }
                if (SupplierData.LegalName != null) {
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
                if (supplierBankData.Count() != 0) {
                    viewModel.createBankDetails = supplierBankData
                    .Select(b => new CreateBankDetailsModel {
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
                if (supplierConatctData.Count() != 0) {
                    viewModel.createContactModels = supplierConatctData
                  .Select(c => new CreateContactModel {
                      ContactType = c.ContactType,
                      ContactName = c.ContactName,
                      ContactTelNo = c.ContactTelNo,
                      EmailAddress = c.EmailAddress,
                      ContactID = c.ContactID,
                  }).ToList();
                }
                //production
                var supplierProductrionData = _context.ProductionInformation.Where(p => p.SupplierID == SupplierData.SupplierID).ToList();
                if (supplierProductrionData.Count() != 0) {
                    viewModel.createProductrion = supplierProductrionData
                   .Select(p => new CreateProductrionInformationModel {
                       ProductionCapacity = p.ProductionCapacity,
                       ExportCapacity = p.ExportCapacity,
                       Item = p.Item,
                       ProductionInformationID = p.ProductionInformationID,

                   }).ToList();
                }
                viewModel.SupplierID = ID;
                return View(viewModel);
            }
            else {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierViewModel model, IFormFile file) {
            //var supplier = _context.Suppliers.Where(s=>s.SupplierID == model.SupplierID).FirstOrDefault();
            //bool IsLocked = supplier.IsLocked;
            if (model.IsLocked != true) {
                using var transaction = _context.Database.BeginTransaction();
                try {
                    // Save the form data to the database
                    var supplier = new Supplier {
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
                    if (model.FileAttachment != null) {

                    }
                    // supplier.Contacts = new List<Contact>();
                    //foreach (var item in model.createContactModels)
                    for (int i = 0; i < model.createContactModels.Count; i++) {
                        var item = model.createContactModels[i];
                        if (item.ContactID == Guid.Empty) {
                            for (int j = i + 1; j < model.createContactModels.Count; j++) {
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
                        if (existingContact == null && item.IsDeleted == false && IsExisted == false) {
                            _context.Contacts.Add(new Contact {
                                SupplierID = supplier.SupplierID,
                                ContactType = item.ContactType,
                                ContactName = item.ContactName,
                                ContactTelNo = item.ContactTelNo,
                                EmailAddress = item.EmailAddress,
                            });

                        }
                        else if (existingContact != null && item.IsDeleted == false && IsExisted == false) {
                            existingContact.ContactTelNo = item.ContactTelNo;
                            existingContact.ContactType = item.ContactType;
                            existingContact.ContactName = item.ContactName;
                            existingContact.EmailAddress = item.EmailAddress;
                        }
                        else if (existingContact != null && item.IsDeleted == true) {
                            _context.Contacts.Remove(existingContact);
                        }
                        //else if (existingContact == null && item.IsDeleted == false && IsExisted == true)
                        //{
                        //    return Json(new { success = false, message = "Data submitted Is Not Valid! Contact Data Submitted duplicated!" }); 
                        //}
                    }
                    for (int i = 0; i < model.createBankDetails.Count; i++) {
                        var bitem = model.createBankDetails[i];
                        if (bitem.BankDetailsID == Guid.Empty) {
                            for (int j = i + 1; j < model.createBankDetails.Count; j++) {
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

                        if (existingBankDetails == null && bitem.IsDeleted == false && IsExisted == false) {
                            _context.BankDetails.Add(new BankDetails {

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
                        else if (existingBankDetails != null && bitem.IsDeleted == false && IsExisted == false) {
                            existingBankDetails.BankName = bitem.BankName;
                            existingBankDetails.BeneficiaryName = bitem.BeneficiaryName;
                            existingBankDetails.SwiftBIC = bitem.SwiftBIC;
                            existingBankDetails.BankAccountNo = bitem.BankAccountNo;
                            existingBankDetails.BankAddress = bitem.BankAddress;
                            existingBankDetails.IBAN = bitem.IBAN;
                            existingBankDetails.AccountCurrency = bitem.AccountCurrency;
                            existingBankDetails.PaymentTerm = bitem.PaymentTerm;

                        }
                        else if (existingBankDetails != null && bitem.IsDeleted == true) {
                            _context.BankDetails.Remove(existingBankDetails);

                        }

                    }
                    for (int i = 0; i < model.createProductrion.Count; i++) {
                        var Proitem = model.createProductrion[i];
                        if (Proitem.ProductionInformationID == Guid.Empty) {
                            for (int j = i + 1; j < model.createProductrion.Count; j++) {
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
                        if (existingProducationInformation == null && Proitem.IsDeleted == false && IsExisted == false) {
                            _context.ProductionInformation.Add(new ProductionInformation {
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
                        else if (existingProducationInformation != null && Proitem.IsDeleted == false && IsExisted == false) {
                            existingProducationInformation.Item = Proitem.Item;
                            existingProducationInformation.ProductionCapacity = Proitem.ProductionCapacity;
                            existingProducationInformation.ExportCapacity = Proitem.ExportCapacity;
                            existingProducationInformation.PhoneNo = Proitem.PhoneNo;
                            existingProducationInformation.ContactPerson = Proitem.ContactPerson;
                            existingProducationInformation.EmailAddress = Proitem.EmailAddress;
                        }
                        else if (existingProducationInformation != null && Proitem.IsDeleted == true) {
                            _context.ProductionInformation.Remove(existingProducationInformation);

                        }

                    }
                    _context.Suppliers.Update(supplier);
                    _context.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true });
                }
                catch (Exception ex) {
                    transaction.Rollback();
                }
                return Redirect(Url.Action("Edit", "Supplier") + "?id=" + model.SupplierID);

            }
            else {
                return Json(new { Message = "You can not edit your data please contact the Administrator" });
                // return View(model); }
            }
        }

        public async Task<IActionResult> CreateAttachment(Guid ID) {
            ViewBag.SupplierID = ID;
            var viewModel = new CreateAttachmentModel();
            viewModel.SupplierID = ID;
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAttachment(CreateAttachmentModel model) {
            if (ModelState.IsValid) {
                var supplier = _context.Suppliers.Find(model.SupplierID);
                if (supplier != null) {
                    HelperMethods helper = new HelperMethods(attachmentOptions, attachmentSettings);

                    //string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Attachment");
                    if (model.FileAttachmentPath != null) {
                        TempData["SupplierID"] = model.SupplierID;


                        ValidateImgResponse vres = helper.ValidateGeneralFile(model.FileAttachmentPath, "wwwroot\\Attachment");
                        if (vres.Success) {
                            string attachmentFullPath = Path.Combine(Directory.GetCurrentDirectory(), vres.Result);

                            await helper.SaveIFormFile(model.FileAttachmentPath, attachmentFullPath);


                            supplier.FileAttachmentPath = vres.Result;
                            supplier.FileAttachmentName = model.FileAttachmentPath.FileName;
                            _context.SaveChanges();

                        }
                        else
                        // throw new ValidationException(vres.Result);
                        { return Json(new { success = false, message = vres.Result }); }
                        //if (model.FileAttachmentPath.Length > 0)
                        //{
                        //    FileInfo imgFileInfo = new FileInfo(model.FileAttachmentPath.FileName);

                        //    string attachmentName = Guid.NewGuid().ToString()+ imgFileInfo.Extension;
                        //    string attachmentPath = Path.Combine("wwwroot", "Attachment", attachmentName);

                        //   // string filePath = Path.Combine(uploads, model.FileAttachmentPath.FileName);
                        //    using (Stream fileStream = new FileStream(attachmentPath, FileMode.Create))
                        //    {
                        //        await model.FileAttachmentPath.CopyToAsync(fileStream);
                        //    }
                        //    supplier.FileAttachmentPath = attachmentPath;
                        //    supplier.FileAttachmentName = model.FileAttachmentPath.FileName;

                        //}

                    }

                }
                return Redirect(Url.Action("AttachmentsIndex", "Supplier") + "?id=" + model.SupplierID);

            }


            return View(model);

        }
        public IActionResult AttachmentsIndex(Guid id) {
            CreateAttachmentModel createAttachmentModel = new CreateAttachmentModel();
            createAttachmentModel.SupplierID = id;


            return View(createAttachmentModel);
        }

        public async Task<IActionResult> Download(string filePath, string fileName) {
            if (filePath == null)
                return Content("filename not present");


            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open)) {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), fileName);
        }
        private string GetContentType(string path) {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes() {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }

}

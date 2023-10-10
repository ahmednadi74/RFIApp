using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace RFIApp.Helper
{
    public class ValidateImgResponse
        {
            public string Result { set; get; }
            public bool Success { set; get; }
        }
    public class HelperMethods
    {
        private readonly AttachmentSettings attachmentSettings;
        private readonly long _maxAttachmentSize;
        private readonly List<string> _allowedExtensions;

        public HelperMethods(IOptions<AttachmentSettings> Attachmentoptions, AttachmentSettings attachmentSettings)
        {
            this.attachmentSettings = attachmentSettings;
            this._maxAttachmentSize = Attachmentoptions.Value.MaxAttachmentSize;
            this._allowedExtensions = Attachmentoptions.Value.AllowedExtensions;
        }
        public ValidateImgResponse ValidateGeneralFile(IFormFile file, string path)
        {
            //attachmentSettings= appsettingattachmentSettings.GetSection
            ValidateImgResponse response = new ValidateImgResponse();
            FileInfo imgFileInfo = new FileInfo(file.FileName);

            if (!ValidateFileExtension(file))
            {
                response.Success = false;
                response.Result = imgFileInfo.Extension + "file is not allowed";
                return response;

            }
            if (!ValidateFileSize(file))
            {
                response.Success = false;
                response.Result = "file has exceeded max size";
                return response;

            }


            string attachmentName = Guid.NewGuid().ToString() + imgFileInfo.Extension;
            string attachmentPath = Path.Combine("wwwroot", "Attachment", attachmentName);


            response.Result = attachmentPath;
            response.Success = true;
            return response;
        }
        public bool ValidateFileExtension(IFormFile formFile)
        {
            FileInfo fileInfo = new FileInfo(formFile.FileName);

            List<string> AllowedExtensions = _allowedExtensions;

            if (!AllowedExtensions.Contains(fileInfo.Extension))
            {
                return false;
            }
            return true;
        }
        public bool ValidateFileSize(IFormFile formFile)
        {
            
            if (formFile.Length > _maxAttachmentSize)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task SaveIFormFile(IFormFile formFile, string fullPath)
        {
            if (formFile.Length > 0)
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    await formFile.CopyToAsync(fs);
                }

            }
        }


    }

}

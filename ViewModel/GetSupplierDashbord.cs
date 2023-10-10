using System.Reflection.Metadata;

namespace RFIApp.ViewModel
{
    public class GetAdminDashboard
    {
        public Guid SupplierID { get; set; }
        public int LeanID { get; set; }
        public string? LegalName { get; set; }
        public bool IsLocked { get; set; }
        public string ?FileAttachmentPath { get; set; }
        public string? FileAttachmentName { get; set; }

    }
    public class SupplierViewModelDashbord
    {
        public IEnumerable <GetAdminDashboard> Suppliers { get; set; }
        public int SupplierPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string? searchString { get; set; }
        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Suppliers.Count() / (double)SupplierPerPage));
        }
        public IEnumerable<GetAdminDashboard> PaginatedSuppliers()
        {
            int start = (CurrentPage - 1) * SupplierPerPage;
            return Suppliers.OrderBy(b => b.SupplierID).Skip(start).Take(SupplierPerPage);
        }
    }
}


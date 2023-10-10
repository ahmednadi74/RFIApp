using System.ComponentModel;
using System.Reflection;

namespace RFIApp.Helper
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute =
                Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
            //https://github.com/TahirNaushad/Fiver.Mvc.FileUpload/tree/master
            //https://www.c-sharpcorner.com/article/upload-download-files-in-asp-net-core-2-0/
        }
    }
}

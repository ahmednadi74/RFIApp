namespace RFIApp.Helper
{
    public  static class AjaxHelper
    {
        public static bool IsAjax(this HttpRequest? request, string httpVerb = "")
        {
            if (request == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(httpVerb))
            {
                if (request.Method.ToLower() != httpVerb.ToLower())
                {
                    return false;
                }
            }

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

    }
}

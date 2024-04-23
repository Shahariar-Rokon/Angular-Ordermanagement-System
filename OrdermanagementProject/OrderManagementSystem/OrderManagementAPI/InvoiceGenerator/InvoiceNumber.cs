namespace OrderManagementAPI.InvoiceGenerator
{
    public static class InvoiceNumber
    {
        public static string Get()
        {
            return DateTime.Now.ToString("yyMMddhhmmssffff");
        }
    }
}

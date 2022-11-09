namespace Smartwyre.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        public bool Success { get; set; }
        
        public Account Result { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}

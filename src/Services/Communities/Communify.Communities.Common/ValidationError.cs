namespace Communify.Communities.Common
{
    public class ValidationError
    {
        public ValidationError()
        {
            
        }
        
        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
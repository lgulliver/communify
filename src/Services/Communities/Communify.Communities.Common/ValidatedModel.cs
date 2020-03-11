using System.Collections.Generic;

namespace Communify.Communities.Common
{
    public class ValidatedModel<T>
    {
        public ValidatedModel()
        {
            ValidationFailures = new List<ValidationError>();
        }
        public T Value { get; set; }

        public bool IsValid { get; set; }

 
        public IList<ValidationError> ValidationFailures { get; set; }
    }
}

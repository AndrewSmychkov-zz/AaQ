using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DomainModels.Validators
{
    internal abstract class ValidatorBase<T> : IValidatorObject<T>
    { 
        readonly protected IDictionary<string, string[]> dic = new Dictionary<string, string[]>();

        #region implement IvalidateObject
        public IDictionary<string, IEnumerable<string>> ValidateObject(T context)
        {
            return ValidatePropertys(context, context.GetType().GetRuntimeProperties().Where(x => x.CanRead).Select(x => x.Name));
        }

        public IEnumerable<string> ValidateProperty(T context, string propertyName)
        {
            IList<string> errors = new List<string>();
            var val = context.GetType().GetRuntimeProperty(propertyName);
            if (val != null)
                CheckProperty(context, errors, val);
            return errors;
        }      

        public IDictionary<string, IEnumerable<string>> ValidatePropertys(T context, IEnumerable<string> propertyNames)
        {
            IDictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();
            foreach (var property in propertyNames.Where(x => dic.Keys.Contains(x)))
            {
                var messages = ValidateProperty(context, property);
                if (messages.Any())
                    errors.Add(property, messages);
            }
            return errors;
        }

        public bool IsValidObject(T context)
        {
            if (context == null)
                return false;
            return ValidateObject(context).Count == 0;
        }
        #endregion

        #region implement IValidatorPrimitives
        public IEnumerable<string> ValidateProperty(object value, string propertyName)
        {
            IList<string> errors = new List<string>();
            CheckProperty(value, errors, propertyName);
            return errors;
        }

        public string[] GetErrorStringFor(string propertyName)
        {
            var err = dic[propertyName];
            if (err == null)
                return new string[0];
            return err;
        }
        #endregion
               
        protected virtual void CheckProperty(T context, IList<string> errors, PropertyInfo val)
        {
            CheckProperty(val.GetValue(context), errors, val.Name);
        }

        protected abstract void CheckProperty(object value, IList<string> errors, string propertyName);

        protected bool ValidateString(string value, string pattern, bool required)
        {
            if (string.IsNullOrWhiteSpace(value))
                return !required;
            var reg = new Regex(pattern);
            return reg.IsMatch(value);
        }
    }
}

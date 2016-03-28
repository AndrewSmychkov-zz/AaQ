using DomainModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Validate
{
    public class ValidateByPrimitivesAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly IValidatorPrimitives _prim;
        private readonly string _nameofproperty;
        private readonly string _interface;

        public ValidateByPrimitivesAttribute(string nameofinterface, string nameofproperty)
        {
            _nameofproperty = nameofproperty;
            _interface = nameofinterface;          
            _prim = ValidatorFactory.GetValidatorByInterfaceName(_interface);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
          ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage =
                  _prim == null
                    ? FormatErrorMessage(metadata.GetDisplayName())
                    : CreateStringError(_prim.GetErrorStringFor(_nameofproperty)),
                ValidationType = "validatebyprimitives"
            };

            rule.ValidationParameters.Add("nameofproperty", _nameofproperty);
            rule.ValidationParameters.Add("nameofinterface", _interface);

            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_prim == null)
                return ValidationResult.Success;
          
            var error = _prim.ValidateProperty(value, _nameofproperty).ToList();

            return error.Any()
              ? new ValidationResult(CreateStringError(error), new[] { validationContext.MemberName })
              : ValidationResult.Success;
        }

        private string CreateStringError(IEnumerable<string> error)
        {
            var text = new StringBuilder(string.Empty);
            foreach (var e in error)
                text.AppendLine(e);
            return text.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    /// <summary>
    /// Class for handling standard Request Object Validations
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Validates that a required string value is present on a request
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatRequiredValueIsPresent(string value, string name, List<ValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"{name} is a required value.", memberNames: new string[] { name }));
            }

            return validationResults;
        }

        /// <summary>
        /// Validates that a required string value is of Guid Type
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatRequiredValueIsGuid(string value, string name, List<ValidationResult> validationResults)
        {
            if (!Guid.TryParse(value, out Guid result))
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"{name} is a not Guid Type .", memberNames: new string[] { name }));
            }

            return validationResults;
        }

        /// <summary>
        /// Validates that a required object is present on a request
        /// </summary>
        /// <param name="value">Object to check</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatRequiredObjectIsPresent(object value, string name, List<ValidationResult> validationResults)
        {
            if (value == null)
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"{name} is a required object.", memberNames: new string[] { name }));
            }

            return validationResults;
        }

        /// <summary>
        /// Validates that a required object is present on a request
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="value">value</param>
        /// <param name="name">name</param>
        /// <param name="validationResults">validation results</param>
        /// <returns>validations results list</returns>
        public static List<ValidationResult> ThatListIsNotEmpty<T>(IEnumerable<T> value, string name, List<ValidationResult> validationResults)
        {
            if (value != null)
            {
                bool nonEmpty = false;
                foreach (var item in value)
                {
                    nonEmpty = true;
                }

                if (nonEmpty == false)
                {
                    if (validationResults == null)
                    {
                        validationResults = new List<ValidationResult>();
                    }

                    validationResults.Add(new ValidationResult($"{name} cannot be an empty list.", memberNames: new string[] { name }));
                }
            }

            return validationResults;
        }

        /// <summary>
        /// Validates a maximum string length isn't exceeded
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="length">Maximum length to compare</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatMaxLengthRequirementIsMet(string value, int length, string name, List<ValidationResult> validationResults)
        {
            if (value?.Length > length)
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"{name} has a maximum allowed length of {length} characters.", memberNames: new string[] { nameof(name) }));
            }

            return validationResults;
        }

        /// <summary>
        /// Validates a maximum  and minimum string length isn't exceeded
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="minlength">min length</param>
        /// <param name="maxlength">max length</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatMaxAndMinLengthRequirementIsMet(string value, int minlength, int maxlength, string name, List<ValidationResult> validationResults)
        {
            ThatMinLengthRequirementIsMet(value, minlength, name, validationResults);
            ThatMaxLengthRequirementIsMet(value, maxlength, name, validationResults);
            return validationResults;
        }

        /// <summary>
        /// Validates a minimum string length.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="length">Minimum length to compare</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatMinLengthRequirementIsMet(string value, int length, string name, List<ValidationResult> validationResults)
        {
            if (value?.Length < length)
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"{name} has a minimum required length of {length} characters.", memberNames: new string[] { nameof(name) }));
            }

            return validationResults;
        }

        /// <summary>
        /// Checks a list of resources for uniqueness.
        /// </summary>
        /// <typeparam name="T">Type of resource to compare</typeparam>
        /// <param name="values">A list of resources.</param>
        /// <param name="name">Member name</param>
        /// <param name="validationResults">A list of validation results to add to and return</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatListValuesAreUnique<T>(IReadOnlyList<T> values, string name, List<ValidationResult> validationResults)
        {
            Dictionary<T, string> tmp = new Dictionary<T, string>();
            if (values == null)
            {
                return validationResults;
            }

            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                if (tmp.TryGetValue(value, out string dummy) == true)
                {
                    if (validationResults == null)
                    {
                        validationResults = new List<ValidationResult>();
                    }

                    validationResults.Add(new ValidationResult($"{name} must be a unique list of resources.", memberNames: new string[] { nameof(name) }));
                    break;
                }

                tmp.Add(value, value.ToString());
            }

            return validationResults;
        }

        /// <summary>
        /// Validates that a string matches a given content and format
        /// </summary>
        /// <param name="value">String to validate.</param>
        /// <param name="name">Name of the object property that the string represents</param>
        /// <param name="regex">A regular expression object that expresses the desired string format and content</param>
        /// <param name="message">The message that describes the content validation issue if the string does not match the regex</param>
        /// <param name="validationResults">A list of validation results to populate with any validation issues.</param>
        /// <returns>An appended list of validation errors.</returns>
        public static List<ValidationResult> ThatStringAdheresToFormat(string value, string name, Regex regex, string message, List<ValidationResult> validationResults)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!regex.IsMatch(value))
                {
                    if (validationResults == null)
                    {
                        validationResults = new List<ValidationResult>();
                    }

                    validationResults.Add(new ValidationResult(errorMessage: $"String [{value}] contains invalid characters. {message}", memberNames: new List<string> { name }));
                }
            }

            return validationResults;
        }

        /// <summary>
        /// Helper funcition that safely constructs a domain object and traps any exceptions it throws, turning them into
        /// a list of validation results.
        /// </summary>
        /// <remarks>This allows us to leverage the validation code in the domain objects without having to re-write it externally somewhere else.</remarks>
        /// <typeparam name="TDomain">Type of the Domain Object</typeparam>
        /// <param name="func">A function delegate used to construct the domain object</param>
        /// <param name="name">The name of the parameter being validated.</param>
        /// <param name="domain">An output parameter containing the constructed domain object if successful.</param>
        /// <returns>A list of validation errors.</returns>
        public static List<ValidationResult> ThatDomainObjectCanConstructSafely<TDomain>(Func<TDomain> func, string name, out TDomain domain)
            where TDomain : class
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            domain = null;
            try
            {
                domain = func.Invoke();
            }
            catch (Exception ex)
            {
                errors.Add(new ValidationResult(errorMessage: ex.Message, memberNames: new List<string> { name }));
            }

            return errors;
        }

        /// <summary>
        /// Validate the supplied value to ensure that it is defined in the specified enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of enum to validate.</typeparam>
        /// <typeparam name="TValue">The type of value to validate (must match enum underlying type).</typeparam>
        /// <param name="value">The integer to validate</param>
        /// <param name="name">The name of the parameter being validated.</param>
        /// <param name="validationResults">A list of validation results to populate with any validation issues.</param>
        /// <returns>A list of validation errors.</returns>
        public static List<ValidationResult> ThatRequiredValueIsInEnum<TEnum, TValue>(object value, string name, List<ValidationResult> validationResults)
            where TEnum : Enum
            where TValue : struct
        {
            if (value is TValue x)
            {
                var type = typeof(TEnum);

                if (Enum.IsDefined(type, x) == false)
                {
                    if (validationResults == null)
                    {
                        validationResults = new List<ValidationResult>();
                    }

                    validationResults.Add(new ValidationResult(errorMessage: $"{name} is not a valid value.", new List<string> { name }));
                }
            }
            else
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult(errorMessage: $"{name} is a required value.", new List<string> { name }));
            }

            return validationResults;
        }

        /// <summary>
        /// Verifies whether given string is a Guid
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="name">name</param>
        /// <param name="validationResults">validation results</param>
        /// <returns>Validation result list</returns>
        public static List<ValidationResult> ThatStringIsAGuid(string value, string name, List<ValidationResult> validationResults)
        {
            if (Guid.TryParse(value, out Guid result) == false)
            {
                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"Value must represent a valid GUID", new List<string>() { name }));
            }

            return validationResults;
        }

        /// <summary>
        /// Formats an exception as a list of validation results
        /// </summary>
        /// <param name="ex">Exception to format</param>
        /// <returns>A list of validation results</returns>
        public static List<ValidationResult> ToValidationResults(this Exception ex)
        {
            return new List<ValidationResult> { new ValidationResult(ex.Message) };
        }

        /// <summary>
        /// Validates that a required object is within a specified min and max range.
        /// </summary>
        /// <typeparam name="T">A comparable value type</typeparam>
        /// <param name="value">The value to test</param>
        /// <param name="minValue">Minimum acceptable value</param>
        /// <param name="maxValue">Maximum acceptable value</param>
        /// <param name="name">The name of the property being compared</param>
        /// <param name="validationResults">A list of validation results</param>
        /// <returns>ValidationResult results</returns>
        public static List<ValidationResult> ThatValueTypeIsInRange<T>(T value, T minValue, T maxValue, string name, List<ValidationResult> validationResults)
            where T : struct, IComparable
        {
            if (value.CompareTo(minValue) == -1 || value.CompareTo(maxValue) == 1)
            {
                validationResults = validationResults ?? new List<ValidationResult>();

                if (validationResults == null)
                {
                    validationResults = new List<ValidationResult>();
                }

                validationResults.Add(new ValidationResult($"{name} must be greater than or equal to {minValue} and less than or equal to {maxValue}", memberNames: new string[] { name }));
            }

            return validationResults;
        }
    }
}

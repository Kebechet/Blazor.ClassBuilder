using Blazor.ClassBuilder.Extensions;
using IsNullOrEmpty.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.ClassBuilder
{
    /// <summary>
    /// A builder for HTML attributes to be used in Blazor components.
    /// </summary>
    public class AttributeBuilder
    {
        private Dictionary<string, object> _attributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Adds the attribute. E.g. Add("disabled") or Add("placeholder", "Enter your name")
        /// </summary>
        public AttributeBuilder Add(string parameterName, object? parameterValue = null)
        {
            return AddIf(true, parameterName, parameterValue);
        }

        /// <summary>
        /// Adds the attributes from another AttributeBuilder instance
        /// </summary>
        public AttributeBuilder Add(AttributeBuilder? attributeBuilder)
        {
            if (attributeBuilder.IsNullOrEmpty())
            {
                return this;
            }

            _attributes = _attributes
                .Union(attributeBuilder!.Build())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return this;
        }

        /// <summary>
        /// Adds the attribute if the condition is true. E.g. AddIf(isDisabled, "disabled")
        /// </summary>
        public AttributeBuilder AddIf(bool canAdd, string parameterName)
        {
            return AddIf(canAdd, parameterName, null);
        }

        /// <summary>
        /// Adds the attribute if the condition is true. E.g. AddIf(isDisabled, "disabled", "disabled")
        /// </summary>
        public AttributeBuilder AddIf(bool canAdd, string parameterName, object? parameterValue)
        {
            if (!canAdd)
            {
                return this;
            }

            _attributes.Add(parameterName, parameterValue?.ToString() ?? string.Empty);

            return this;
        }

        /// <summary>
        /// Adds the attribute if the parameterValue is not null or empty. E.g. AddIfFilled("placeholder", Placeholder)
        /// </summary>
        public AttributeBuilder AddIfFilled(string parameterName, object? parameterValue)
        {
            var parameterValueString = parameterValue?.ToString() ?? null;

            return AddIf(!parameterValueString.IsNullOrEmpty(), parameterName, parameterValue);
        }

        /// <summary>
        /// Throws an ArgumentException if the condition is true. E.g. Throw(isDisabled, "The component cannot be disabled")
        /// </summary>
        public AttributeBuilder Throw(bool canThrow, string exceptionText)
        {
            if (!canThrow)
            {
                return this;
            }

            throw new ArgumentException(exceptionText);
        }

        /// <summary>
        /// Builds the attributes dictionary
        /// </summary>
        public Dictionary<string, object> Build()
        {
            return _attributes;
        }
    }
}

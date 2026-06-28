using IsNullOrEmpty.Extensions;
using System;
using System.Collections.Generic;

namespace Blazor.ClassBuilder
{
    /// <summary>
    /// A builder for HTML attributes to be used in Blazor components.
    /// </summary>
    public class AttributeBuilder
    {
        private readonly Dictionary<string, object> _attributes = new Dictionary<string, object>();

        /// <summary>
        /// Adds the attribute. E.g. Add("disabled") or Add("placeholder", "Enter your name")
        /// </summary>
        public AttributeBuilder Add(string parameterName, object? parameterValue = null)
        {
            return AddIf(true, parameterName, parameterValue);
        }

        /// <summary>
        /// Adds the attributes from another AttributeBuilder instance. Values from the merged-in
        /// builder overwrite existing ones with the same name (last-wins).
        /// </summary>
        public AttributeBuilder Add(AttributeBuilder? attributeBuilder)
        {
            if ((attributeBuilder?._attributes).IsNullOrEmpty() || ReferenceEquals(attributeBuilder, this))
            {
                return this;
            }

            foreach (var attribute in attributeBuilder!._attributes)
            {
                _attributes[attribute.Key] = attribute.Value;
            }

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
        /// Adds the attribute if the condition is true. E.g. AddIf(isDisabled, "disabled", "disabled").
        /// The value is stored as-is so Blazor renders bools and numbers with the correct attribute
        /// semantics. An existing attribute with the same name is overwritten (last-wins).
        /// </summary>
        public AttributeBuilder AddIf(bool canAdd, string parameterName, object? parameterValue)
        {
            if (!canAdd)
            {
                return this;
            }

            _attributes[parameterName] = parameterValue ?? string.Empty;

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
        /// Adds an attribute with a lazy-evaluated value factory if the condition is true.
        /// The factory is only invoked if the condition is true.
        /// </summary>
        public AttributeBuilder Add(bool canAdd, string parameterName, Func<object?> valueFactory)
        {
            if (!canAdd)
            {
                return this;
            }

            return Add(parameterName, valueFactory());
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
        /// Builds a copy of the attributes dictionary.
        /// </summary>
        public Dictionary<string, object> Build()
        {
            return new Dictionary<string, object>(_attributes);
        }
    }
}

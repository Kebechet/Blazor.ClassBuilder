using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blazor.ClassBuilder.Extensions;

namespace Blazor.ClassBuilder
{
    /// <summary>
    /// A builder for inline CSS styles to be used in Blazor components.
    /// </summary>
    public class StyleBuilder
    {
        private readonly StringBuilder _styleBuilder = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBuilder"/> class.
        /// </summary>
        public StyleBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBuilder"/> class with an initial style.
        /// </summary>
        public StyleBuilder(string property, string value)
        {
            Add(property, value);
        }

        /// <summary>
        /// Adds a CSS style property. E.g. Add("color", "red")
        /// </summary>
        public StyleBuilder Add(string property, string value)
        {
            return AddIf(true, property, value);
        }

        /// <summary>
        /// Adds a raw CSS style string. E.g. Add("color: red")
        /// </summary>
        public StyleBuilder Add(string style)
        {
            return AddIf(true, style);
        }

        /// <summary>
        /// Adds a CSS style property with a numeric value. E.g. Add("width", 50.5, "%")
        /// </summary>
        public StyleBuilder Add(string property, double value, string unit = "")
        {
            return AddIf(true, property, value, unit);
        }

        /// <summary>
        /// Adds a CSS style property with an integer value. E.g. Add("z-index", 10)
        /// </summary>
        public StyleBuilder Add(string property, int value, string unit = "")
        {
            return AddIf(true, property, value, unit);
        }

        /// <summary>
        /// Adds a CSS style property if the condition is true.
        /// </summary>
        public StyleBuilder AddIf(bool canAdd, string property, string value)
        {
            if (!canAdd || string.IsNullOrEmpty(property) || string.IsNullOrEmpty(value))
            {
                return this;
            }

            _styleBuilder.Append(property);
            _styleBuilder.Append(": ");
            _styleBuilder.Append(value);
            _styleBuilder.Append("; ");

            return this;
        }

        /// <summary>
        /// Adds a raw CSS style string if the condition is true.
        /// </summary>
        public StyleBuilder AddIf(bool canAdd, string style)
        {
            if (!canAdd || string.IsNullOrEmpty(style))
            {
                return this;
            }

            _styleBuilder.Append(style);
            if (!style.EndsWith(";"))
            {
                _styleBuilder.Append(";");
            }
            _styleBuilder.Append(" ");

            return this;
        }

        /// <summary>
        /// Adds a CSS style property with a numeric value if the condition is true.
        /// </summary>
        public StyleBuilder AddIf(bool canAdd, string property, double value, string unit = "")
        {
            if (!canAdd || string.IsNullOrEmpty(property))
            {
                return this;
            }

            _styleBuilder.Append(property);
            _styleBuilder.Append(": ");
            _styleBuilder.Append(value.ToCssString());
            _styleBuilder.Append(unit);
            _styleBuilder.Append("; ");

            return this;
        }

        /// <summary>
        /// Adds a CSS style property with an integer value if the condition is true.
        /// </summary>
        public StyleBuilder AddIf(bool canAdd, string property, int value, string unit = "")
        {
            if (!canAdd || string.IsNullOrEmpty(property))
            {
                return this;
            }

            _styleBuilder.Append(property);
            _styleBuilder.Append(": ");
            _styleBuilder.Append(value);
            _styleBuilder.Append(unit);
            _styleBuilder.Append("; ");

            return this;
        }

        /// <summary>
        /// Adds a CSS style or invokes a StyleBuilder action based on a condition.
        /// </summary>
        public StyleBuilder AddIf(bool canAdd, Action<StyleBuilder> styleBuilder)
        {
            if (!canAdd)
            {
                return this;
            }

            styleBuilder.Invoke(this);

            return this;
        }

        /// <summary>
        /// Adds a CSS style based on a condition.
        /// </summary>
        public StyleBuilder AddIfElse(bool isTrue, string propertyIfTrue, string valueIfTrue, string propertyIfFalse, string valueIfFalse)
        {
            return isTrue ? Add(propertyIfTrue, valueIfTrue) : Add(propertyIfFalse, valueIfFalse);
        }

        /// <summary>
        /// Adds a CSS style or invokes a StyleBuilder action based on a condition.
        /// </summary>
        public StyleBuilder AddIfElse(bool isTrue, Action<StyleBuilder> styleBuilderIfTrue, Action<StyleBuilder> styleBuilderIfFalse)
        {
            if (isTrue)
            {
                styleBuilderIfTrue.Invoke(this);
            }
            else
            {
                styleBuilderIfFalse.Invoke(this);
            }

            return this;
        }

        /// <summary>
        /// Adds inline styles from an attributes dictionary (e.g., from Blazor @attributes).
        /// Merges the "style" attribute value by appending it to the current styles.
        /// Builder styles are declared first, followed by attribute styles.
        /// Note: In CSS, later declarations override earlier ones, so attribute styles will take precedence
        /// when the same property appears in both builder and attributes.
        /// </summary>
        /// <param name="attributes">The attributes dictionary to merge from.</param>
        /// <param name="key">The attribute key to look for (default: "style").</param>
        public StyleBuilder AddStyleFromAttributes(IReadOnlyDictionary<string, object?>? attributes, string key = "style")
        {
            if (attributes == null)
            {
                return this;
            }

            if (!attributes.TryGetValue(key, out var value))
            {
                return this;
            }

            var styleString = value?.ToString();
            if (string.IsNullOrWhiteSpace(styleString))
            {
                return this;
            }

            // Normalize and append the style string
            var trimmed = styleString.Trim();
            
            // Use the existing Add(string) method which handles raw style strings
            Add(trimmed);

            return this;
        }

        /// <summary>
        /// Adds a raw CSS style snippet verbatim without parsing.
        /// This is an alias for Add(string) for clarity when appending raw styles.
        /// </summary>
        public StyleBuilder AddVerbatim(string cssSnippet)
        {
            return Add(cssSnippet);
        }

        /// <summary>
        /// Adds a style property with a lazy-evaluated condition.
        /// The condition function is invoked when this method is called.
        /// </summary>
        public StyleBuilder Add(Func<bool> when, string property, string value)
        {
            return AddIf(when(), property, value);
        }

        /// <summary>
        /// Adds a style property with a lazy-evaluated value factory if the condition is true.
        /// The factory is only invoked if the condition is true.
        /// </summary>
        public StyleBuilder Add(string property, bool when, Func<string> valueFactory)
        {
            if (!when)
            {
                return this;
            }

            return Add(property, valueFactory());
        }

        /// <summary>
        /// Adds a style property with a numeric value using a lazy-evaluated factory if the condition is true.
        /// The factory is only invoked if the condition is true.
        /// </summary>
        public StyleBuilder Add(string property, bool when, Func<double> valueFactory, string unit = "")
        {
            if (!when)
            {
                return this;
            }

            return Add(property, valueFactory(), unit);
        }

        /// <summary>
        /// Clears all added styles.
        /// </summary>
        public void Clear()
        {
            _styleBuilder.Clear();
        }

        /// <summary>
        /// Builds the final CSS style string.
        /// </summary>
        public string Build()
        {
            return _styleBuilder.ToString().Trim();
        }

        /// <summary>
        /// Builds the final CSS style string, returning null if the result is empty or whitespace.
        /// Useful for preventing Blazor from rendering empty style attributes.
        /// </summary>
        public string? NullIfEmpty()
        {
            var result = Build();
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }

        /// <summary>
        /// Returns the built CSS style string. Equivalent to calling Build().
        /// </summary>
        public override string ToString()
        {
            return Build();
        }
    }
}

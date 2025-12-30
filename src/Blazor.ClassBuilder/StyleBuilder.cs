using System;
using System.Text;

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
        /// Returns the built CSS style string. Equivalent to calling Build().
        /// </summary>
        public override string ToString()
        {
            return Build();
        }
    }
}

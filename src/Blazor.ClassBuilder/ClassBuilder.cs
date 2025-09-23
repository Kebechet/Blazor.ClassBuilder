using System;
using System.Text;

namespace Blazor.ClassBuilder
{
    /// <summary>
    /// A builder for CSS class names to be used in Blazor components.
    /// </summary>
    public class ClassBuilder
    {
        private readonly StringBuilder _cssBuilder = new StringBuilder();
        private readonly string _delimiter = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBuilder"/> class.
        /// </summary>
        public ClassBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassBuilder"/> class with an initial CSS class.
        /// </summary>
        /// <param name="value"></param>
        public ClassBuilder(string value)
        {
            Add(value);
        }

        /// <summary>
        /// Adds a CSS class.
        /// </summary>
        public ClassBuilder Add(string value)
        {
            return AddIf(true, value);
        }

        /// <summary>
        /// Adds multiple CSS classes.
        /// </summary>
        public ClassBuilder Add(params string[] values)
        {
            foreach (var value in values)
            {
                AddIf(true, value);
            }

            return this;
        }

        /// <summary>
        /// Adds a CSS class if the condition is true.
        /// </summary>
        public ClassBuilder AddIf(bool canAdd, string value)
        {
            if (!canAdd)
            {
                return this;
            }

            if (string.IsNullOrEmpty(value))
            {
                return this;
            }

            _cssBuilder.Append(value);
            _cssBuilder.Append(_delimiter);

            return this;
        }

        /// <summary>
        /// Adds a CSS class or invokes a ClassBuilder action based on a condition.
        /// </summary>
        public ClassBuilder AddIf(bool canAdd, Action<ClassBuilder> classBuilder)
        {
            if (!canAdd)
            {
                return this;
            }

            classBuilder.Invoke(this);

            return this;
        }

        /// <summary>
        /// Adds a CSS class or invokes a ClassBuilder action based on a condition.
        /// </summary>
        public ClassBuilder AddIfElse(bool isTrue, string valueIfTrue, string valueIfFalse)
        {
            if (isTrue)
            {
                Add(valueIfTrue);
            }
            else
            {
                Add(valueIfFalse);
            }

            return this;
        }

        /// <summary>
        /// Adds a CSS class or invokes a ClassBuilder action based on a condition.
        /// </summary>
        public ClassBuilder AddIfElse(bool isTrue, Action<ClassBuilder> classBuilderIfTrue, Action<ClassBuilder> classBuilderIfFalse)
        {
            if (isTrue)
            {
                classBuilderIfTrue.Invoke(this);
            }
            else
            {
                classBuilderIfFalse.Invoke(this);
            }

            return this;
        }

        /// <summary>
        /// Adds a CSS class or invokes a ClassBuilder action based on a condition.
        /// </summary>
        public ClassBuilder AddIfElse(bool isTrue, string valueIfTrue, Action<ClassBuilder> classBuilderIfFalse)
        {
            if (isTrue)
            {
                Add(valueIfTrue);
            }
            else
            {
                classBuilderIfFalse.Invoke(this);
            }

            return this;
        }

        /// <summary>
        /// Adds a CSS class or invokes a ClassBuilder action based on a condition.
        /// </summary>
        public ClassBuilder AddIfElse(bool isTrue, Action<ClassBuilder> classBuilderIfTrue, string valueIfFalse)
        {
            if (isTrue)
            {
                classBuilderIfTrue.Invoke(this);
            }
            else
            {
                Add(valueIfFalse);
            }

            return this;
        }

        /// <summary>
        /// Clears all added CSS classes.
        /// </summary>
        public void Clear()
        {
            _cssBuilder.Clear();
        }

        /// <summary>
        /// Builds the final CSS class string.
        /// </summary>
        public string Build()
        {
            return _cssBuilder.ToString().Trim();
        }

        /// <summary>
        /// Returns the built CSS class string. Equivalent to calling Build().
        /// </summary>
        /// <returns>The CSS class string.</returns>
        public override string ToString()
        {
            return Build();
        }
    }
}

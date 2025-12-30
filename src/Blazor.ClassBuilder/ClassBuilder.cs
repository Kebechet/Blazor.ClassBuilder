using System;
using System.Collections.Generic;
using System.Linq;
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
        private string? _prefix = null;
        private string _prefixSeparator = "-";

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

            var classToAdd = _prefix != null ? _prefix + _prefixSeparator + value : value;
            _cssBuilder.Append(classToAdd);
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
        /// Sets a prefix to be applied to all subsequently added CSS classes.
        /// The prefix applies only to classes added via Add/AddIf methods, not to classes merged from attributes.
        /// </summary>
        /// <param name="prefix">The prefix to apply. If null or empty, prefix is cleared.</param>
        /// <param name="separator">The separator between prefix and class name (default: "-").</param>
        public ClassBuilder SetPrefix(string? prefix, string separator = "-")
        {
            _prefix = string.IsNullOrEmpty(prefix) ? null : prefix;
            _prefixSeparator = separator ?? "-";
            return this;
        }

        /// <summary>
        /// Clears the prefix so no prefix is applied to subsequently added classes.
        /// </summary>
        public ClassBuilder ClearPrefix()
        {
            _prefix = null;
            return this;
        }

        /// <summary>
        /// Adds CSS classes from an attributes dictionary (e.g., from Blazor @attributes).
        /// Merges the "class" attribute value by tokenizing and adding individual classes.
        /// Classes from attributes are NOT prefixed even if SetPrefix was called.
        /// </summary>
        /// <param name="attributes">The attributes dictionary to merge from.</param>
        /// <param name="key">The attribute key to look for (default: "class").</param>
        public ClassBuilder AddClassFromAttributes(IReadOnlyDictionary<string, object?>? attributes, string key = "class")
        {
            if (attributes == null)
            {
                return this;
            }

            if (!attributes.TryGetValue(key, out var value))
            {
                return this;
            }

            var classString = value?.ToString();
            if (string.IsNullOrWhiteSpace(classString))
            {
                return this;
            }

            // Tokenize by whitespace, trim each token, and materialize to avoid re-enumeration
            var tokens = classString.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(t => t.Trim())
                                    .Where(t => !string.IsNullOrEmpty(t))
                                    .Distinct()
                                    .ToArray(); // Materialize to avoid re-enumeration

            // Temporarily save prefix and clear it for attribute classes
            var savedPrefix = _prefix;
            try
            {
                _prefix = null;

                foreach (var token in tokens)
                {
                    Add(token);
                }
            }
            finally
            {
                // Always restore prefix, even if an exception occurs
                _prefix = savedPrefix;
            }

            return this;
        }

        /// <summary>
        /// Adds a CSS class based on a lazy-evaluated condition.
        /// The condition function is invoked when this method is called.
        /// </summary>
        public ClassBuilder Add(Func<bool> when, string value)
        {
            return AddIf(when(), value);
        }

        /// <summary>
        /// Adds a CSS class with a lazy-evaluated value factory if the condition is true.
        /// The factory is only invoked if the condition is true.
        /// </summary>
        public ClassBuilder Add(bool when, Func<string> valueFactory)
        {
            if (!when)
            {
                return this;
            }

            return Add(valueFactory());
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
        /// Builds the final CSS class string, returning null if the result is empty or whitespace.
        /// Useful for preventing Blazor from rendering empty class attributes.
        /// </summary>
        public string? NullIfEmpty()
        {
            var result = Build();
            return string.IsNullOrWhiteSpace(result) ? null : result;
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

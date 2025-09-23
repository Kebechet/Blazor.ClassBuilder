using IsNullOrEmpty.Extensions;

namespace Blazor.ClassBuilder.Extensions
{
    internal static class AttributeBuilderExtensions
    {
        internal static bool IsNullOrEmpty(this AttributeBuilder? attributeBuilder)
        {
            if (attributeBuilder is null)
            {
                return true;
            }

            return attributeBuilder.Build().IsNullOrEmpty();
        }
    }
}

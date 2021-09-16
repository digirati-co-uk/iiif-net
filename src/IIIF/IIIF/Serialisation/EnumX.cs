using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace IIIF.Serialisation
{
    internal static class EnumX
    {
        /// <summary>
        /// Get Description value for enum. Will use <see cref="DescriptionAttribute"/> if found, or fall back to value.ToString().
        /// </summary>
        /// <param name="enumValue">Value to get description for.</param>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>String description for enum value.</returns>
        public static string GetDescription<T>(this T enumValue)
            where T : System.Enum
        {
            var memberInfo = typeof(T).GetMember(enumValue.ToString()).Single();
            var desc = memberInfo.GetCustomAttribute<DescriptionAttribute>();
            return desc == null ? enumValue.ToString() : desc.Description;
        }
    }
}
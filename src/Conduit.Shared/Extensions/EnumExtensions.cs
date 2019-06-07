namespace Conduit.Shared.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        /// <summary>
        /// Extracts the description from the description attribute on any enum.
        /// </summary>
        /// <param name="enumeration">Enum with description attributes</param>
        /// <typeparam name="T">Generic enum type</typeparam>
        /// <returns>Description as written in the attribute</returns>
        public static string GetDescription<T>(this T enumeration)
            where T : IConvertible
        {
            string description = null;

            if (!(enumeration is Enum))
            {
                return null;
            }

            var type = enumeration.GetType();
            var values = Enum.GetValues(type);

            foreach (int value in values)
            {
                // Match the enum value in question
                if (value != enumeration.ToInt32(CultureInfo.InvariantCulture))
                {
                    // Enum value is not castable to an integer, continue to next iteration
                    continue;
                }

                // Retrieve the enum member information
                var memberInfoArray = type.GetMember(type.GetEnumName(value));

                // Validate the enum in question
                if (!IsValidEnum(memberInfoArray))
                {
                    continue;
                }

                var descriptionAttributes = memberInfoArray[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptionAttributes.Length <= 0)
                {
                    continue;
                }

                description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                break;
            }

            return description;
        }

        /// <summary>
        /// Interrogates the enum members to determine if the enum contains a valid description attribute.
        /// </summary>
        /// <param name="memberInfo">Enum members and their corresponding meta data</param>
        /// <returns>True if description attribute is found</returns>
        private static bool IsValidEnum(IEnumerable<MemberInfo> memberInfo)
        {
            var memberInfoEnumerable = memberInfo as MemberInfo[] ?? memberInfo.ToArray();
            return memberInfoEnumerable.Any() &&
                   memberInfoEnumerable
                       .First()
                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                       .Any();
        }
    }
}
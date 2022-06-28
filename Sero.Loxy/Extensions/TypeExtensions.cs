using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public static class TypeExtensions
    {
        /// <summary>
        ///     Makes sure the full type name returned is human friendly, 
        ///     although less detailed.
        ///     
        ///     Mostly relevant for generic types, where the full name gets
        ///     too verbous.
        ///     
        ///     Example output: ´LoggerProxyEventCandidate<String>´
        /// </summary>
        public static string GetFriendlyFullName(this Type type)
        {
            if (type.IsGenericType)
            {
                IEnumerable<string> genericArgTypeNames =
                    type.GetGenericArguments().Select(x => GetFriendlyFullName(x));

                string friendlyName =
                    string.Format(
                        "{0}<{1}>",
                        type.Name.Split('`')[0],
                        string.Join(", ", genericArgTypeNames));

                return friendlyName;
            }
            else
                return type.Name;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nbn.eCommerce.ItemService.Utility
{
    public static class Guard
    {
        /// <summary>
        /// Guard against null or empty enumerable.
        /// </summary>
        /// <param name="value">The IEnumerable to evaulate.</param>
        /// <param name="name">The object name used in logging.</param>
        /// <param name="file">Optional file name.</param>
        /// <param name="line">Optional line number.</param>
        /// <param name="method">Optional method name.</param>
        /// <exception cref="ArgumentException">The IEnumerable is null or empty.</exception>
        /// <exception cref="ArgumentNullException">The IEnumerable is null.</exception>
        public static void AgainstNullOrEmptyEnumerable(IEnumerable value, string name, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string method = "")
        {
            if (value == null)
            {
                throw new ArgumentNullException(message: $"Null enumerable failed guard. {MethodInfo.GetInfo(file, line, method)}", paramName: name);
            }
            else
            {
                foreach (object o in value)
                {
                    return;
                }

                throw new ArgumentException(message: $"Empty enumerable failed guard. {MethodInfo.GetInfo(file, line, method)}", paramName: name);
            }
        }
    }
}

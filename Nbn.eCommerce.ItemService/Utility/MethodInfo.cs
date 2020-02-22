// <copyright file="Startup.cs" company="Nabin eCommerce API">
// Copyright (c) 2020 Nanina Trading Est. All rights reserved.
// </copyright>

namespace Nbn.eCommerce.ItemService.Utility
{
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Method info class
    /// </summary>
    public static class MethodInfo
    {
        /// <summary>
        /// Get the name of the calling method.
        /// </summary>
        /// <param name="name">Default name, leave this alone.</param>
        /// <returns>The name of the calling method.</returns>
        public static string GetMethodName([CallerMemberName] string name = "")
        {
            return name;
        }

        /// <summary>
        /// Get the name of the file containing the calling method.
        /// </summary>
        /// <param name="path">Default path, leave this alone.</param>
        /// <returns>The name of the file containing the calling method.</returns>
        public static string GetFileName([CallerFilePath] string path = "")
        {
            var file = Path.GetFileName(path);
            return file;
        }

        /// <summary>
        /// Get the calling line number.
        /// </summary>
        /// <param name="line">Default line, leave this alone.</param>
        /// <returns>The calling line number.</returns>
        public static int GetLineNumber([CallerLineNumber] int line = -1)
        {
            return line;
        }

        /// <summary>
        /// Get a string representation of the method info.
        /// </summary>
        /// <param name="path">Default path, leave this alone.</param>
        /// <param name="line">Default line, leave this alone.</param>
        /// <param name="name">Default name, leave this alone.</param>
        /// <returns>A string representation of the method info.</returns>
        public static string GetInfo([CallerFilePath] string path = "", [CallerLineNumber] int line = -1, [CallerMemberName] string name = "")
        {
            var file = Path.GetFileName(path);
            return $"(method=\"{name}\", file=\"{file}\", line={line})";
        }
    }
}

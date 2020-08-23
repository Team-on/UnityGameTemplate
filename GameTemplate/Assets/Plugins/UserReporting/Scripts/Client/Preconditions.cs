using System;

namespace Unity.Cloud
{
    /// <summary>
    /// Provides static methods for helping with preconditions.
    /// </summary>
    public static class Preconditions
    {
        #region Static Methods

        /// <summary>
        /// Ensures that an argument is less than or equal to the specified length.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <param name="argumentName">The argument name.</param>
        public static void ArgumentIsLessThanOrEqualToLength(object value, int length, string argumentName)
        {
            string stringValue = value as string;
            if (stringValue != null && stringValue.Length > length)
            {
                throw new ArgumentException(argumentName);
            }
        }

        /// <summary>
        /// Ensures that an argument is not null or whitespace.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="argumentName">The argument name.</param>
        public static void ArgumentNotNullOrWhitespace(object value, string argumentName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            string stringValue = value as string;
            if (stringValue != null && stringValue.Trim() == string.Empty)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        #endregion
    }
}
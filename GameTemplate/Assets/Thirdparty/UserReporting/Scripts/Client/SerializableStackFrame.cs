using System;
using System.Diagnostics;
using System.Reflection;

namespace Unity.Cloud
{
    /// <summary>
    /// Represents a serializable stack frame.
    /// </summary>
    public class SerializableStackFrame
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="SerializableStackFrame"/> class.
        /// </summary>
        public SerializableStackFrame()
        {
            // Empty
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SerializableStackFrame"/> class.
        /// </summary>
        /// <param name="stackFrame">The stack frame.</param>
        public SerializableStackFrame(StackFrame stackFrame)
        {
            MethodBase method = stackFrame.GetMethod();
            Type declaringType = method.DeclaringType;
            this.DeclaringType = declaringType != null ? declaringType.FullName : null;
            this.Method = method.ToString();
            this.MethodName = method.Name;
            this.FileName = stackFrame.GetFileName();
            this.FileLine = stackFrame.GetFileLineNumber();
            this.FileColumn = stackFrame.GetFileColumnNumber();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the declaring type.
        /// </summary>
        public string DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the file column.
        /// </summary>
        public int FileColumn { get; set; }

        /// <summary>
        /// Gets or sets the file line.
        /// </summary>
        public int FileLine { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the method name.
        /// </summary>
        public string MethodName { get; set; }

        #endregion
    }
}
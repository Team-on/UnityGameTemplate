using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Unity.Cloud
{
    /// <summary>
    /// Represents a serializable exception.
    /// </summary>
    public class SerializableException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="SerializableException"/> class.
        /// </summary>
        public SerializableException()
        {
            // Empty
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SerializableException"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public SerializableException(Exception exception)
        {
            // Message
            this.Message = exception.Message;

            // Full Text
            this.FullText = exception.ToString();

            // Type
            Type exceptionType = exception.GetType();
            this.Type = exceptionType.FullName;

            // Stack Trace
            this.StackTrace = new List<SerializableStackFrame>();
            StackTrace stackTrace = new StackTrace(exception, true);
            foreach (StackFrame stackFrame in stackTrace.GetFrames())
            {
                this.StackTrace.Add(new SerializableStackFrame(stackFrame));
            }

            // Problem Identifier
            if (this.StackTrace.Count > 0)
            {
                SerializableStackFrame stackFrame = this.StackTrace[0];
                this.ProblemIdentifier = string.Format("{0} at {1}.{2}", this.Type, stackFrame.DeclaringType, stackFrame.MethodName);
            }
            else
            {
                this.ProblemIdentifier = this.Type;
            }

            // Detailed Problem Identifier
            if (this.StackTrace.Count > 1)
            {
                SerializableStackFrame stackFrame1 = this.StackTrace[0];
                SerializableStackFrame stackFrame2 = this.StackTrace[1];
                this.DetailedProblemIdentifier = string.Format("{0} at {1}.{2} from {3}.{4}", this.Type, stackFrame1.DeclaringType, stackFrame1.MethodName, stackFrame2.DeclaringType, stackFrame2.MethodName);
            }

            // Inner Exception
            if (exception.InnerException != null)
            {
                this.InnerException = new SerializableException(exception.InnerException);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the detailed problem identifier.
        /// </summary>
        public string DetailedProblemIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the full text.
        /// </summary>
        public string FullText { get; set; }

        /// <summary>
        /// Gets or sets the inner exception.
        /// </summary>
        public SerializableException InnerException { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the problem identifier.
        /// </summary>
        public string ProblemIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public List<SerializableStackFrame> StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}
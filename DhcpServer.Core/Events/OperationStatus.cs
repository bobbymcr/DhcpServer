// <copyright file="OperationStatus.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer.Events
{
    using System;

    /// <summary>
    /// Captures the success or failure status of an operation.
    /// </summary>
    public readonly struct OperationStatus
    {
        private OperationStatus(bool succeeded, Exception exception)
        {
            this.Succeeded = succeeded;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets a value indicating whether the operation succeeded.
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// Gets the exception (if any) thrown by the operation.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Creates a status value indicating success.
        /// </summary>
        /// <returns>A successful status value.</returns>
        public static OperationStatus Success() => new OperationStatus(true, null);

        /// <summary>
        /// Creates a status value indicating failure.
        /// </summary>
        /// <param name="exception">The exception thrown by the operation or <c>null</c>.</param>
        /// <returns>A failure status value.</returns>
        public static OperationStatus Failure(Exception exception) => new OperationStatus(false, exception);
    }
}

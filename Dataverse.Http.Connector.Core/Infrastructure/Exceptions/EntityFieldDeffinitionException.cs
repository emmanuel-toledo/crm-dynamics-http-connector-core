﻿namespace Dataverse.Http.Connector.Core.Infrastructure.Exceptions
{
    /// <summary>
    /// This class works to define a type of exception where an entity's (class) property was not configured to use the library.
    /// </summary>
    [Serializable]
    public class EntityFieldDeffinitionException : Exception
    {
        /// <summary>
        /// Throw a new default exception.
        /// </summary>
        public EntityFieldDeffinitionException() : base() { }

        /// <summary>
        /// Throw a new exception with custom message.
        /// </summary>
        /// <param name="message">Error custom message.</param>
        public EntityFieldDeffinitionException(string message) : base(message) { }

        /// <summary>
        /// Throw a new exception with custom message and details exception. 
        /// </summary>
        /// <param name="message">Error custom message.</param>
        /// <param name="innerException">Details exception instance.</param>
        public EntityFieldDeffinitionException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Throw a new exception with default message.
        /// </summary>
        /// <param name="name">Configured entity name.</param>
        /// <param name="key">Configured entity type.</param>
        public EntityFieldDeffinitionException(string name, object key) : base($"Entity's property with name '{name}' and type {key} was not configured in the scheme.") { }
    }
}

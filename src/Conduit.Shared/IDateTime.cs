namespace Conduit.Shared
{
    using System;

    /// <summary>
    /// Date time interface used throughout the application, implemented in the infrastructure layer.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the current date time year.
        /// </summary>
        int CurrentYear { get; }

        /// <summary>
        /// Gets the current date time month.
        /// </summary>
        int CurrentMonth { get; }

        /// <summary>
        /// Gets the current date time year.
        /// </summary>
        int CurrentDay { get; }
    }
}
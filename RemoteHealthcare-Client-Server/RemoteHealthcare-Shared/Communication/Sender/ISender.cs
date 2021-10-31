using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteHealthcare_Shared
{
    /// <summary>
    /// Interface that can be used by all types of tcp read and write classes.
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Read the encrypted message.
        /// </summary>
        /// <returns>The string that was received.</returns>
        string ReadMessage();

        /// <summary>
        /// Sends a string to the connection.
        /// </summary>
        /// <param name="message">String to send.</param>
        void SendMessage(string message);
    }
}

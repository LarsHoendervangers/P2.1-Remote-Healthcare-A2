using CommClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;

namespace RemoteHealthcare_Shared
{
    public class EncryptedClient : EncryptedSenderReceiver
    {
        /// <summary>
        /// Constructor. Will try to authenticate as client when creating this instance.
        /// </summary>
        /// <param name="network">The stream that should be encrypted.</param>
        public EncryptedClient(NetworkStream network)
        {
            base.sslStream = new SslStream(network, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            
            try
            {
                base.sslStream.AuthenticateAsClient("localhost");
                Trace.WriteLine("Client: Authenticated");
            } 
            catch (AuthenticationException e)
            {
                Trace.WriteLine($"Exception: {e.Message}");
                if (e.InnerException != null)
                {
                    Trace.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
                Trace.WriteLine("Authentication failed - closing the connection.");
                base.sslStream.Close();
                return;
            }
        }

        /// <summary>
        /// This method is invoked by constructor to validate the certificate of the server.
        /// </summary>
        /// <returns>True if the certificate is valid. Otherwise it will return false.</returns>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Debug.WriteLine($"Certificate error: {sslPolicyErrors}");

            return true;
        }
    }
}

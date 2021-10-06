using RemoteHealthcare_Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CommClass
{
    public class EncryptedServer : EncryptedSenderReceiver
    {
        static X509Certificate serverCertificate = null;

        public EncryptedServer(NetworkStream network)
        {
            base.sslStream = new SslStream(network, false);
            Debug.WriteLine("EncryptedServer() called");
            // Try to create a new certificate.
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"Server.pfx");
                serverCertificate = X509Certificate.CreateFromCertFile(path);

                Debug.WriteLine(path);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Certificate exception {e.Message}");
            }

            // Try to authenticate.
            try
            {
                base.sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);
            }
            catch (AuthenticationException e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {e.InnerException.Message}");
                }
                Debug.WriteLine("Authentication failed - closing the connection.");
                base.sslStream.Close();
                network.Close();
                return;
            }
        }
    }
}

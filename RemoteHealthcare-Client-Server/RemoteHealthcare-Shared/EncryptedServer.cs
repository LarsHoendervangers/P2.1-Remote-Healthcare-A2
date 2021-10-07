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

                // Display the properties and settings for the authenticated stream.
                DisplaySecurityLevel(sslStream);
                DisplaySecurityServices(sslStream);
                DisplayCertificateInformation(sslStream);
                DisplayStreamProperties(sslStream);
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

        static void DisplaySecurityLevel(SslStream stream)
        {
            Debug.WriteLine($"Cipher: {stream.CipherAlgorithm} strength {stream.CipherStrength}");
            Debug.WriteLine($"Hash: {stream.HashAlgorithm} strength {stream.HashStrength}");
            Debug.WriteLine($"Key exchange: {stream.KeyExchangeAlgorithm} strength {stream.KeyExchangeStrength}");
            Debug.WriteLine($"Protocol: {stream.SslProtocol}");
        }

        static void DisplaySecurityServices(SslStream stream)
        {
            Debug.WriteLine($"Is authenticated: {stream.IsAuthenticated} as server? {stream.IsServer}");
            Debug.WriteLine($"IsSigned: {stream.IsSigned}");
            Debug.WriteLine($"Is Encrypted: {stream.IsEncrypted}");
        }

        static void DisplayStreamProperties(SslStream stream)
        {
            Debug.WriteLine($"Can read: {stream.CanRead}, write {stream.CanWrite}");
            Debug.WriteLine($"Can timeout: {stream.CanTimeout}");
        }

        static void DisplayCertificateInformation(SslStream stream)
        {
            Debug.WriteLine($"Certificate revocation list checked: {stream.CheckCertRevocationStatus}");

            X509Certificate localCertificate = stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Debug.WriteLine($"Local cert was issued to {localCertificate.Subject} and is valid from {localCertificate.GetEffectiveDateString()} until {localCertificate.GetExpirationDateString()}.");
            }
            else
            {
                Debug.WriteLine("Local certificate is null.");
            }
            // Display the properties of the client's certificate.
            X509Certificate remoteCertificate = stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
                Debug.WriteLine($"Remote cert was issued to {remoteCertificate.Subject} and is valid from {remoteCertificate.GetEffectiveDateString()} until {remoteCertificate.GetExpirationDateString()}.");
            }
            else
            {
                Debug.WriteLine("Remote certificate is null.");
            }
        }
    }
}

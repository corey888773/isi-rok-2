using System;
using System.IO;
using System.Security.Cryptography;

namespace Main
{ 
    class Lab07
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "0")
            {
                GenerateKeys();
            }
            if (args.Length == 3 && args[0] == "1"){
                EncryptFile(args[1], args[2]);
            }
            if (args.Length == 3 && args[0] == "2"){
                DecryptFile(args[1], args[2]);
            }
            else{
                Console.WriteLine("Invalid command or arguments.");
            }
        }

        static void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                File.WriteAllText("public.key", rsa.ToXmlString(false));
                File.WriteAllText("private.key", rsa.ToXmlString(true));
                Console.WriteLine("Keys generated.");
            }
        }

        static void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                var publicKey = File.ReadAllText("public.key");
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    var inputData = File.ReadAllBytes(inputFile);
                    var encryptedData = rsa.Encrypt(inputData, false);
                    File.WriteAllBytes(outputFile, encryptedData);
                    Console.WriteLine("File has been encrypted and saved in {0}", outputFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encryption exception: " + ex.Message);
            }
        }

        static void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                var privateKey = File.ReadAllText("private.key");
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);
                    var encryptedData = File.ReadAllBytes(inputFile);
                    var decryptedData = rsa.Decrypt(encryptedData, false);
                    File.WriteAllBytes(outputFile, decryptedData);
                    Console.WriteLine("File has been decrypted and saved in {0}", outputFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption exception: " + ex.Message);
            }
        }
    }
}

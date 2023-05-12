using System;
using System.IO;
using System.Security.Cryptography;

namespace SymmetricEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: SymmetricEncryption <input_file> <output_file> <password> <0/1>");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];
            string password = args[2];
            int operationType = int.Parse(args[3]);

            if (operationType == 0) // Encrypt
            {
                // Generate key and IV from password
                Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                byte[] key = keyGenerator.GetBytes(32);
                byte[] iv = keyGenerator.GetBytes(16);

                // Encrypt file
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                    using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        inputFileStream.CopyTo(cryptoStream);
                    }
                }

                Console.WriteLine("Plik został zaszyfrowany.");    
            }
            else if (operationType == 1) // Decrypt
            {
                // Generate key and IV from password
                Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                byte[] key = keyGenerator.GetBytes(32);
                byte[] iv = keyGenerator.GetBytes(16);

                // Decrypt file
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                    using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(outputFileStream);
                    }
                }
                
                Console.WriteLine("Plik został odszyfrowany.");
            }
            else
            {
                Console.WriteLine("Invalid operation type: {0}. Allowed values: 0 (encrypt), 1 (decrypt)", operationType);
            }
        }
    }
}

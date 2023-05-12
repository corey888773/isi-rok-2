using System;
using System.IO;
using System.Security.Cryptography;

namespace ChecksumCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid arguments");
                return;
            }

            string fileName = args[0];
            string hashFileName = args[1];
            string hashAlgorithmName = args[2];

            HashAlgorithm hashAlgorithm;

            switch (hashAlgorithmName)
            {
                case "SHA256":
                    hashAlgorithm = SHA256.Create();
                    break;
                case "SHA512":
                    hashAlgorithm = SHA512.Create();
                    break;
                case "MD5":
                    hashAlgorithm = MD5.Create();
                    break;
                default:
                    Console.WriteLine("Invalid hash algorithm name");
                    return;
            }

            if (!File.Exists(hashFileName))
            {
                byte[] hash = ComputeChecksum(fileName, hashAlgorithm);
                File.WriteAllBytes(hashFileName, hash);
                Console.WriteLine($"File {hashFileName} created. Checksum: {BitConverter.ToString(hash).Replace("-", "")}");
            }
            else
            {
                byte[] hash = File.ReadAllBytes(hashFileName);
                byte[] computedHash = ComputeChecksum(fileName, hashAlgorithm);
                bool match = CompareChecksums(hash, computedHash);
                if (match)
                {
                    Console.WriteLine($"Checksum of file {fileName} is correct.");
                }
                else
                {
                    Console.WriteLine($"Checksum of file {fileName} is incorrect.");
                }
            }
        }

        static byte[] ComputeChecksum(string fileName, HashAlgorithm hashAlgorithm)
        {
            using (FileStream stream = File.OpenRead(fileName))
            {
                return hashAlgorithm.ComputeHash(stream);
            }
        }

        static bool CompareChecksums(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
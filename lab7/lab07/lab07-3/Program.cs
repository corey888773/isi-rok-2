using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Użycie: Podpis.exe plik_wejsciowy plik_wyjsciowy");
            return;
        }

        string inputFile = args[0];
        string signatureFile = args[1];

        if (!File.Exists("private.key") || !File.Exists("public.key"))
        {
            Console.WriteLine("Nie można odnaleźć plików z kluczami prywatnym i publicznym.");
            return;
        }

        byte[] dataToSign = File.ReadAllBytes(inputFile);

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(File.ReadAllText("private.key"));

        if (File.Exists(signatureFile))
        {
            // Weryfikacja podpisu

            byte[] signature = File.ReadAllBytes(signatureFile);

            if (rsa.VerifyData(dataToSign, CryptoConfig.MapNameToOID("SHA256"), signature))
            {
                Console.WriteLine("Podpis jest poprawny.");
            }
            else
            {
                Console.WriteLine("Podpis jest niepoprawny.");
            }
        }
        else
        {
            // Generowanie podpisu i zapisanie do pliku

            byte[] signature = rsa.SignData(dataToSign, CryptoConfig.MapNameToOID("SHA256"));

            File.WriteAllBytes(signatureFile, signature);

            Console.WriteLine("Podpis wygenerowany i zapisany do pliku.");
        }
    }
}
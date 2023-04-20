using System;
using System.Net.Sockets;

class ClientProgram
{
    static void Main(string[] args)
    {
        // Utworzenie gniazda klienta i połączenie z serwerem
        TcpClient client = new TcpClient("localhost", 12345);
        Console.WriteLine("Connected to server");

        // Wysłanie rozmiaru wiadomości do serwera
        string message = Console.ReadLine();
        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
        byte[] sizeBytes = BitConverter.GetBytes(messageBytes.Length);
        client.GetStream().Write(sizeBytes, 0, sizeBytes.Length);

        // Wysłanie właściwej wiadomości do serwera
        client.GetStream().Write(messageBytes, 0, messageBytes.Length);

        // Odebranie potwierdzenia od serwera
        byte[] responseSizeBytes = new byte[4];
        client.GetStream().Read(responseSizeBytes, 0, responseSizeBytes.Length);
        int responseSize = BitConverter.ToInt32(responseSizeBytes, 0);
        byte[] responseBytes = new byte[responseSize];
        client.GetStream().Read(responseBytes, 0, responseBytes.Length);
        string response = System.Text.Encoding.UTF8.GetString(responseBytes);
        Console.WriteLine(response);

        // Zamknięcie połączenia
        client.Close();
        Console.WriteLine("Connection closed");
    }
}
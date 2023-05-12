using System;
using System.Net;
using System.Net.Sockets;

class ServerProgram
{
    static void Main(string[] args)
    {
        // Utworzenie gniazda serwerowego i ustawienie adresu i portu
        TcpListener listener = new TcpListener(IPAddress.Any, 12345);
        listener.Start();
        Console.WriteLine("Server started");

        // Oczekiwanie na połączenie z klientem
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Client connected");

        // Odebranie rozmiaru wiadomości od klienta
        byte[] sizeBytes = new byte[4];
        client.GetStream().Read(sizeBytes, 0, sizeBytes.Length);
        int size = BitConverter.ToInt32(sizeBytes, 0);
        Console.WriteLine("Received message size: {0}", size);

        // Odebranie właściwej wiadomości od klienta
        byte[] messageBytes = new byte[size];
        client.GetStream().Read(messageBytes, 0, messageBytes.Length);
        string message = System.Text.Encoding.UTF8.GetString(messageBytes);
        Console.WriteLine("Received message: {0}", message);

        // Wysłanie potwierdzenia do klienta
        string response = "odczytalem: " + message;
        byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
        byte[] responseSizeBytes = BitConverter.GetBytes(responseBytes.Length);
        client.GetStream().Write(responseSizeBytes, 0, responseSizeBytes.Length);
        client.GetStream().Write(responseBytes, 0, responseBytes.Length);

        // Zamknięcie połączenia
        client.Close();
        listener.Stop();
        Console.WriteLine("Server stopped");
    }
}
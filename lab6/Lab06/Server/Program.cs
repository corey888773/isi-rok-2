using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    public static void Main() {
        byte[] buffer = new byte[1024];
        TcpListener serverSocket = new TcpListener(IPAddress.Any, 8888);
        serverSocket.Start();
        Console.WriteLine("Serwer uruchomiony, oczekiwanie na klienta...");

        TcpClient clientSocket = serverSocket.AcceptTcpClient();
        Console.WriteLine("Klient połączony.");

        NetworkStream networkStream = clientSocket.GetStream();
        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        
        if (dataReceived.Length > 1024) {
            dataReceived = dataReceived.Substring(0, 1024);
        }
        
        Console.WriteLine("Odebrano wiadomość: " + dataReceived);

        string messageToSend = "odczytałem: " + dataReceived;
        byte[] messageBytes = Encoding.UTF8.GetBytes(messageToSend);
        if (messageBytes.Length > 1024) {
            messageBytes = messageBytes[..1024];
        }
        networkStream.Write(messageBytes, 0, messageBytes.Length);

        clientSocket.Close();
        serverSocket.Stop();
        Console.WriteLine("Serwer zakończył działanie.");
    }
}
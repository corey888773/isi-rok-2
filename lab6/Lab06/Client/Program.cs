using System;
using System.Net.Sockets;
using System.Text;

public class Client {
    public static void Main() {
        TcpClient clientSocket = new TcpClient();
        clientSocket.Connect("localhost", 8888);

        Console.WriteLine("Połączono z serwerem. Wpisz wiadomość do przesłania:");

        string messageToSend = Console.ReadLine();
        if (messageToSend.Length > 1024) {
            messageToSend = messageToSend.Substring(0, 1024);
        }
        byte[] messageBytes = Encoding.UTF8.GetBytes(messageToSend);
        NetworkStream networkStream = clientSocket.GetStream();
        networkStream.Write(messageBytes, 0, messageBytes.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        if (dataReceived.Length > 1024) {
            dataReceived = dataReceived.Substring(0, 1024);
        }
        Console.WriteLine("Odebrano wiadomość od serwera: " + dataReceived);

        clientSocket.Close();
        Console.WriteLine("Klient zakończył działanie.");
    }
}
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient("localhost", 8001);
                Console.WriteLine("Connected to server");
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                bool end = false;
                while (!end)
                {
                    Console.Write("Enter command: ");
                    string input = Console.ReadLine();

                    if (input.Equals("!end"))
                    {
                        end = true;
                    }
                    else
                    {
                        writer.WriteLine(input);
                        writer.Flush();

                        string response = reader.ReadLine();
                        Console.WriteLine(response);
                    }
                }

                reader.Close();
                writer.Close();
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}

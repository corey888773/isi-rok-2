using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpListener server = new TcpListener(IPAddress.Any, 8001);
                server.Start();
                Console.WriteLine("Waiting for client connection...");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected");

                // string myDir = Directory.GetCurrentDirectory();
                string myDir = Path.GetFullPath("./");
                Console.WriteLine("Server directory: " + myDir);

                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                bool end = false;
                while (!end)
                {
                    string input = reader.ReadLine();
                    Console.WriteLine("Received command: " + input);

                    if (input.Equals("!end"))
                    {
                        end = true;
                    }
                    else if (input.Equals("list"))
                    {
                        string[] files = Directory.GetFiles(myDir);
                        string[] dirs = Directory.GetDirectories(myDir);

                        string response = "";
                        foreach (string file in files)
                        {
                            response += Path.GetFileName(file) + ", ";
                        }
                        foreach (string dir in dirs)
                        {
                            response += Path.GetFileName(dir) + ", ";
                        }

                        Console.WriteLine("Sending response: " + response);

                        writer.WriteLine(response);
                        writer.Flush();
                    }
                    else if (input.StartsWith("in "))
                    {
                        try{
                            string subdir = input.Substring(3);

                            if (Directory.Exists(Path.Combine(myDir, subdir)))
                            {
                                myDir = Path.GetFullPath(Path.Combine(myDir, subdir));
                            }
                            else
                            {
                                writer.WriteLine("katalog nie istnieje");
                                writer.Flush();
                                continue;
                            }

                            string[] files = Directory.GetFiles(myDir);
                            string[] dirs = Directory.GetDirectories(myDir);

                            string response = "";
                            foreach (string file in files)
                            {
                                response += Path.GetFileName(file) + ", ";
                            }
                            foreach (string dir in dirs)
                            {
                                response += Path.GetFileName(dir) + ", ";
                            }

                            writer.WriteLine(response);
                            writer.Flush();
                            }catch(Exception e)
                        {
                            Console.WriteLine("Nie da się wejść do katalogu");
                            writer.Flush();
                        }
                    }
                    else
                    {
                        writer.WriteLine("nieznane polecenie");
                        writer.Flush();
                    }
                }

                reader.Close();
                writer.Close();
                stream.Close();
                client.Close();
                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
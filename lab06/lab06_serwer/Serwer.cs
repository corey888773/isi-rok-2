using System.Security.Cryptography;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using lab06.lab06_client;

namespace lab06.lab06_serwer
{
    public class Serwer
    {
        public bool koniec = false;
        public void Zakoncz()
        {
            try
            {
                Console.WriteLine("Kończenie pracy serwera");
                lock (workerThreads)
                {
                    foreach(ClientThread wt in workerThreads)
                    {
                        wt.ZakonczPolaczenie();
                    }
                }
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
            catch {}
            koniec = true;
        }

        //lista z obiektami obsługującymi połączenia full duplex z klientem
        List<ClientThread> workerThreads = new List<ClientThread>();
        public static int Main(String[] args)
        {
            //tworzenie nowego obiektu serwera, program napisany jest obiektowo 
            //a nie na metodach statycznych
            Serwer sl = new Serwer();
            //puszczamy serwer w tle
            Task watekSerwera = new Task(sl.StartServer);
            watekSerwera.Start();
            //ponieważ Task jest wątkiem w tle nie kończymy głównego wątku tylko
            //trzymamy go w pętli, jeśli chcemy możemy zrobić obsługę klawiatury itp.
            bool koniec = false;

            /* zamyka serwer po 30 sekundach
            Task.Delay(30000).Wait();
            sl.Zakoncz();
            */
            while (!koniec)
            {
                Task.Delay(100).Wait();
            }
            // zatrzymaj wątek serwera
            return 0;
        }
        //funkcja do obsługi callbacka wątków klienta, kiedy klient się rozłącza
        //usuwamy jego obiekt z listy
        public void UsunWorkera(ClientThread wt)
        {
            lock(workerThreads)
            {
                if (workerThreads.Contains(wt))
                    workerThreads.Remove(wt);
            }
        }
        //callback obsługujący odbieranie wiadomości
        public void OdebranaWiadomosc(string abc, ClientThread wt)
        {
            Console.Write("Wątek " + wt.Name + ":");
            Console.WriteLine(" odebrano wiadomość " + abc);

            if (abc == "!end")
            {
                // zatrzymaj wątek serwera i zakończ program
                Zakoncz();
            }
        }

        Socket ?listener = null;
        public void StartServer()
        {
            // Startujemy serwer na 127.0.0.1 na wysokim porcie, np. 11000
            // jeśli host ma wiele adresów, dostajemy listę adresów
            IPHostEntry host = Dns.GetHostEntry("localhost");
            //wybieramy pierwszy adres z listy
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            try {
                // tworzymy socket na protokole TCP
                listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // łączymy socket z adresem metodą BIND
                listener.Bind(localEndPoint);
                // ustawiamy socket w stan nasłuchu, w tym wypadku na maksimum 10 połączeń,
                // które zostaną obsłużone, jeśli będzie ich więcej serwer odpowie, że jest zajęty
                listener.Listen(10);

                // symulacja komunikacji klient - serwer
                // w losowych odstępach czasu wysyłamy wiadomość do każdego wątku klienta 
                Random rand = new Random();
                Task symKomunikacji = new Task(async ()=>
                {
                    while (!koniec)
                    {
                        // sekcja krytyczna - nie możemy modyfikować kolekcji workerThreads
                        // z innego miejsca programu, jeśli idzie po niej pętla foreach
                        // ta sekcja wyklucza się z sekcją dodawania nowego połączenia do listy
                        // workerThreads
                        lock (workerThreads)
                        {
                            foreach(ClientThread wt in workerThreads)
                            {
                                // jeśli wątek ustawiony jest na koniec, to usuwamy go
                                if (wt.koniec)
                                    workerThreads.Remove(wt);
                                else//jeśli nie, wysyłamy wiadomość
                                {
                                    // wt.WyslijWiadomosc("Serwer mówi: cześć!");
                                }       
                            }
                        }
                        Task.Delay(rand.Next(500,5000)).Wait();
                    }
                });
                symKomunikacji.Start();
                //
                Console.WriteLine("Serwer czeka na nowe połączenia");
                //wątek czeka do zakończenia programu, tak naprawdę program serwera
                while (!koniec) 
                {
                    Socket handler = listener.Accept();
                    Console.WriteLine("Odebrano połączenie");
                    ClientThread wt = new ClientThread(handler, 
                        OdebranaWiadomosc, UsunWorkera);
                    wt.WyslijWiadomosc("HEJ" + wt.Name);
                    Task t = new Task(wt.Start);
                    t.Start();
                    workerThreads.Add(wt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
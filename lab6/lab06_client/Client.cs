using System.Net;
using System.Net.Sockets;
using System.Text;
namespace lab06.lab06_client
{
    public class Client
    {
        public bool koniec = false;
        public static int Main(String[] args)
        {
            //tworzenie nowego wątku klienta
            Client sc = new Client();
            Task t = new Task(() => sc.StartClient());
            t.Start();
            //główny wątek gotowy jest np. na przyjmowanie
            //komend z klawiatury, w tym wypadku program działa tak
            //długo jak nie rozłączy się z nim serwer
            while (!sc.koniec){
                Task.Delay(100).Wait();
            }
            return 0;
        }
        //callback do funkcji odbierającej dane
        public void OdebranaWiadomosc(string abc, ClientThread wt)
        {
            Console.WriteLine("Odebrano wiadomość " + abc);
        }

        //funkcja do obsługi callbacka kiedy połączenie z serwerem zostanie zakończone
        //ustawiamy koniec == true, co zastopuje główny program
        public void ZakonczPolaczenie(ClientThread wt)
        {
            koniec = true;
        }
        //start wątku klienta
        public void StartClient()
        {
            //zakładamy, że wiadomość nie przekroczy 1024 bajtów
            byte[] bytes = new byte[1024];

            try
            {
                // Łączymy się z serwerm lokalhości na wysokim porcie, np. 11000
                // jeśli host ma wiele adresów, dostajemy listę adresów
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Tworzymy socket TCP/IP
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // łączymy się ze zdalnym endpointem i przechwytujemy wyjątki
                try
                {
                    sender.Connect(remoteEP);
                    //tworzymy wątek klienta
                    ClientThread wt = new ClientThread(sender, 
                        OdebranaWiadomosc,
                        ZakonczPolaczenie);
                    Task t = new Task(wt.Start);
                    t.Start();

                    //symulacja komunikacji z serwerem, w losowych odstępach
                    //czasu na serwer wysyłana jest wiadomość
                    Random rand = new Random();
                    Task symKomunikacji = new Task(async () =>
                    {
                        while (!wt.koniec)
                        {

                            wt.WyslijWlasnaWiadomosc();
                        }
                    });
                    symKomunikacji.Start();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void WczytajWyslij(){
            var message = Console.ReadLine();
        }
    }
}

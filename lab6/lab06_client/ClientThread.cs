using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lab06.lab06_client
{
    //wątek obsługuje pojedyncze połączenie klienta TCP z serwerem TCP
    //obsługuje połączenie full duplex
    //przesyła napisy kodowane w UTF8
    public class ClientThread
    {
        //kiedy koniec == true kończy wszystkie wątki
        public bool koniec = false;

        private string myDir;
        //wątek do odbierania danych z serwera
        Task ?taskOdbieranie = null;
        //gniazdo (socket) TCP/IP klienta
        Socket ?gniazdoKlienta = null;
        //metoda jest wywoływana kiedy przychodzi nowa wiadomość z serwera
        private Action<string, ClientThread> ?OdbieranieWiadomosciCallback = null;
        //metoda jest wywoływana kiedy zamykane jest połączenie klienta z serwerem
        private Action<ClientThread> ?ZakonczPolaczenieCallback = null;
        //nazwa klienta: IP:PORT
        public String Name { get{ 
            IPEndPoint remoteIpEndPoint = gniazdoKlienta.RemoteEndPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                return "" + remoteIpEndPoint.Address + ":" + remoteIpEndPoint.Port;
            }
            else return "";
        }}
        //konstruktor pobiera gniazdo klienta oraz metody callbeckowe, które będę
        //wywoływane w głównym programie
        public ClientThread(Socket gniazdoKlienta,
            Action<string, ClientThread> OdbieranieWiadomosciCallback = null,
            Action<ClientThread> ZakonczPolaczenieCallback = null)
        {
            this.gniazdoKlienta = gniazdoKlienta;
            this.OdbieranieWiadomosciCallback = OdbieranieWiadomosciCallback;
            this.ZakonczPolaczenieCallback = ZakonczPolaczenieCallback;
            this.myDir = Path.GetFullPath("./");
        }
        //zamyka gniazdo i sygnalizuje do głównego wątku przez ZakonczPolaczenieCallback
        //że należy uwzględnić zamknięcie połączenia
        public void ZakonczPolaczenie()
        {
            Console.WriteLine(this.Name + ": zakończenie połączenia");
            koniec = true;
            //jeśli gniazdo byłoby już zamknięte, łapiemy wyjątek
            try {
                gniazdoKlienta.Shutdown(SocketShutdown.Both);
                gniazdoKlienta.Close();
                if (ZakonczPolaczenieCallback != null)
                    ZakonczPolaczenieCallback(this);
            }
            catch (Exception e){}
        }
        //sprawdza, czy połączenie jest otwarte a dokładnie, czy można czytać z gniazda
        public bool CzyPolaczony()
        {
            try
            {
                return !(gniazdoKlienta.Poll(1, SelectMode.SelectRead) 
                && gniazdoKlienta.Available == 0);
            }
            catch (SocketException) { return false; }
        }
        //wysyła wiadomość w postaci stringa
        //wysyłąnie jest thread safe, więc nie trzeba robić lock
        public void WyslijWiadomosc(String wiadomosc)
        {
            Console.WriteLine(this.Name + ": wysyłanie wiadomości " + wiadomosc);
            byte[] msg = Encoding.UTF8.GetBytes(wiadomosc);
            gniazdoKlienta.Send(msg);
        }

        public void WyslijWlasnaWiadomosc(){
            string wiadomosc = "";
            do{
                wiadomosc = Console.ReadLine();
            }
            while(wiadomosc == "" || wiadomosc == null);
            if(wiadomosc == "list"){
                PokazZawartoscKataloguDomowego();
            }
            if(wiadomosc.StartsWith("in")){
                string[] wiadomoscPodzielona = wiadomosc.Split(" ");
                if(wiadomoscPodzielona.Length == 2){
                    string nazwaPliku = wiadomoscPodzielona[1];
                    if(Directory.Exists(nazwaPliku)){
                        WyslijWiadomosc("in " + nazwaPliku);
                        this.myDir = Path.GetFullPath(nazwaPliku);
                        PokazZawartoscKataloguDomowego();
                    }
                    else{
                        WyslijWiadomosc("Nie ma takiego pliku");
                    }
                }
                else{
                    WyslijWiadomosc("Niepoprawna komenda");
                }
            }

            else{
                WyslijWiadomoscNieznane();
            }
        }

        public void WyslijWiadomoscNieznane(){
            WyslijWiadomosc("Nieznana komenda");
        }

        public void PokazZawartoscKataloguDomowego(){
            List<string> katalogi = Directory.GetFileSystemEntries(this.myDir).ToList();
            katalogi = katalogi.Select(x => Path.GetFileName(x)).ToList();
            katalogi.ForEach(x => WyslijWiadomosc(x));
        }

        //metoda wywoływana po odebraniu wiadomości, jeżeli callback
        //jest nienullowy wywołuje callback aby obsłużył odebranie wiadomości 
        public void OdbierzWiadomosc(string wiadomosc, ClientThread wt)
        {
            if (OdbieranieWiadomosciCallback != null)
                OdbieranieWiadomosciCallback(wiadomosc, wt);
            else//altrnatywna obsługa jeśli nie ma callbacka
            {
                //zrób coś :-)
            }
        }
        //startowanie obsługi połączenia, są dwa wątki: 
        // - wątek odbierający dane,
        // - wątek sprawdzający, czy połączenie jest otwarte
        // Każdy wątek jest w pętli, która kończy się po ustawienie koniec == true
        public void Start()
        {
            // dane przychodzące
            taskOdbieranie = new Task(() => 
            {
                string ?data = null;
                byte[] ?bytes = null;
                Console.WriteLine(this.Name + ": start wątku odbierającego dane");
                while (!koniec)
                {
                    //odbieramy max po 1024 bajty
                    bytes = new byte[1024];
                    int bytesRec = gniazdoKlienta.Receive(bytes);
                    //jeśli odebrano więcej niż 0 bajtów
                    if (bytesRec > 0)
                    {
                        //zmieniamy na UTF8
                        data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        //uruchamiamy metodę, która ma obsługę callbacku
                        OdbierzWiadomosc(data, this);
                    }
                }
            });
            //startujemy wątek odbierania danych
            taskOdbieranie.Start();
            //wątek spradzający, czy klient jest połączony
            Task sprawdzaczPolaczenie = new Task(() => 
            {
                while(!koniec)
                {
                    //jeśli nie jest, zamyka połączenie
                    if (!CzyPolaczony())
                        ZakonczPolaczenie();
                    Task.Delay(100).Wait();
                }
            });
            sprawdzaczPolaczenie.Start();
        }
    }
}
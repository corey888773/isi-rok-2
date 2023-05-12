// [4 punkty] Napisz program, który będzie monitorował w czasie rzeczywistym zmiany zachodzące w wybranym katalogu polegające 
// na usuwaniu lub dodawaniu do niego plików, ma monitorować również podkatalogi! Jeżeli w katalogu pojawi się nowy plik program
// ma wypisać: "dodano plik [nazwa pliku]" a jeśli usunięto plik "usunięto plik [nazwa pliku]". Program ma się zatrzymywać po
// wciśnięciu litery "q". Monitorowanie ma być w osobnym wątku niż oczekiwanie na wciśnięcie klawisza!

namespace lab5_odrabianie
{
    class Zadanie3{
        private readonly string path;
        private List<Thread> threads = new List<Thread>();

        public Zadanie3(string path)
        {
            this.path = path;
        }

        public void Run()
        {
            var monitorQKeyThread = new MonitorQKeyThread();
            var monitorPathThread = new MonitorPathThread(path);
            threads.Add(new Thread(monitorQKeyThread.Start));
            threads.Add(new Thread(monitorPathThread.Start));
        
            foreach (Thread t in threads)
            {
                t.Start();
            }
        }
    }

    class MonitorPathThread
    {
        private readonly string path;
        public ThreadStart? ThreadStart { get; private set; } = null; 

        public MonitorPathThread(string path)
        {
            this.path = path;
        }

        public void Start()
        {
            var watcher = new FileSystemWatcher(path);
            watcher.IncludeSubdirectories = true;
            watcher.Created += (sender, args) => Console.WriteLine($"dodano plik {args.Name}");
            watcher.Deleted += (sender, args) => Console.WriteLine($"usunięto plik {args.Name}");
            watcher.EnableRaisingEvents = true;
        }
    }

    class MonitorQKeyThread
    {
        public ThreadStart? ThreadStart { get; private set; } = null;
        public MonitorQKeyThread(){
        }

        public void Start()
        {
            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'q')
                {
                    break;
                }
            }
        }
    }
}

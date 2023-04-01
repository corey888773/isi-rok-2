// 3. [2 punkty] Napisz program, który począwszy od zadanego katalogu będzie wyszukiwał pliki, których nazwa będzie posiadała zadany napis 
// (podnapis, np. makaron.txt posiada "ron"). Wyszukiwanie ma brać pod uwagę podkatalogi. Wyszukiwanie ma odbywać się w wątku. 
// Kiedy wątek wyszukujący znajdzie plik pasujący do wzorca wątek główny ma wypisać nazwę tego pliku do konsoli (wątek wyszukujący
// ma nie zajmować się bezpośrednio wypisywaniem znalezionych plików do konsoli). 

namespace lab5{
    class Zadanie3{
        private readonly string path;
        private readonly string pattern;
        private readonly SearchThread searchThread;
        private readonly WriterThread writerThread;
        private List<Thread> threads = new List<Thread>();
        private Mutex acces = new Mutex();
        private List<string> Files = new List<string>();

        public Zadanie3(string path, string pattern){
            this.path = path;
            this.pattern = pattern;
            searchThread = new SearchThread();
            writerThread = new WriterThread();
        }

        public void Run(){
            var searchThread = new SearchThread{
                Path = path, 
                Pattern = pattern, 
                Acces = acces, 
                Files = Files,
            };
            var writerThread = new WriterThread{
                Acces = acces, 
                Files = Files,
            };
            threads.Add(new Thread(searchThread.Start));
            threads.Add(new Thread(writerThread.Start));
            foreach (Thread t in threads){
                t.Start();
            }
        }
    }

    class SearchThread{
        public string Path { get;  set; }
        public string Pattern { get;  set; }
        public ThreadStart? ThreadStart { get;  set; } = null;
        public Mutex? Acces { get;  set; } = null;
        public List<string> Files { get;  set; }

        public SearchThread(){
        }

        public void Start(){
            var files = Directory.GetFiles(Path, Pattern, SearchOption.AllDirectories);
            foreach (var file in files){
                Acces?.WaitOne();
                Files.Add(file);
                Acces?.ReleaseMutex();
            }
            
        }
    }

    class WriterThread{
        public Mutex? Acces { get;  set; } = null;
        public List<string> Files { get;  set; }

        public WriterThread(){

        }

        public void Start(){
            while (true){
                Acces?.WaitOne();
                if (Files?.Count > 0){
                    var file = Files[0];
                    Files.RemoveAt(0);
                    Console.WriteLine(file);
                }
                Acces?.ReleaseMutex();
            }
        }
    }
}
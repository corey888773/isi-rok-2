// 4. [2 punkty] Napisz program, który uruchomi n wątków i poczeka, aż wszystkie z tych wątków zaczną się wykonywać. Uruchomienie Thread.Start()
// nie jest równoznaczne z tym, że dany wątek zaczął się już wykonywać. Uznajmy, że wykonanie zaczyna się wtedy, kiedy wątek wykonał co najmniej
// jedną instrukcje w swoim kodzie. Kiedy wszystkie wątki zostaną uruchomione główny wątek ma o tym poinformować (wypisać informację do konsoli)
// a następnie zainicjować zamknięcie wszystkich wątków. Po otrzymaniu informacji, że wszystkie wątki zostaną zamknięte, główny program
// ma o tym poinformować oraz sam zakończyć działanie. 

namespace lab5{
    class Zadanie4
    {
        private readonly int threadsCount;
        private List<ThreadTest> threads = new List<ThreadTest>();

        public Zadanie4(int threadsCount)
        {
            this.threadsCount = threadsCount;
        }

        public void Run()
        {
            for (int i = 0; i < threadsCount; i++)
            {
                var thread = new ThreadTest();
                thread.Id = i;
                thread.ThreadStart = thread.Start;
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                new Thread(thread.ThreadStart!).Start();
            }

            if(threads.All(t => t.HasStarted)){
                Console.WriteLine("All threads started");
            }

            threads.ForEach(t => t.Running = false);

            if(threads.All(t => !t.Running)){
                Thread.Sleep(5000);
                Console.WriteLine("All threads stopped");
            }
        }

    }

    class ThreadTest
    {
        public ThreadStart? ThreadStart { get; set; } = null;
        public int Id { get; set; }
        public bool Running { get; set; } = true;
        public bool HasStarted {get; set;} = false;

        public ThreadTest()
        {
        }

        public void Start()
        {
            HasStarted = true;
            Console.WriteLine($"Thread {Id} started");
            while(Running){
                Thread.Sleep(1000);
            }
            Console.WriteLine($"Thread {Id} stopped");
        }
    }
}
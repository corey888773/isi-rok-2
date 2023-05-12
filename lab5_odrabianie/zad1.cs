// [2 punkty] Napisz program, który uruchomi n wątków (n ma być wczytane z klawiatury) i poczeka, aż wszystkie z tych wątków zaczną się wykonywać.
// Uruchomienie Thread.Start() nie jest równoznaczne z tym, że dany wątek zaczął się już wykonywać. Uznajmy, że wykonanie zaczyna się wtedy, kiedy
// wątek wykonał co najmniej jedną instrukcje w swoim kodzie. Kiedy wszystkie wątki zostaną uruchomione główny wątek ma o tym poinformować 
// (wypisać informację do konsoli) a następnie zainicjować zamknięcie wszystkich wątków. Po otrzymaniu informacji, że wszystkie wątki zostaną zamknięte,
// główny program ma o tym poinformować oraz sam zakończyć działanie.
using System;

namespace lab5_odrabianie{
    class Zadanie1
    {
        private int threadsCount;
        private List<ThreadTest> threads = new List<ThreadTest>();

        public Zadanie1()
        {
        }

        public void StartThreads(){
            foreach (var thread in threads)
            {
                new Thread(thread.ThreadStart!).Start();
            }
        }

        public void StopThreads(){
            threads.ForEach(t => t.Running = false);
        }

        public void Run()
        {
            bool isValid;
            string n;

            do
            {
                Console.WriteLine("Enter number of threads to run");
                n = Console.ReadLine();
                isValid = Int32.TryParse(n, out threadsCount);
                
                if (!isValid)
                {
                    Console.WriteLine("Invalid value");
                }

            } while (!isValid);

            Task.Run(() => MainRun()).Wait();
        }

        public async void MainRun(){
            for (int i = 0; i < threadsCount; i++)
            {
                var thread = new ThreadTest();
                thread.Id = i;
                thread.ThreadStart = thread.Start;
                threads.Add(thread);
            }

            // wait for all threads to start
            await Task.Run(() => StartThreads());
            Thread.Sleep(1000);

            if(threads.All(t => t.HasStarted)){
                Console.WriteLine("All threads started");
            }

            await Task.Run(() => StopThreads());
            Thread.Sleep(1000);

            if(threads.All(t => !t.Running)){
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
        {}

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
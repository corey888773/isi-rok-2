// 1. [4 punkty] Napisz program modelujący problem producent-konsument. Program ma uruchomić n wątków generujących dane oraz m wątków pobierających dane.
// Każdy z wątków ma  przechowywać informację o swoim numerze porządkowym, załóżmy, że są numerowane od 0..n-1 i odpowiednio od 0..m-1.
// Generowanie i odpowiednio odczytywanie danych przez każdy wątek ma odbywać się w losowych przedziałach czasu, które będą podawane jako parametr 
// dla danego wątku. Generowane dane mają być umieszczane na liście (lub innej strukturze), załóżmy, że dane to obiekty klasy,  które będą miały
// identyfikator informujący o numerze porządkowym wątku, który je wygenerował. Wątek pobierający dane pobiera i usuwa zawsze pierwszy element 
// ze struktury danych   i "zapamiętuje", jaki był identyfikator wątku producenta, który te dane tam umieścił. Program ma zatrzymywać wszystkie 
// wątki jeśli wciśniemy klawisz q i kończyć swoje działanie. Każdy zatrzymywany wątek ma wypisać ile "skonsumował" danych od poszczególnych producentów,
// np. Producent 0 - 4, Producent 1 - 5 itd.

namespace lab5{
    class Zadanie1
    {
        public List<int> data = new List<int>();
        private readonly int producersCount;
        private readonly int consumersCount;
        private List<Producer> producers = new List<Producer>();
        private List<Consumer> consumers = new List<Consumer>();

        public Zadanie1(int producersCount, int consumersCount)
        {
            this.producersCount = producersCount;
            this.consumersCount = consumersCount;
        }

        public void Run()
        {
            for (int i = 0; i < producersCount; i++)
            {
                var producer = new Producer();
                producer.Id = i;
                producer.Parent = this;
                producer.SleepTime = new Random().Next(1000, 5000);
                producers.Add(producer);
            }

            for (int i = 0; i < consumersCount; i++)
            {
                var consumer = new Consumer();
                consumer.Id = i;
                consumer.Parent = this;
                consumer.SleepTime = new Random().Next(1000, 5000);
                consumers.Add(consumer);
            }

            foreach (var producer in producers)
            {
                new Thread(producer.Start).Start();
            }

            foreach (var consumer in consumers)
            {
                new Thread(consumer.Start).Start();
            }

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    producers.ForEach(p => p.Stop = true);
                    consumers.ForEach(c => c.Stop = true);
                    break;
                }
            }

            foreach (var consumer in consumers)
            {
                Console.WriteLine($"Consumer {consumer.Id} consumed:");
                foreach (var (key, value) in consumer.ConsumedData)
                {
                    Console.WriteLine($"Producer {key} - {value}");
                }
            }
        }
    }

    class Producer
    {
        public int Id { get; set; }
        public Zadanie1 Parent { get; set; } = null!;
        public int SleepTime { get; set; }
        public bool Stop { get; set; }

        public Producer()
        {
        }

        public void Start()
        {
            while (!Stop)
            {
                GenerateData();
                Thread.Sleep(SleepTime);
            }
        }

        private void GenerateData()
        {
            Parent.data.Add(Id);
        }
    }

    class Consumer
    {
        public int Id { get; set; }
        public Zadanie1 Parent { get; set; } = null!;
        public int SleepTime { get; set; }
        public bool Stop { get; set; }

        public Dictionary<int, int> ConsumedData = new Dictionary<int, int>();

        public Consumer()
        {
        }

        public void Start()
        {
            while (!Stop)
            {
                ConsumeData();
                Thread.Sleep(SleepTime);
            }
        }
        
        private void ConsumeData()
        {
            if (Parent.data.Count > 0)
            {
                var data = Parent.data[0];
                Parent.data.RemoveAt(0);
                if (ConsumedData.ContainsKey(data))
                {
                    ConsumedData[data]++;
                }
                else
                {
                    ConsumedData.Add(data, 1);
                }
            }
        }
    }
}
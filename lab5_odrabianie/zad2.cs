// [4 punkty] Napisz program modelujący problem producent-konsument. Program ma uruchomić n wątków generujących dane oraz m wątków pobierających dane 
// (n i m pobieramy z klawiatury). Każdy z wątków ma przechowywać informację o swoim numerze porządkowym, załóżmy, że są numerowane od 0..n-1 i 
// odpowiednio od 0..m-1. Generowanie i odpowiednio odczytywanie danych przez każdy wątek ma odbywać się w losowych przedziałach czasu, które będą 
// podawane jako parametr dla danego wątku. Generowane dane mają być umieszczane na liście (lub innej strukturze), załóżmy, że dane to obiekty klasy, 
// które będą miały identyfikator informujący o numerze porządkowym wątku, który je wygenerował. Wątek pobierający dane pobiera i usuwa zawsze ostatni 
// element ze struktury danych i "zapamiętuje", jaki był identyfikator wątku producenta, który te dane tam umieścił. Programy producentów mają 
// zatrzymać się, kiedy wygenerują sumarycznie co najmniej 100 porcji danych. Programy konsumentów mają zatrzymywać się po zatrzymaniu programów 
// producentów, kiedy pobiorą wszystkie wyprodukowane przez producentów dane. Każdy zatrzymywany wątek konsumenta ma wypisać ile "skonsumował" 
// danych od poszczególnych producentów, np. Producent 0 - 4, Producent 1 - 5 itd. W tym momencie ma się również zakończyć główny program.

namespace lab5_odrabianie{
    class Zadanie2
    {
        public List<int> data = new List<int>();
        private readonly int producersCount;
        private readonly int consumersCount;
        private List<Producer> producers = new List<Producer>();
        private List<Consumer> consumers = new List<Consumer>();

        public Zadanie2(int producersCount, int consumersCount)
        {
            this.producersCount = producersCount;
            this.consumersCount = consumersCount;
        }

        public void Run()
        {
            for (int i = 0; i < producersCount; i++)
            {
                var producer = new Producer(){
                    Id = i,
                    Parent = this,
                    SleepTime = new Random().Next(1000, 5000)
                };
                producers.Add(producer);
            }

            for (int i = 0; i < consumersCount; i++)
            {
                var consumer = new Consumer(){
                    Id = i,
                    Parent = this,
                    SleepTime = new Random().Next(1000, 5000)
                };
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

            while(producers.Any(x => !x.Stop))
            {}

            while (data != null && data.Count != 0)
            {}

            foreach (var consumer in consumers)
            {
                consumer.StopThread();
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
        public Zadanie2 Parent { get; set; } = null!;
        public int SleepTime { get; set; }
        public bool Stop { get; set; }
        public int Counter { get; set; }

        public Producer()
        {
        }

        public void Start()
        {
            while (!Stop)
            {
                if(StopThread()){
                    break;
                }
                GenerateData();
                Thread.Sleep(SleepTime);

                StopThread();
            }
        }

        public bool StopThread()
        {
            if (Counter >= 3)
            {
                Stop = true;
                return true;
            }
            return false;
        }

        private void GenerateData()
        {
            Parent.data.Add(Id);
            Counter++;
            Console.WriteLine($"Producer {Id} generated data {Counter}");
        }
    }

    class Consumer
    {
        public int Id { get; set; }
        public Zadanie2 Parent { get; set; } = null!;
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

        public void StopThread(){
            Stop = true;
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
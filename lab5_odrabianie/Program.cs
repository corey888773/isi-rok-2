namespace lab5_odrabianie
{
    class Program
    {
        static void Main(string[] args)
        {
            // var Zadanie1 = new Zadanie1();
            // Zadanie1.Run();

            // var Zadanie2 = new Zadanie2(producersCount: 3, consumersCount: 4);
            // Zadanie2.Run();

            var Zadanie3 = new Zadanie3(path: Path.GetFullPath("./"));
            Zadanie3.Run();
        }
    }
}
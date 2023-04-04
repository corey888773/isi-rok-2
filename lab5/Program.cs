namespace lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            var zadanie1 = new Zadanie1(10, 10);
            zadanie1.Run();

            var zadanie2 = new Zadanie2(Path.GetFullPath("./"));
            zadanie2.Run();

            var zadanie3 = new Zadanie3(Path.GetFullPath("../"), "*.cs");
            zadanie3.Run();

            var zadanie4 = new Zadanie4(100);
            zadanie4.Run();
        }
    }
}
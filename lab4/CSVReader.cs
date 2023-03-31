namespace Main;

public class CSVReader<T>
{
    public List<T> ReadList(string path, Func<string[], T> generateClass)
    {
        List<T> list = new List<T>();
        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line?.Split(',');
                list.Add(generateClass(values));
            }
        }
        list.RemoveAt(0);
        return list;
    }
}
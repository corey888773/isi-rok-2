using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

class Program
{
    public static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        var filePath  = Path.GetFullPath("./dane.csv");

        (List<List<String>> data, List<String> headers) = readCSV(filePath, ',');
        List<(string, string, bool)> types = checkColumns(data, headers);

        string connectionString = "Data Source=xyz.sqlite";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            CreateTableFromData(types, "dane", connection);
            fillData(data, headers, "dane", connection);
            printTable("dane", connection);
        }
    }

    public static void print(List<List<String>> data, List<String> headers)
    {
        foreach (var header in headers)
        {
            Console.Write(header + " ");
        }
        Console.WriteLine();
        foreach (var row in data)
        {
            foreach (var value in row)
            {
                if (value == "")
                {
                    Console.Write("NULL ");
                }
                else
                {
                    Console.Write(value + " ");
                }
            }
            Console.WriteLine();
        }
    }

// Napisz metodę wczytującą dane z pliku CSV do dowolnej struktury danych (na przykład na List<List>). Metoda ma zwrócić tą strukturę danych oraz informację o nazwach kolumn - nazwy kolumn proszę wczytać z headera pliku. Metoda jako argument ma przyjmować nazwę pliku csv oraz separator dzielący od siebie poszczególne wartości w kolumnach. Zakładamy, że ten separator nie występuje nigdzie jako wartość kolumny, na przykład, jeśli separatorem jest przecinek, to przecinek nie występuje w kolumnach z napisami.
    public static (List<List<String>>, List<String>) readCSV(String filename, char separator)
    {
        List<List<String>> result = new List<List<String>>();
        List<String> headers = null;
        using (StreamReader sr = new StreamReader(filename))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] values = line.Split(separator);
                if (headers == null)
                {
                    headers = new List<String>(values);
                }
                else
                {
                    result.Add(new List<String>(values));
                }
            }
        }
        return (result, headers);
    }

// Napisz metodę, która jako parametr pobiera dane zwrócone przez metodę z punktu 1. Metoda na podstawie tych danych ma zwracać typy danych dla poszczególnych kolumn oraz czy kolumna może przyjmować wartości NULL czy też nie. Zakładamy, że jeśli kolumna nigdy nie ma wartości NULL, to nie może przyjmować wartości NULL. Jeżeli wszystkie pola tej kolumny można zrzutować na int, kolumna jest typu INTEGER, jeżeli kolumna nie jest typu INTEGER a wszystkie jej pola można zrzutować na double, to jest typu REAL. W pozostałym przypadku kolumna jest typu TEXT.
    public static List<(string, string, bool)> checkColumns(List<List<String>> data, List<String> headers)
    {
        var types = new List<(string, string, bool)>();
        for (int i = 0; i < headers.Count; i++)
        {
            string header = headers[i];
            bool canBeNull = false;
            String type = null;

            var column = data.Select(row => row[i]).ToList();

            if (column.Any(value => value == ""))
            {
                canBeNull = true;
            }
            if (column.All(value => int.TryParse(value, out _) || value == ""))
            {
                type = "INTEGER";
            }
            else if (column.All(value => double.TryParse(value, out _) || value == ""))
            {
                type = "REAL";
            }
            else
            {
                type = "TEXT";
            }

            types.Add((header, type.ToString(), canBeNull));
        }
        types.ForEach(x => Console.WriteLine("Column name: " + x.Item1 + ", DataType: " + x.Item2 + ", Nullable:" + x.Item3));

        return types;
    }

// Napisz metodę, która jako parametry przyjmuje dane zwrócone przez metodę z punktu 2, nazwę tabeli do utworzenia oraz obiekt klasy SqliteConnection. Metoda na podstawie danych ma utworzyć w bazie odpowiednią tabelę (o zdanych nazwach kolumn i typach) i nazwie zgodnej z przekazaną. Połączenie z bazą przekazywane jest w obiekcie SqliteConnection.
    public static void CreateTableFromData(List<(string, string, bool)> types, string name, SqliteConnection connection)
    {
        string dropTableQuery = "DROP TABLE IF EXISTS " + name + ";";
        using (SqliteCommand command = new SqliteCommand(dropTableQuery, connection))
        {
            command.ExecuteNonQuery();
        }
        string createTableQuery = "CREATE TABLE " + name + " (";
        foreach (var column in types)
        {
            createTableQuery += column.Item1 + " " + column.Item2;
            if (!column.Item3)
            {
                createTableQuery += " NOT NULL";
            }
            createTableQuery += ", ";
        }
        createTableQuery = createTableQuery.TrimEnd(',', ' ') + ");";
        
        using (SqliteCommand command = new SqliteCommand(createTableQuery, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine(createTableQuery);

    }

// Napisz metodę, która jako parametry przyjmuje dane, które mają znaleźć się w tabeli (dane zostały wczytane metodą z punktu 1), nazwę tabeli oraz obiekt klasy SqliteConnection. Metoda ma wypełnić tabelę utworzoną w punkcie 3 tymi danymi.
    public static void fillData(List<List<String>> data, List<String> headers, string tableName, SqliteConnection connection)
    {
        string insertQuery = "INSERT INTO " + tableName + " \n(";
        foreach (var header in headers)
        {
            insertQuery += header + ", ";
        }
        insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
        insertQuery += ") \nVALUES \n(";
        foreach (var row in data)
        {
            foreach (var value in row)
            {
                if (double.TryParse(value, out _))
                {
                    insertQuery += value + ", ";
                }
                else if (value == "")
                {
                    insertQuery += "NULL, ";
                }
                else
                {
                    insertQuery += '"' + value + '"' + ", ";
                }
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += "), \n(";
        }
        insertQuery = insertQuery.Substring(0, insertQuery.Length - 4) + ";";
        using (SqliteCommand command = new SqliteCommand(insertQuery, connection))
        {
            command.ExecuteNonQuery();
        }
        Console.WriteLine(insertQuery);
    }

    //Napisz metodę, która jako parametr przyjmuje nazwę tabeli oraz obiekt klasy SqliteConnection. Metoda przy pomocy kwerendy SELECT ma wypisać do konsoli wszystkie dane, które znajdują się w tej tabeli. Proszę wypisać również nazwy kolumn.
    public static void printTable(string tableName, SqliteConnection connection)
    {
        string selectQuery = "SELECT * FROM " + tableName + ";";
        using (SqliteCommand command = new SqliteCommand(selectQuery, connection))
        {
            Console.WriteLine(selectQuery);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i) + "\t");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i) + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
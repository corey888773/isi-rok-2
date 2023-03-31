using System;
using System.Globalization;
using System.Text.Json;
using System.Xml.Serialization;

namespace lab3
{
  public static class Program{
    public static void Main(string[] args){

      var source = ReadFromJson("favourite-tweets.jsonl");
      toXML(source, "tweets.xml");
      var tweets = fromXML("tweets.xml");

      var sortedByUserName = sortByUserName(tweets);
      var sortedByDate = sortByDate(tweets);
      printFirstAndLastTweet(tweets);
      var freqDict = createFreqDict(tweets);
      print10MostFrequentWordsWithLengthMoreThan5(freqDict);
      var idfDict = createIDFDict(tweets.Count, freqDict);
      print10MostIdf(idfDict);
    }

    private static List<Tweet> ReadFromJson(string file){
      string jsonString = File.ReadAllText(file);
      jsonString = "[\n" + jsonString.Replace("\n", ",\n") + "\n]";
      var source = JsonSerializer.Deserialize<List<Tweet>>(jsonString);
      return source;
    }

    static void toXML(List<Tweet> tweets, string outFileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
            using (TextWriter writer = new StreamWriter(outFileName))
            {
                serializer.Serialize(writer, tweets);
            }
        }

    static List<Tweet> fromXML(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
            using (TextReader reader = new StreamReader(fileName))
            {
                List<Tweet> tweets = (List<Tweet>)serializer.Deserialize(reader);
                return tweets;
            }
        }

      static List<Tweet> sortByUserName(List<Tweet> tweets){
        return tweets.OrderBy(tweet => tweet.UserName).ToList();
      }

      static List<Tweet> sortByDate(List<Tweet> tweets){
        return tweets.OrderBy(tweet => tweet.CreatedAtDate).ToList();
      }
      
      static void printFirstAndLastTweet(List<Tweet> tweets){
        tweets = sortByDate(tweets);
        Console.WriteLine(tweets.First().UserName);
        Console.WriteLine(tweets.Last().UserName);
      }

      static Dictionary<string, string> createDict(List<Tweet> tweets){
        var dict = new Dictionary<string, string>();
        foreach (var tweet in tweets)
        {
          dict.Add(tweet.UserName, tweet.Text);
        }
        return dict;
      }

static Dictionary<string, int> createFreqDict(List<Tweet> tweets)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();
            char[] charsToRemove = new char[] { '.', ',', '!', '?', ':', ';', '(', ')', '[', ']', '-', '/', '\\' };
            string text = "";

            foreach (Tweet tweet in tweets)
            {
                text = tweet.Text;
                foreach (var c in charsToRemove)
                {
                    text = text.Replace(c, ' ');
                }
                string[] wordsInTweet = tweet.Text.Split(new char[] {' ', '\t', '\n'}, StringSplitOptions.RemoveEmptyEntries);


                foreach (string word in wordsInTweet)
                {
                    if (words.ContainsKey(word))
                        words[word]++;
                    else
                        words.Add(word, 1);
                }
            }
        return words;
      }

      static void print10MostFrequentWordsWithLengthMoreThan5(Dictionary<string, int> words, int minLen = 5)
        {
            int i = 0;
            foreach (var word in words.OrderByDescending(key => key.Value).Where(key => key.Key.Length >= minLen).Take(10))
            {
                    Console.WriteLine(word.Key + " - " + word.Value);
                    i++; 
            }
            Console.WriteLine();
        }

      static Dictionary<string, double> createIDFDict(int tweetsLenght, Dictionary<string, int> dict){
        var idfDict = new Dictionary<string, double>();
        foreach (var item in dict)
        {
          idfDict.Add(item.Key, Math.Log(tweetsLenght / item.Value));
        }
        return idfDict;
        }
      
      static void print10MostIdf(Dictionary<string, double> words, int minLen = 5)
        {
            int i = 0;
            foreach (var word in words.OrderByDescending(key => key.Value).Where(key => key.Key.Length >= minLen).Take(10))
            {
              Console.WriteLine(word.Key + " - " + word.Value);
              i++; 
            }
            Console.WriteLine();
        }
    }
  }

  public class Tweet{
    public string Text { get; set; }
    public string UserName { get; set; }
    public string LinkToTweet {get; set;}
    public string FirstLinkUrl {get; set;}
    public string CreatedAt {get; set;}

    public DateTime CreatedAtDate{
      get{
        return ConvertDate(CreatedAt);
      }
    }
    public string TweetEmbedCode {get; set;}

    private DateTime _createdAt;

    public Tweet(){
      
    }

    private DateTime ConvertDate(string date){
      date = date.Replace("at ", "");
      return DateTime.ParseExact(date, "MMMM dd, yyyy hh:mmtt", CultureInfo.InvariantCulture);
    }
}



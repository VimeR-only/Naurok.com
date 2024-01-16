using System.Text.RegularExpressions;

namespace naurok.com.models
{
    public class Rootobject
    {
        public Session session { get; set; }
        public Settings settings { get; set; }
        public Document document { get; set; }
        public Question[] questions { get; set; }
    }

    public class Session
    {
        public int id { get; set; }
        public string? uuid { get; set; }
        public object? account_id { get; set; }
        public object? latest_question { get; set; }
        public int start_at { get; set; }
        public int answers { get; set; }
    }

    public class Settings
    {
        public int shuffle_question { get; set; }
        public int shuffle_options { get; set; }
        public int show_answer { get; set; }
        public int show_review { get; set; }
        public int show_leaderbord { get; set; }
        public int show_memes { get; set; }
    }

    public class Document
    {
        public int questions { get; set; }
    }

    public class Question
    {
        public string id { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public string point { get; set; }
        public string hint { get; set; }
        public string hint_penalty { get; set; }
        public string hint_description { get; set; }
        public string order { get; set; }
        public object clone_id { get; set; }
        public Option[] options { get; set; }
        public int questionLength { get; set; }
        public int optionMaxLength { get; set; }
        public bool optionHasImage { get; set; }

        public void PrintQuest()
        {
            foreach (var item in options)
            {
                Console.WriteLine($"{item.GetQuestion()}");
            }
            Console.WriteLine("\n");
        }

        public string GetQuestion()
        {
            Regex p = new Regex(@"<p>(.*?)<\/p>");
            Regex strong = new Regex(@"<\/?strong>");

            return strong.Replace(p.Replace(content, "$1"), "");
        }
    }

    public class Option
    {
        public string id { get; set; }
        public string question_id { get; set; }
        public string value { get; set; }
        public string image { get; set; }
        public object order { get; set; }

        public string GetQuestion()
        {
            Regex p = new Regex(@"<p>(.*?)<\/p>");
            Regex strong = new Regex(@"<\/?strong>");

            return strong.Replace(p.Replace(value, "$1"), "");
        }
    }

}

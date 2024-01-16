using System.Text.RegularExpressions;
using naurok.com.models;
using Newtonsoft.Json;

namespace naurok.com
{
    internal class Program
    {
        static async Task<string> GetTestId(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string htmlCode = await client.GetStringAsync(url);
                    string[] lines = htmlCode.Split('\n');

                    Regex regTest = new Regex(@"ng-app=""([^""]*)""");

                    foreach (var line in lines)
                    {
                        if (!regTest.Match(line).Success) continue;

                        Regex regId = new Regex(@"init\(\d+,\s*(\d+),\s*\d+\)");

                        Match Id = regId.Match(line);

                        if (Id.Success)
                        {
                            return Id.Groups[1].Value;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Опомилка під час виконання HTTP-запиту: {e.Message}.");
                    throw;
                }
            }
            return "";
        }

        static async Task<List<object>> GetQuest(string testid)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync("https://naurok.com.ua/api2/test/sessions/" + testid);

                    List<object> arrayQuest = new List<object>();
                    Rootobject? quest = JsonConvert.DeserializeObject<Rootobject>(json);

                    foreach (var data in quest.questions)
                    {
                        arrayQuest.Add(data.GetQuestion());

                        foreach (var item in data.options)
                        {
                            arrayQuest.Add(item.GetQuestion());
                        }
                        arrayQuest.Add(" ");
                    }

                    return arrayQuest;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Помилка під час виконання Get запиту JSON: {e.Message}.");
                }
            }
            return null;
        }

        static void PrintQuest(List<object> quest)
        {
            foreach (var item in quest)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\n");
        }

        static async Task Main()
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            string url;
            string testId;

            Console.WriteLine("Введіть URL теста");

            url = Console.ReadLine();

            testId = await GetTestId(url);

            if (testId == "")
            {
                Console.WriteLine("Помилка ID тесту не знайдено.");
                Console.ReadLine();
                return;
            }

            List<object> questList = new List<object>();
            questList = await GetQuest(testId);

            if (questList == null)
            {
                Console.WriteLine("Помилка масив з питанями пустий.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(" ");

            PrintQuest(questList);

            Console.ReadLine();
        }
    }
}
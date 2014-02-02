namespace ConsoleBookParserUI
{
    using System;
    using System.Collections.Generic;

    using BookParser;

    class Program
    {
        private const string exitConst = "Exit";

        private const string defaultFileResult = "result.xml";

        // TODO Перенести в настройки
        private const string webPage = "http://www.ozon.ru/context/detail/id/{0}";

        static void Main(string[] args)
        {
            Console.WriteLine("Для выхода введите '{0}'", Program.exitConst);

            var start_id = GetID("Введите начальный ID:");

            if (!start_id.HasValue)
            {
                return;
            }

            var finish_id = GetID("Введите конечный ID:");

            if (!finish_id.HasValue)
            {
                return;
            }

            if (start_id.Value > finish_id.Value)
            {
                Console.Write("ID начальный должен быть больше ID конечного");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Погнали");

            string processedPage = string.Empty;

            var webPages = new List<string>();

            for (int index = start_id.Value; index < finish_id.Value; index++)
            {
                webPages.Add(string.Format(Program.webPage, index));
            }

            var books = Program.Parse(webPages);

            Program.Save(books);
            Console.Write("Парсинг закончен");
            Console.ReadKey();

        }

        private static void Save(List<Book> books)
        {
            var factory = new FactoryBookSaver();
            var saver = factory.GetSaver();
            
            Console.WriteLine("Используется сохранитель {0}", saver.Name);

            saver.Save(books);
        }

        private static List<Book> Parse(List<string> webPages)
        {
            var factory = new FactoryBookParser();
            var parser = factory.GetParser();

            Console.WriteLine("Используется парсер {0}", parser.Name);

            return parser.Parse(webPages);
        }

        private static int? GetID(string text)
        {
            int? result = null;
            while (!result.HasValue)
            {
                Console.Write(text);
                var start_id_str = Console.ReadLine();

                if (start_id_str != null && start_id_str.ToLower() == Program.exitConst.ToLower())
                {
                    return null;
                }

                int id;
                if (int.TryParse(start_id_str, out id))
                {
                    result = id;
                }
                else
                {
                    Console.Write("ID должен быть целым числом");
                }
            }

            return result;
        }

    }
}

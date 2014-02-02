namespace ConsoleSimple
{
    using System;
    using System.Collections.Generic;

    using BookParser;

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new FactoryBookParser();
            var parser = factory.GetParser();
            
            Console.WriteLine("Используется парсер {0}", parser.Name);

            var factorySaver = new FactoryBookSaver();
            var saver = factorySaver.GetSaver();

            Console.WriteLine("Используется сохранитель {0}", saver.Name);

            var books = parser.Parse(new List<string> { "http://www.ozon.ru/context/detail/id/24763888/" });
            
            saver.Save(books);

            Console.ReadKey();
        }
    }
}

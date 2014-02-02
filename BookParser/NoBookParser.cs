namespace BookParser
{
    using System.Collections.Generic;

    internal class NoBookParser : IBookParser
    {
        public List<Book> Parse(List<string> webPages)
        {
            return new List<Book>();
        }

        public string Name
        {
            get
            {
                return "Пустой парсер";
            }
        }
    }
}
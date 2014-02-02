namespace BookParser
{
    using System.Collections.Generic;

    public class Book
    {
        public string Title { get; set; }

        public List<string> Autors { get; set; }

        public List<string> Translators { get; set; }

        public string Language { get; set; }

        public string PublishingHouse { get; set; }

        public string Series { get; set; }

        public string ISBN { get; set; }

        public decimal Price { get; set; }

        public Book()
        {
            this.Autors = new List<string>();
            this.Translators = new List<string>();
        }
    }
}

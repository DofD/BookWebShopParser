namespace XmlBookSaver
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using BookParser;

    [Serializable]
    public class BookXmlModel
    {
        public string Title;

        [XmlArrayItem( ElementName = "Autor")]
        public List<string> Autors { get; private set; }

        [XmlArrayItem("Translator")]
        public List<string> Translators { get; private set; }

        public string Language { get; set; }

        public string PublishingHouse { get; set; }

        public string Series { get; set; }

        public string ISBN { get; set; }

        public decimal Price { get; set; }

        public BookXmlModel()
        {
            this.Translators = new List<string>();
            this.Autors =new List<string>();
        }
        public BookXmlModel(Book book)
            :this()
        {
            this.Autors.AddRange(book.Autors);
            this.Translators.AddRange(book.Translators);

            this.Title = book.Title;
            this.Series = book.Series;
            this.PublishingHouse = book.PublishingHouse;
            this.Price = book.Price;
            this.Language = book.Language;
            this.ISBN = book.ISBN;
        }
    }
}
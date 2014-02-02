namespace XmlBookSaver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using BookParser;

    [Serializable]
    public class BookListXmlModel
    {
        [XmlArrayItem(ElementName = "Book")]
        public List<BookXmlModel> Books { get; private set; }

        public BookListXmlModel()
        {
            this.Books = new List<BookXmlModel>();
        }

        public BookListXmlModel(IEnumerable<Book> books)
            : this()
        {
            this.Books.AddRange(books.Select(x => new BookXmlModel(x)).ToArray());
        }
    }
}
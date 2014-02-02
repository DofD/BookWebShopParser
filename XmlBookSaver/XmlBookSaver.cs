namespace XmlBookSaver
{
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using BookParser;

    public class XmlBookSaver : IBookSaver
    {
        public string Name
        {
            get
            {
                return "сохраняем в XML";
            }
        }

        public void Save(List<Book> books)
        {
            var xmlBookList = new BookListXmlModel(books);

            // TODO сделать получение из файла настроек
            using (var writer = new XmlTextWriter("result.xml", Encoding.UTF8) { Formatting = Formatting.Indented })
            {
                    var ser = new XmlSerializer(typeof(BookListXmlModel));
                    ser.Serialize(writer, xmlBookList);
            }
        }
    }
}

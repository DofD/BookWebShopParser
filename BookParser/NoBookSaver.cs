namespace BookParser
{
    using System.Collections.Generic;

    internal class NoBookSaver : IBookSaver
    {
        public void Save(List<Book> books)
        {
        }

        public string Name
        {
            get { return "Пустой сохранитель"; }
        }
    }
}
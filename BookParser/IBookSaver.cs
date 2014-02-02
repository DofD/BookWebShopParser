namespace BookParser
{
    using System.Collections.Generic;

    public interface IBookSaver
    {
        string Name { get; }

        void Save(List<Book> books);
    }
}
namespace BookParser
{
    using System.Collections.Generic;

    public interface IBookParser
    {
        string Name { get; }

        List<Book> Parse(List<string> webPages);
    }
}
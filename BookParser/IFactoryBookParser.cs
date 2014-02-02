namespace BookParser
{
    public interface IFactoryBookParser
    {
        IBookParser GetParser();
    }
}
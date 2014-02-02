namespace BookParser
{
    public interface IFactoryBookSaver
    {
        IBookSaver GetSaver();
    }
}
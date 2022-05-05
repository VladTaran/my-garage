namespace MyGarage.Common;

public class PageOf<T> where T : class
{
    public IList<T> Items { get; set; }
    public int Total { get; set; }
}
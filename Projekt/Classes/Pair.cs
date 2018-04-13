namespace Projekt.Classes
{
    /// <summary>
    ///     Generic combination of two or more items
    /// </summary>
    /// <typeparam name="T1">Generic item 1</typeparam>
    /// <typeparam name="T2">Generic item 2</typeparam>
    public class Pair<T1, T2>
    {
        public Pair(T1 _item1, T2 _item2)
        {
            item1 = _item1;
            item2 = _item2;
        }

        public T1 item1 { get; set; }
        public T2 item2 { get; set; }
    }
}
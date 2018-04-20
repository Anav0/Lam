namespace Projekt.Classes
{
    public class Request
    {
        public Request()
        {
            NameType = "Unknown";
            Quantity = 0;
            StarChances = 0;
        }

        /// <summary>
        ///     Nazwa żądania np: txt
        /// </summary>
        public string NameType { get; set; }

        /// <summary>
        ///     Ilość wystąpień żądania w danej grupie
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Szansa że sesja rozpocznie się od tego zapytania
        /// </summary>
        public double StarChances { get; set; }

        /// <summary>
        ///     Ile sesji rozpoczyna się od tego żądania
        /// </summary>
        public int SessionStarts { get; set; }
    }
}
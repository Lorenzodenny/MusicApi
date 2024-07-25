namespace MusicApi.Model
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } // Elementi della pagina corrente
        public int TotalItems { get; set; } // Numero totale di elementi
        public int PageNumber { get; set; } // Numero della pagina corrente
        public int PageSize { get; set; } // Numero di elementi per pagina
    }

}

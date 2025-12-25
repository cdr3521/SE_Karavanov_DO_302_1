namespace lab2_11.Entity;

public class SearchForm
{
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            // Можна додати PropertyChanged якщо потрібно
        }
    }

    public SearchForm()
    {
        SearchText = string.Empty;
    }
}

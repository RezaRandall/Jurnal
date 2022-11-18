namespace TabpediaFin.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SearchableAttribute : Attribute
{
    public bool _isSearchable = false;
    public virtual bool IsSearchable
    {
        get { return _isSearchable; }
    }

    public SearchableAttribute()
    {
        _isSearchable = true;
    }

    public SearchableAttribute(bool isSearchable)
    {
        _isSearchable = isSearchable;
    }
}

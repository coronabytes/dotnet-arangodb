namespace Core.Arango.Linq.Interface
{
    public interface ICollectionPropertySetting
    {
        string CollectionName { get; set; }

        NamingConvention Naming { get; set; }
    }
}
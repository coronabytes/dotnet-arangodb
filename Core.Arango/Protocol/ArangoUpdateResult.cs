namespace Core.Arango.Protocol
{
    public class ArangoUpdateResult<T>
    {
        public string Id { get; set; }
        public string Key { get; set; }

        public T Old { get; set; }
        public T New { get; set; }
    }
}
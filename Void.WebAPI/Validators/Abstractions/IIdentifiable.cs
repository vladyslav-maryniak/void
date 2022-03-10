namespace Void.WebAPI.Validators.Abstractions
{
    public interface IIdentifiable<T>
    {
        public T Id { get; set; }
    }
}

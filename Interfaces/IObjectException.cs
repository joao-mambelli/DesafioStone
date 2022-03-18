namespace DesafioStone.Interfaces
{
    public interface IObjectException<T>
    {
        Exception Exception { get; set; }
        T Object { get; set; }
    }
}

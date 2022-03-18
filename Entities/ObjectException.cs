using DesafioStone.Interfaces;

namespace DesafioStone.Entities
{
    public class ObjectException<T> : IObjectException<T>
    {
        public Exception Exception { get; set; }
        public T Object { get; set; }

        public ObjectException(T obj, Exception ex)
        {
            Exception = ex;
            Object = obj;
        }

        public ObjectException(T obj)
        {
            Object = obj;
        }

        public ObjectException(Exception ex)
        {
            Exception = ex;
        }
    }
}

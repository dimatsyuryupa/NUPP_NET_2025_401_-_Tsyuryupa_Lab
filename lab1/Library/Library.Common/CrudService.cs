using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Common
{
    public interface ICrudService<T>
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }

    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly List<T> _data = new List<T>();

        public void Create(T element)
        {
            _data.Add(element);
        }

        public T Read(Guid id)
        {
            var prop = typeof(T).GetProperty("Id");
            return _data.FirstOrDefault(x => (Guid)prop.GetValue(x) == id);
        }

        public IEnumerable<T> ReadAll()
        {
            return _data;
        }

        public void Update(T element)
        {
            var prop = typeof(T).GetProperty("Id");
            Guid id = (Guid)prop.GetValue(element);

            var old = Read(id);
            if (old != null)
            {
                _data.Remove(old);
                _data.Add(element);
            }
        }

        public void Remove(T element)
        {
            _data.Remove(element);
        }
    }
}

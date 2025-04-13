namespace Schedule.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();        // Получить все записи
        T GetById(int id);              // Получить запись по идентификатору
        void Add(T entity);             // Добавить новую запись
        void Update(T entity);          // Обновить запись
        void Delete(T entity);          // Удалить запись
        void Save();                    // Сохранить изменения (при необходимости)
    }

}

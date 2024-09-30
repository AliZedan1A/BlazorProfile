namespace ServerSide.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? Get(int id);
        ReturnModel Update(T entity);
        ReturnModel Delete(int id);
        ReturnModel Insert(T entity);
        void SaveChanges();

    }
}

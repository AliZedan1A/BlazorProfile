using ServerSide.Database;

namespace ServerSide.Repository.Repositorys
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public ReturnModel Delete(int id)
        {
            var RetModel = new ReturnModel()
            {
                IsSucceeded = false,
                Comment = ""
            };
            T Entity = _context.Set<T>().Find(id);
            if (Entity == null)
            {
                RetModel.IsSucceeded = false;
                RetModel.Comment = "the user not found";
                return RetModel;
            }
            _context.Set<T>().Remove(Entity);
            try
            {
                SaveChanges();
                RetModel.IsSucceeded = true;
                RetModel.Comment = "User deleted successfully";
                return RetModel;
            }
            catch (Exception ex)
            {
                RetModel.IsSucceeded = false;
                RetModel.Comment = "Error during deletion: " + ex.Message;
                return RetModel;
            }

        }

        public T? Get(int id)
        {
            return _context.Set<T>().Find(id);

        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public ReturnModel Insert(T entity)
        {
            var RetModel = new ReturnModel()
            {
                IsSucceeded = false,
                Comment = ""
            };
            if (entity == null)
            {
                RetModel.Comment = "Entity is null";
                return RetModel;
            }
            _context.Set<T>().Add(entity);
            try
            {
                SaveChanges();
                RetModel.IsSucceeded = true;
                RetModel.Comment = $"User Inserted successfully";
                return RetModel;
            }
            catch (Exception ex)
            {
                RetModel.IsSucceeded = false;
                RetModel.Comment = "Error : " + ex.Message;
                return RetModel;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public ReturnModel Update(T entity)
        {
            var RetModel = new ReturnModel()
            {
                IsSucceeded = false,
                Comment = ""
            };
            if (entity == null)
            {
                RetModel.Comment = "Entity is null";
                return RetModel;
            }
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                SaveChanges();
                RetModel.IsSucceeded = true;
                RetModel.Comment = "User Updated successfully";
                return RetModel;
            }
            catch (Exception ex)
            {
                RetModel.IsSucceeded = false;
                RetModel.Comment = "Error : " + ex.Message;
                return RetModel;
            }
        }
    }
}

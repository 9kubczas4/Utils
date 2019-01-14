using Microsoft.EntityFrameworkCore;

namespace Utils.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> RegisterRepository<T>() where T : class, IEntity;
        void Save();
        DbContext DbContext { get; set; }
    }
}

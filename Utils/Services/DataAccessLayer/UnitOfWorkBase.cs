using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Utils.Interfaces;

namespace Utils.Services.DataAccessLayer
{
    public class UnitOfWorkBase : IUnitOfWork
    {
        private DbContext _context;
        private readonly Dictionary<Type, object> _repositoriesDict;
        private bool _disposed;

        public UnitOfWorkBase(DbContext context)
        {
            _context = context;
            _repositoriesDict = new Dictionary<Type, object>();
        }

        public IRepository<T> RegisterRepository<T>() where T : class, IEntity
        {
            if (_repositoriesDict.ContainsKey(typeof(T)))
                return _repositoriesDict[typeof(T)] as IRepository<T>;

            IRepository<T> repo = new GenericRepository<T>(_context);
            _repositoriesDict.Add(typeof(T), repo);
            return repo;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public DbContext DbContext
        {
            get { return _context; }
            set { _context = value; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

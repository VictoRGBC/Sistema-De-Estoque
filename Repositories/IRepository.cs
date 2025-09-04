﻿
namespace GerenciadorEstoque.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}

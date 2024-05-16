namespace Agenda.Domain.RepositoryInterfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    void Add(TEntity obj);

    TEntity GetById(Guid id);

    IEnumerable<TEntity> GetAll();

    void Update(TEntity obj);

    void Remove(Guid id);
}

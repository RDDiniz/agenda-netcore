using Agenda.Data.Context;
using Agenda.Domain.Models;
using Agenda.Domain.RepositoryInterfaces;

namespace Agenda.Data.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    private TarefaRepository _tarefaRepository;
    private ApplicationDbContext applicationContext;

    public UsuarioRepository(ApplicationDbContext appContext) : base(appContext)
    {
        applicationContext = appContext;
    }

    public Usuario GetByEmail(string email)
    {
        return DbSet.Where(x => x.Email == email).FirstOrDefault();
    }

    public ICollection<Tarefa> GetTasks(Guid usuarioId)
    {
        _tarefaRepository = new TarefaRepository(applicationContext);
        return _tarefaRepository.GetByUserId(usuarioId);
    }
}

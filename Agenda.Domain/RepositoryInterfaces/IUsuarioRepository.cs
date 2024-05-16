using Agenda.Domain.Models;

namespace Agenda.Domain.RepositoryInterfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Usuario GetByEmail(string email);

    ICollection<Tarefa> GetTasks(Guid usuarioId);
}

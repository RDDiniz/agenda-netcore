using Agenda.Domain.Models;
using System;
using System.Collections.Generic;

namespace Agenda.Domain.RepositoryInterfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario GetByEmail(string email);

        ICollection<Tarefa> GetTasks(Guid usuarioId);
    }
}
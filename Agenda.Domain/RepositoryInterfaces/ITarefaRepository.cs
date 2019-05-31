using Agenda.Domain.Models;
using System;
using System.Collections.Generic;

namespace Agenda.Domain.RepositoryInterfaces
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
        ICollection<Tarefa> GetByUserId(Guid usuarioId);

        IEnumerable<Tarefa> GetCompletedTasks(Guid usuarioId);

        IEnumerable<Tarefa> GetNoCompletedTasks(Guid usuarioId);

        bool UserIsValid(Guid usuarioId);
    }
}
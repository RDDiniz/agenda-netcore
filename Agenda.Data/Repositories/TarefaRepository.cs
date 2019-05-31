using Agenda.Data.Context;
using Agenda.Domain.Models;
using Agenda.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agenda.Data.Repositories
{
    public class TarefaRepository : Repository<Tarefa>, ITarefaRepository
    {
        UsuarioRepository _usuarioRepository;
        public TarefaRepository(ApplicationDbContext appContext)
            : base(appContext)
        {
            _usuarioRepository = new UsuarioRepository(appContext);
        }

        
        public ICollection<Tarefa> GetByUserId(Guid usuarioId)
        {
            return DbSet.Where<Tarefa>(x => x.UsuarioId == usuarioId).ToList();
        }

        public IEnumerable<Tarefa> GetCompletedTasks(Guid usuarioId)
        {
            return DbSet.Where<Tarefa>(x => x.UsuarioId == usuarioId && x.DataConclusao != null).ToList();
        }

        public IEnumerable<Tarefa> GetNoCompletedTasks(Guid usuarioId)
        {
            return DbSet.Where<Tarefa>(x => x.UsuarioId == usuarioId && x.DataConclusao == null).ToList();
        }

        public bool UserIsValid(Guid usuarioId)
        {
            var usuario = _usuarioRepository.GetById(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            return true;
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Domain.Models
{
    public partial class Usuario : Entity
    {
        public Usuario()
        {
            Tarefas = new List<Tarefa>();
        }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public virtual List<Tarefa> Tarefas { get; set; }
    }
}
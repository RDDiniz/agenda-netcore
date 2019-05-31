using System;
using System.ComponentModel.DataAnnotations;

namespace Agenda.Domain.Models
{
    public partial class Tarefa : Entity
    {
        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public DateTime Data { get; set; }

        public DateTime? DataConclusao { get; set; }

        public string PrevisaoTempo { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
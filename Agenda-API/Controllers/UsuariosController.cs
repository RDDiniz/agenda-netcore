using Agenda.Domain.Models;
using Agenda.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agenda_API.Controllers
{
    [Route("api/[Controller]")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetAll()
        {
            return Ok(_usuarioRepository.GetAll());
        }

        [HttpGet("{id:guid}", Name = "GetUsuario")]
        public ActionResult<Usuario> GetById(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }

            var usuario = _usuarioRepository.GetById(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpGet("{id:guid}/tasks")]
        public IActionResult GetTasks(Guid id)
        {
            try
            {
                var usuario = _usuarioRepository.GetById(id);
                var tarefas = _usuarioRepository.GetTasks(id);

                IEnumerable<Tarefa> listaTarefas = from tarefa in tarefas
                                                   select new Tarefa
                                                   {
                                                       Id = tarefa.Id,
                                                       Descricao = tarefa.Descricao,
                                                       Data = tarefa.Data,
                                                       PrevisaoTempo = tarefa.PrevisaoTempo,
                                                       DataConclusao = tarefa.DataConclusao
                                                   };

                usuario.Tarefas = listaTarefas.ToList();

                return new ObjectResult(usuario);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpGet("GetByEmail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }
            var usuario = _usuarioRepository.GetByEmail(email);

            return new ObjectResult(usuario);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _usuarioRepository.Add(usuario);
                return CreatedAtRoute("GetUsuario", new { id = usuario.Id }, usuario);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null || id == null || id == Guid.Empty || id != usuario.Id)
                {
                    return BadRequest();
                }

                var v_usuario = _usuarioRepository.GetById(id);
                if (v_usuario == null)
                {
                    return NotFound();
                }

                v_usuario.Nome = usuario.Nome;
                v_usuario.Email = usuario.Email;
                v_usuario.Senha = usuario.Senha;

                _usuarioRepository.Update(v_usuario);
                return new NoContentResult();
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest();
                }

                var v_usuario = _usuarioRepository.GetById(id);
                if (v_usuario == null)
                {
                    return NotFound();
                }

                _usuarioRepository.Remove(id);
                return new NoContentResult();
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }
    }
}
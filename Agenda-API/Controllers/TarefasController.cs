using Agenda.Domain.Models;
using Agenda.Domain.RepositoryInterfaces;
using Agenda.Services.External;
using Microsoft.AspNetCore.Mvc;

namespace Agenda_API.Controllers;

[Route("api/[Controller]")]
public class TarefasController : Controller
{
    private readonly ITarefaRepository _tarefaRepository;
    private WeatherService _weatherService;

    public TarefasController(ITarefaRepository tarefaRepository)
    {
        _tarefaRepository = tarefaRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Tarefa>> GetAll()
    {
        return Ok(_tarefaRepository.GetAll());
    }

    [HttpGet("{id:guid}", Name = "GetTask")]
    public ActionResult<Tarefa> GetById(Guid id)
    {
        if (id == null || id == Guid.Empty)
        {
            return BadRequest();
        }
        var tarefa = _tarefaRepository.GetById(id);

        if (tarefa == null)
        {
            return NotFound();
        }

        return Ok(tarefa);
    }

    [HttpGet("usuario/{id:guid}/CompletedTasks")]
    public IActionResult GetCompletedTasks(Guid id)
    {
        if (id == null || id == Guid.Empty)
        {
            return BadRequest();
        }

        var tarefa = _tarefaRepository.GetCompletedTasks(id);

        return new ObjectResult(tarefa);
    }

    [HttpGet("usuario/{id:guid}/NoCompletedTasks")]
    public IActionResult GetNoCompletedTasks(Guid id)
    {
        var tarefa = _tarefaRepository.GetNoCompletedTasks(id);

        return new ObjectResult(tarefa);
    }

    [HttpGet("GetByUserId/{id:guid}")]
    public IActionResult GetByUserId(Guid id)
    {
        if (id == null || id == Guid.Empty)
        {
            return BadRequest();
        }

        var tarefas = _tarefaRepository.GetByUserId(id);
        return new ObjectResult(tarefas);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Tarefa tarefa)
    {
        try
        {
            if (tarefa == null || tarefa.UsuarioId == null || tarefa.UsuarioId == Guid.Empty)
            {
                return BadRequest();
            }

            var usuarioValido = _tarefaRepository.UserIsValid(tarefa.UsuarioId);
            if (!usuarioValido)
            {
                return BadRequest();
            }

            _weatherService = new WeatherService();
            var previsaoTempo = _weatherService.Get(tarefa.Data.ToString());
            tarefa.PrevisaoTempo = previsaoTempo;

            _tarefaRepository.Add(tarefa);
            return CreatedAtRoute("GetTask", new { id = tarefa.Id }, tarefa);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] Tarefa tarefa)
    {
        try
        {
            if (tarefa == null || id == null || id == Guid.Empty || id != tarefa.Id)
            {
                return BadRequest();
            }

            var v_tarefa = _tarefaRepository.GetById(id);
            if (v_tarefa == null)
            {
                return NotFound();
            }

            _weatherService = new WeatherService();
            var previsaoTempo = _weatherService.Get(tarefa.Data.ToString());

            v_tarefa.Descricao = tarefa.Descricao;
            v_tarefa.Data = tarefa.Data;
            v_tarefa.PrevisaoTempo = previsaoTempo;
            v_tarefa.DataConclusao = tarefa.DataConclusao;

            _tarefaRepository.Update(v_tarefa);
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

            var v_tarefa = _tarefaRepository.GetById(id);
            if (v_tarefa == null)
            {
                return NotFound();
            }

            _tarefaRepository.Remove(id);
            return new NoContentResult();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}

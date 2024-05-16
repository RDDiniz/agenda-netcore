using Agenda.Data.Context;
using Agenda.Data.Repositories;
using Agenda.Domain.Models;
using Agenda.Domain.RepositoryInterfaces;
using Agenda.Tests.Framework;
using Agenda_API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Agenda.Tests;

public class TarefaControllerTest
{
    private TarefasController controller;
    private ITarefaRepository repository;
    private Guid tarefaId;

    public TarefaControllerTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseMySql(Constants.CONNECTION_STRING, ServerVersion.AutoDetect(Constants.CONNECTION_STRING));
        var context = new ApplicationDbContext(optionsBuilder.Options);

        repository = new TarefaRepository(context);
        controller = new TarefasController(repository);
    }

    [Fact]
    public void TestGetAll()
    {
        var okResult = controller.GetAll();

        Assert.IsType<OkObjectResult>(okResult.Result);
    }


    [Fact]
    public void TestGetByIdUnknownGuidReturnsNotFoundResult()
    {
        var notFoundResult = controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }


    [Fact]
    public void CreateInvalidObjectReturnsBadRequest()
    {
        var tarefa = new Tarefa()
        {
            UsuarioId = Guid.NewGuid(),
            Descricao = "Testes"
        };
        controller.ModelState.AddModelError("Data", "Required");

        var badResponse = controller.Create(tarefa);

        Assert.IsType<BadRequestResult>(badResponse);
    }

    [Fact]
    public void CreateValidObjectReturnsCreatedResponse()
    {

        var tarefa = new Tarefa()
        {
            UsuarioId = Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68"),
            Descricao = "Testes",
            Data = DateTime.Now
        };

        var createdResponse = controller.Create(tarefa);

        Assert.IsType<CreatedAtRouteResult>(createdResponse);
    }

    [Fact]
    public void CreateValidObjectReturnedResponseHasCreatedItem()
    {
        var tarefa = new Tarefa()
        {
            UsuarioId = Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68"),
            Descricao = "Testes",
            Data = DateTime.Now
        };

        var createdResponse = controller.Create(tarefa) as CreatedAtRouteResult;
        var item = createdResponse.Value as Tarefa;
        tarefaId = item.Id;

        Assert.IsType<Tarefa>(item);
        Assert.Equal(Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68"), item.UsuarioId);
    }

    [Fact]
    public void UpdateNotExistingGuidReturnsNotFoundResponse()
    {
        var notExistingGuid = Guid.NewGuid();

        var tarefa = new Tarefa()
        {
            Id = notExistingGuid,
            UsuarioId = Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68"),
            Descricao = "Testes",
            Data = DateTime.Now
        };

        var notFondResponse = controller.Update(notExistingGuid, tarefa);

        Assert.IsType<NotFoundResult>(notFondResponse);
    }

    [Fact]
    public void UpdateGuidEmptyReturnsBadRequestResponse()
    {
        var notExistingGuid = Guid.Empty;

        var tarefa = new Tarefa()
        {
            Id = notExistingGuid,
            UsuarioId = Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68"),
            Descricao = "Testes",
            Data = DateTime.Now
        };

        var badRequestResponse = controller.Update(notExistingGuid, tarefa);

        Assert.IsType<BadRequestResult>(badRequestResponse);
    }

    [Fact]
    public void UpdateGuidNotEqualTaskGuidReturnsBadRequestResponse()
    {
        var notExistingGuid = Guid.NewGuid();

        var tarefa = new Tarefa()
        {
            Id = Guid.NewGuid(),
            UsuarioId = Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68"),
            Descricao = "Testes",
            Data = DateTime.Now
        };

        var badRequestResponse = controller.Update(notExistingGuid, tarefa);

        Assert.IsType<BadRequestResult>(badRequestResponse);
    }

    [Fact]
    public void DeleteNotExistingGuidReturnsNotFoundResponse()
    {
        var notExistingGuid = Guid.NewGuid();

        var notFondResponse = controller.Delete(notExistingGuid);

        Assert.IsType<NotFoundResult>(notFondResponse);
    }

    [Fact]
    public void DeleteGuidEmptyReturnsBadRequestResponse()
    {
        var guidEmpty = Guid.Empty;

        var badResponse = controller.Delete(guidEmpty);

        Assert.IsType<BadRequestResult>(badResponse);
    }

    [Fact]
    public void DeleteExistingGuidReturnsOkResult()
    {
        var tarefaResponse = controller.GetByUserId(Guid.Parse("9042c02a-af97-47b3-278c-08d6e572fa68")) as ObjectResult;
        var tarefas = new List<Tarefa>((IEnumerable<Tarefa>)tarefaResponse.Value);
        var tarefa = tarefas.First();
        
        var response = controller.Delete(tarefa.Id);

        Assert.IsType<NoContentResult>(response);
    }
}

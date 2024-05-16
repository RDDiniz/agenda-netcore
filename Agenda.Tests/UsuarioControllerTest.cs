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

public class UsuarioControllerTest
{
    private UsuariosController controller;
    private IUsuarioRepository repository;

    public UsuarioControllerTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseMySql(Constants.CONNECTION_STRING, ServerVersion.AutoDetect(Constants.CONNECTION_STRING));
        var context = new ApplicationDbContext(optionsBuilder.Options);

        repository = new UsuarioRepository(context);
        controller = new UsuariosController(repository);
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
    public void TestGetByEmailExistingGuidReturnsOkResult()
    {
        var getResponse = controller.GetByEmail("admin@teste.com") as ObjectResult;
        var usuario = getResponse.Value as Usuario;
        var testGuid = usuario.Id;

        var okResult = controller.GetById(testGuid);

        Assert.IsType<OkObjectResult>(okResult.Result);
    }

    [Fact]
    public void CreateInvalidObjectReturnsBadRequest()
    {
        var usuario = new Usuario()
        {
            Nome = "Usuário de Teste",
            Senha = "123"
        };
        controller.ModelState.AddModelError("Email", "Required");

        var badResponse = controller.Create(usuario);

        Assert.IsType<BadRequestObjectResult>(badResponse);
    }

    [Fact]
    public void CreateValidObjectReturnsCreatedResponse()
    {
        var usuario = new Usuario()
        {
            Nome = "Usuário de Teste",
            Email = "teste@teste.com",
            Senha = "123"
        };

        var createdResponse = controller.Create(usuario);

        Assert.IsType<CreatedAtRouteResult>(createdResponse);
    }

    [Fact]
    public void CreateValidObjectReturnedResponseHasCreatedItem()
    {
        var usuario = new Usuario()
        {
            Nome = "Usuário de Teste de Retorno",
            Email = "teste.retorno@teste.com",
            Senha = "123"
        };

        var createdResponse = controller.Create(usuario) as CreatedAtRouteResult;
        var item = createdResponse.Value as Usuario;

        Assert.IsType<Usuario>(item);
        Assert.Equal("teste.retorno@teste.com", item.Email);
    }

    [Fact]
    public void UpdateNotExistingGuidReturnsNotFoundResponse()
    {
        var notExistingGuid = Guid.NewGuid();

        var usuario = new Usuario()
        {
            Id = notExistingGuid,
            Nome = "Usuário de Teste de Retorno",
            Email = "teste.retorno@teste.com",
            Senha = "12345"
        };

        var notFondResponse = controller.Update(notExistingGuid, usuario);

        Assert.IsType<NotFoundResult>(notFondResponse);
    }

    [Fact]
    public void UpdateGuidEmptyReturnsBadRequestResponse()
    {
        var notExistingGuid = Guid.Empty;

        var usuario = new Usuario()
        {
            Id = notExistingGuid,
            Nome = "Usuário de Teste de Retorno",
            Email = "teste.retorno@teste.com",
            Senha = "12345"
        };

        var badRequestResponse = controller.Update(notExistingGuid, usuario);

        Assert.IsType<BadRequestResult>(badRequestResponse);
    }

    [Fact]
    public void UpdateGuidNotEqualUserGuidReturnsBadRequestResponse()
    {
        var notExistingGuid = Guid.NewGuid();

        var usuario = new Usuario()
        {
            Id = Guid.NewGuid(),
            Nome = "Usuário de Teste de Retorno",
            Email = "teste.retorno@teste.com",
            Senha = "12345"
        };

        var badRequestResponse = controller.Update(notExistingGuid, usuario);

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
        var getResponse = controller.GetByEmail("teste.retorno@teste.com") as ObjectResult;
        var usuario = getResponse.Value as Usuario;

        var existingGuid = usuario.Id;

        var response = controller.Delete(existingGuid);

        Assert.IsType<NoContentResult>(response);
    }
}
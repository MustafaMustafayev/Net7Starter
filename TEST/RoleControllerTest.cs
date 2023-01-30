using API.Controllers;
using BLL.Abstract;
using DTO.Responses;
using DTO.Role;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TEST;

public class RoleControllerTest
{
    private readonly RoleController _controller;

    public RoleControllerTest()
    {
        Mock<IRoleService> mock = new();
        _controller = new RoleController(mock.Object);
    }

    [Fact]
    public async Task Get_For_ReturnsAllItems()
    {
        // Act
        var okResult = await _controller.Get() as OkObjectResult;

        // Assert
        var items = Assert.IsAssignableFrom<IDataResult<List<RoleToListDto>>>(okResult?.Value);
        Assert.Equal(3, items.Data?.Count);
    }

    [Fact]
    public async Task GetById_For_ReturnsNotFoundResult()
    {
        // Act
        var notFoundResult = await _controller.Get(1) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IDataResult<RoleToListDto>>(notFoundResult?.Value);
    }

    [Fact]
    public async Task GetById_For_ReturnsOkResult()
    {
        // Act
        var okResult = await _controller.Get(2) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IDataResult<RoleToListDto>>(okResult?.Value);
    }

    [Fact]
    public async Task GetById_For_ReturnsRightItem()
    {
        // Act
        var okResult = await _controller.Get(2) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IDataResult<RoleToListDto>>(okResult);
        Assert.Equal(2, (okResult?.Value as IDataResult<RoleToListDto>)?.Data!.RoleId);
    }

    [Fact]
    public async Task Add_For_ReturnsCreatedResponse()
    {
        // Arrange
        var testItem = new RoleToAddDto
        {
            Name = "TT",
            Key = "Test"
        };

        // Act
        var createdResponse = await _controller.Add(testItem) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IResult>(createdResponse?.Value);
    }

    [Fact]
    public async Task Remove_For_ReturnsNotFoundResponse()
    {
        // Act
        var badResponse = await _controller.Delete(2) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IResult>(badResponse?.Value);
    }

    [Fact]
    public async Task Remove_For_RemovesOneItem()
    {
        // Act
        var okResponse = await _controller.Delete(2) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IResult>(okResponse?.Value);
    }
}
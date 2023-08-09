using API.Controllers;
using BLL.Abstract;
using DTO.Permission;
using DTO.Responses;
using DTO.Role;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TEST;

public class RoleControllerTest
{
    private readonly RoleController _controller;
    private readonly Mock<IRoleService> _mock;

    public RoleControllerTest()
    {
        _mock = new Mock<IRoleService>();
        _controller = new RoleController(_mock.Object);
    }

    [Fact]
    public async Task Get_ReturnsAllItems()
    {
        // Arrange
        _mock.Setup(m => m.GetAsync())
            .ReturnsAsync(new SuccessDataResult<List<RoleToListDto>>(new List<RoleToListDto>()));

        // Act
        var okResult = await _controller.Get() as OkObjectResult;

        // Assert
        var items = Assert.IsAssignableFrom<IDataResult<List<RoleToListDto>>>(okResult?.Value);

        Assert.Equal(0, items.Data?.Count);
    }

    [Fact]
    public async Task GetById_ReturnsItem()
    {
        // Arrange
        _mock.Setup(m => m.GetAsync(1))
            .ReturnsAsync(
                new SuccessDataResult<RoleToListDto>(new RoleToListDto(1, "TT", "TT",
                    new List<PermissionToListDto>())));

        // Act
        var notFoundResult = await _controller.Get(1) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IDataResult<RoleToListDto>>(notFoundResult?.Value);
    }

    [Fact]
    public async Task Add_ReturnsSuccess()
    {
        // Arrange
        var roleToAdd = new RoleToAddDto("TT", "Test", new List<int>());
        _mock.Setup(m => m.AddAsync(roleToAdd)).ReturnsAsync(new SuccessResult());

        // Act
        var createdResponse = await _controller.Add(roleToAdd) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IResult>(createdResponse?.Value);
    }

    [Fact]
    public async Task Delete_ReturnsSuccess()
    {
        // Arrange
        _mock.Setup(m => m.SoftDeleteAsync(2)).ReturnsAsync(new SuccessResult());

        // Act
        var okResponse = await _controller.Delete(2) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IResult>(okResponse?.Value);
    }
}
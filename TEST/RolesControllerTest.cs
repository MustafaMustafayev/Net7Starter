using API.Controllers;
using BLL.Abstract;
using DTO.Permission;
using DTO.Responses;
using DTO.Role;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TEST;

public class RolesControllerTest
{
    private readonly RolesController _controller;
    private readonly Mock<IRoleService> _mock;

    public RolesControllerTest()
    {
        _mock = new Mock<IRoleService>();
        _controller = new RolesController(_mock.Object);
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
        _mock.Setup(m => m.GetAsync(Guid.NewGuid()))
            .ReturnsAsync(
                new SuccessDataResult<RoleToListDto>(new RoleToListDto() { Id = Guid.NewGuid(), Key = "TT", Name = "TT", Permissions = new List<PermissionToListDto>() }));

        // Act
        var notFoundResult = await _controller.Get(Guid.NewGuid()) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IDataResult<RoleToListDto>>(notFoundResult?.Value);
    }

    [Fact]
    public async Task Add_ReturnsSuccess()
    {
        // Arrange
        var roleToAdd = new RoleToAddDto() { Key = "TT", Name = "Test", PermissionIds = new List<Guid>() };
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
        _mock.Setup(m => m.SoftDeleteAsync(Guid.NewGuid())).ReturnsAsync(new SuccessResult());

        // Act
        var okResponse = await _controller.Delete(Guid.NewGuid()) as OkObjectResult;

        // Assert
        Assert.IsAssignableFrom<IResult>(okResponse?.Value);
    }
}
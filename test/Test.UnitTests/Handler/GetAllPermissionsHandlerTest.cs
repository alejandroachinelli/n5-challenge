using AutoMapper;
using Moq;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Application.Interfaces.Kafka;
using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Application.UseCases.Permission.Queries.GetAllPermissions;
using N5Challenge.Domain.Entities;

namespace Test.UnitTests.Handler;

public class GetAllPermissionsHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IKafkaProducerService> _kafkaMock = new();
    private readonly Mock<IElasticsearchService> _elasticsearchMock = new();

    [Fact]
    public async Task Handle_ShouldReturnPermissionsList()
    {
        // Arrange
        var permisos = new List<PermissionEntity>
        {
            new() {
                Id = 1,
                EmployeeName = "Juan",
                EmployeeLastName = "Pérez",
                PermissionTypeId = 1,
                PermissionDate = DateTime.Now,
                PermissionType = new PermissionTypeEntity { Id = 1, Description = "Administrador" }
            },
            new() {
                Id = 2,
                EmployeeName = "Ana",
                EmployeeLastName = "García",
                PermissionTypeId = 2,
                PermissionDate = DateTime.Now,
                PermissionType = new PermissionTypeEntity { Id = 2, Description = "Supervisor" }
            }
        };

        _unitOfWorkMock.Setup(x => x.PermissionRepository.GetAllAsync())
            .ReturnsAsync(permisos);

        var handler = new GetAllPermissionsHandler(
            _unitOfWorkMock.Object, 
            _mapperMock.Object,
            _kafkaMock.Object,
            _elasticsearchMock.Object);

        // Act
        var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
        Assert.Contains(result.Data, p => p.EmployeeName == "Juan");
        Assert.Contains(result.Data, p => p.EmployeeName == "Ana");
    }
}
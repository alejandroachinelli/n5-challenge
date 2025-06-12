using AutoMapper;
using Moq;
using N5Challenge.Application.DTO.DTOs.Kafka;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Application.Interfaces.Kafka;
using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Application.UseCases.Permission.Commands.ModifyPermission;
using N5Challenge.Domain.Entities;

namespace Test.UnitTests.Handler;

public class ModifyPermissionHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IKafkaProducerService> _kafkaMock = new();
    private readonly Mock<IElasticsearchService> _elasticsearchMock = new();

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPermissionExists()
    {
        // Arrange
        var command = new ModifyPermissionCommand(
            Id: 1,
            EmployeeName: "Pedro",
            EmployeeLastName: "Martínez",
            PermissionTypeId: 2,
            PermissionDate: DateTime.Now
        );

        var permisoExistente = new PermissionEntity
        {
            Id = 1,
            EmployeeName = "Original",
            EmployeeLastName = "Nombre",
            PermissionTypeId = 1,
            PermissionDate = DateTime.Now.AddDays(-1)
        };

        _unitOfWorkMock.Setup(x => x.PermissionRepository.GetByIdAsync(1))
            .ReturnsAsync(permisoExistente);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var handler = new ModifyPermissionHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaMock.Object,
            _elasticsearchMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Permiso modificado exitosamente", result.Message);
        _unitOfWorkMock.Verify(x => x.PermissionRepository.Update(It.IsAny<PermissionEntity>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _kafkaMock.Verify(k => k.ProduceAsync("permissions-topic", It.IsAny<KafkaMessageDto>()), Times.Once);
        _elasticsearchMock.Verify(e => e.IndexPermissionAsync(It.IsAny<PermissionEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPermissionDoesNotExist()
    {
        // Arrange
        var command = new ModifyPermissionCommand(1, "Juan", "Pérez", 2, DateTime.Now);

        _unitOfWorkMock.Setup(x => x.PermissionRepository.GetByIdAsync(1))
            .ReturnsAsync((PermissionEntity?)null);

        var handler = new ModifyPermissionHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaMock.Object,
            _elasticsearchMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Permiso no encontrado", result.Errors);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        _kafkaMock.Verify(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<KafkaMessageDto>()), Times.Never);
        _elasticsearchMock.Verify(e => e.IndexPermissionAsync(It.IsAny<PermissionEntity>()), Times.Never);
    }
}
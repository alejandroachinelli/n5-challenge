using MediatR;
using N5Challenge.Application.DTO.DTOs.Elasticsearch;
using N5Challenge.Application.DTO.DTOs.Kafka;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Application.Interfaces.Kafka;
using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Transversal.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.UseCases.Permission.Commands.DeletePermission;

public class DeletePermissionHandler : IRequestHandler<DeletePermissionCommand, Response<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IKafkaProducerService _kafka;
    private readonly IElasticsearchService _elasticsearch;

    public DeletePermissionHandler(IUnitOfWork unitOfWork, IKafkaProducerService kafka, IElasticsearchService elasticsearch)
    {
        _unitOfWork = unitOfWork;
        _kafka = kafka;
        _elasticsearch = elasticsearch;
    }

    public async Task<Response<string>> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<string>();

        try
        {
            var entity = await _unitOfWork.PermissionRepository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                response.IsSuccess = false;
                response.Message = "Permiso no encontrado";
                return response;
            }

            _unitOfWork.PermissionRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            await _kafka.ProduceAsync("permissions-topic", new KafkaMessageDto
            {
                Operation = "delete"
            });

            await _elasticsearch.IndexPermissionAsync(new PermissionElasticDto
            {
                Id = entity.Id,
                EmployeeName = entity.EmployeeName,
                EmployeeLastName = entity.EmployeeLastName,
                PermissionTypeId = entity.PermissionTypeId,
                PermissionTypeDescription = entity.PermissionType?.Description ?? "",
                PermissionDate = entity.PermissionDate
            });

            response.IsSuccess = true;
            response.Message = "Permiso eliminado exitosamente";
            response.Data = "OK";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Ocurrió un error al eliminar el permiso";
        }

        return response;
    }
}
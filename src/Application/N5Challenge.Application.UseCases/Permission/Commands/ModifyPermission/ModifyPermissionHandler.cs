using AutoMapper;
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

namespace N5Challenge.Application.UseCases.Permission.Commands.ModifyPermission;

public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, Response<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IKafkaProducerService _kafka;
    private readonly IElasticsearchService _elasticsearch;

    public ModifyPermissionHandler(IUnitOfWork unitOfWork, IMapper mapper, IKafkaProducerService kafka, IElasticsearchService elasticsearch)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _kafka = kafka;
        _elasticsearch = elasticsearch;
    }

    public async Task<Response<int>> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
    {
        var permiso = await _unitOfWork.PermissionRepository.GetByIdAsync(request.Id);

        if (permiso == null)
            return new Response<int> { IsSuccess = false, Errors = ["Permiso no encontrado"] };

        permiso.EmployeeName = request.EmployeeName;
        permiso.EmployeeLastName = request.EmployeeLastName;
        permiso.PermissionTypeId = request.PermissionTypeId;
        permiso.PermissionDate = request.PermissionDate;

        _unitOfWork.PermissionRepository.Update(permiso);
        await _unitOfWork.SaveChangesAsync();

        await _kafka.ProduceAsync("permissions-topic", new KafkaMessageDto
        {
            Operation = "modify"
        });

        await _elasticsearch.IndexPermissionAsync(new PermissionElasticDto
        {
            Id = permiso.Id,
            EmployeeName = permiso.EmployeeName,
            EmployeeLastName = permiso.EmployeeLastName,
            PermissionTypeId = permiso.PermissionTypeId,
            PermissionTypeDescription = permiso.PermissionType?.Description ?? "",
            PermissionDate = permiso.PermissionDate
        });

        return new Response<int>(permiso.Id, "Permiso modificado exitosamente");
    }
}
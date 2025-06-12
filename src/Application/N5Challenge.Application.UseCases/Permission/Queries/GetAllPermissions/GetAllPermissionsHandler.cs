using AutoMapper;
using MediatR;
using N5Challenge.Application.DTO.DTOs;
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

namespace N5Challenge.Application.UseCases.Permission.Queries.GetAllPermissions;

public class GetAllPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, Response<List<PermissionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IKafkaProducerService _kafka;
    private readonly IElasticsearchService _elasticsearch;

    public GetAllPermissionsHandler(IUnitOfWork unitOfWork, IMapper mapper, IKafkaProducerService kafka, IElasticsearchService elasticsearch)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _kafka = kafka;
        _elasticsearch = elasticsearch;
    }

    public async Task<Response<List<PermissionDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permisos = await _unitOfWork.PermissionRepository.GetAllAsync();
        var permisosDto = permisos.Select(p => new PermissionDto
        {
            Id = p.Id,
            EmployeeName = p.EmployeeName,
            EmployeeLastName = p.EmployeeLastName,
            PermissionTypeId = p.PermissionTypeId,
            PermissionTypeDescription = p.PermissionType?.Description ?? "",
            PermissionDate = p.PermissionDate
        }).ToList();

        await _kafka.ProduceAsync("permissions-topic", new KafkaMessageDto
        {
            Operation = "getall"
        });

        return new Response<List<PermissionDto>>(permisosDto);
    }
}
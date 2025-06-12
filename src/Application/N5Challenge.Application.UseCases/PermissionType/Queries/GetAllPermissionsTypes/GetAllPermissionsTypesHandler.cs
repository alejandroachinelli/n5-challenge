using AutoMapper;
using MediatR;
using N5Challenge.Application.DTO.DTOs;
using N5Challenge.Application.DTO.DTOs.Kafka;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Application.Interfaces.Kafka;
using N5Challenge.Application.Interfaces.Persistence;
using N5Challenge.Application.UseCases.Permission.Queries.GetAllPermissions;
using N5Challenge.Transversal.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.UseCases.PermissionType.Queries.GetAllPermissionsTypes
{
    public class GetAllPermissionsTypesHandler : IRequestHandler<GetAllPermissionsTypesQuery, Response<List<PermissionTypeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKafkaProducerService _kafka;
        private readonly IElasticsearchService _elasticsearch;

        public GetAllPermissionsTypesHandler(IUnitOfWork unitOfWork, IMapper mapper, IKafkaProducerService kafka, IElasticsearchService elasticsearch)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _kafka = kafka;
            _elasticsearch = elasticsearch;
        }

        public async Task<Response<List<PermissionTypeDto>>> Handle(GetAllPermissionsTypesQuery request, CancellationToken cancellationToken)
        {
            var tiposDePermisos = await _unitOfWork.PermissionTypeRepository.GetAllAsync();
            var tiposDePermisosDto = tiposDePermisos.Select(p => new PermissionTypeDto
            {
                Id = p.Id,
                Description = p.Description,
            }).ToList();

            await _kafka.ProduceAsync("permissions-type-topic", new KafkaMessageDto
            {
                Operation = "getall"
            });

            return new Response<List<PermissionTypeDto>>(tiposDePermisosDto);
        }
    }
}

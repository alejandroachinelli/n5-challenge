using N5Challenge.Application.DTO.DTOs.Elasticsearch;
using N5Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Application.Interfaces.ElasticSearch;

public interface IElasticsearchService
{
    Task IndexPermissionAsync(PermissionElasticDto permiso);
}
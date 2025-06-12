using Microsoft.Extensions.Configuration;
using N5Challenge.Application.DTO.DTOs.Elasticsearch;
using N5Challenge.Application.Interfaces.ElasticSearch;
using N5Challenge.Domain.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Infrastructure.Services.ElasticSearch;

public class ElasticsearchService : IElasticsearchService
{
    private readonly IElasticClient _client;

    public ElasticsearchService(IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["Elasticsearch:Uri"] ?? "http://localhost:9200"))
            .DefaultIndex("permissions");

        _client = new ElasticClient(settings);
    }

    public async Task IndexPermissionAsync(PermissionElasticDto permiso)
    {
        var response = await _client.IndexDocumentAsync(permiso);

        if (!response.IsValid)
        {
            throw new Exception($"Error al indexar en Elasticsearch: {response.DebugInformation}");
        }
    }
}
﻿using Docker.DotNet;
using Docker.DotNet.Models;
using SS14.MapServer.Configuration;

namespace SS14.MapServer.Services;

public sealed class ContainerService
{
    private readonly ContainerConfiguration _configuration = new();
    private readonly DockerClient _client;

    public ContainerService(IConfiguration configuration)
    {
        configuration.Bind("Container", _configuration);

        var clientConfiguration = _configuration.DockerHost != null
            ? new DockerClientConfiguration(_configuration.DockerHost) 
            : new DockerClientConfiguration();

        _client = clientConfiguration.CreateClient();
    }

    public async Task<VersionResponse> GetVersionInformation()
    {
        return await _client.System.GetVersionAsync();
    }
    
    public async Task<SystemInfoResponse> GetSystemInformation()
    {
        return await _client.System.GetSystemInfoAsync();
    }

    public async Task<string> BuildAndRun(string directory, List<string> command)
    {
        throw new NotImplementedException();
    }
}
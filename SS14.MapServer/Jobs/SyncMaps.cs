﻿using Microsoft.IdentityModel.Tokens;
using Quartz;
using Serilog;
using SS14.MapServer.MapProcessing;
using SS14.MapServer.MapProcessing.Services;
using SS14.MapServer.Services;

namespace SS14.MapServer.Jobs;

[DisallowConcurrentExecution]
public class SyncMaps : IJob
{
    public const string MapListKey = "Maps";
    public const string GitRefKey = "GitRef";

    private readonly ProcessQueue _processQueue;

    public List<string>? Maps { get; set; }
    public string GitRef { get; set; } = "master";

    public SyncMaps(ProcessQueue processQueue)
    {
        _processQueue = processQueue;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (Maps.IsNullOrEmpty())
            throw new JobExecutionException($"Job data value with key ${MapListKey} and type List<string> is missing");

        var processItem = new ProcessItem(GitRef, Maps!, value =>
            { Log.Debug("Finished processing maps for branch/commit {GitRef}. {MapIds}", value.GitRef, value.MapIds); });

        if (!await _processQueue.TryQueueProcessItem(processItem))
            throw new JobExecutionException("Failed to start map sync process. Process queue is full.");
    }
}

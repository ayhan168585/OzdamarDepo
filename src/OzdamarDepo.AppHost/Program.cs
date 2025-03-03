var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.OzdamarDepo_WebAPI>("ozdamardepo-webapi");

builder.Build().Run();

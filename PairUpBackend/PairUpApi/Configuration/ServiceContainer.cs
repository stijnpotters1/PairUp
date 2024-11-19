﻿using PairUpApi.Repositories;
using PairUpApi.Service;

namespace PairUpApi.Configuration;

public static class ServiceContainer
{
    public static IServiceCollection InstantiateServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // CORS Configuration
        services.ConfigureCustomCors();

        // Add controllers
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        // Add httpclient
        services.AddHttpClient();
        
        // Add swagger configuration 
        services.AddSwaggerGen();

        // Configure HTTPS redirection
        services.AddHttpsRedirection(options => { options.HttpsPort = 443; });

        // Database Configuration
        services.ConfigureDatabase(builder);
        
        // Automapper Configuration
        services.ConfigureMapping();
        
        //todo add scoped threading scrape service

        // Scoped custom services (Dependency injection for services and repositories)
        services.AddScoped<IService, ActivityService>();
        services.AddScoped<IActivityRepository, ActivityRepository>();

        return services;
    }
}
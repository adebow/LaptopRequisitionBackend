using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any; // Required for IOpenApiAny

namespace LaptopRequisition.Application.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Laptop Requisition API", Version = "v1" }); // Changed title to match project

                // Security definition for Bearer token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Authorization format : Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                // Configure enums
                c.UseInlineDefinitionsForEnums();
                ConfigureEnums(c);
            });

            return services;
        }

        private static void ConfigureEnums(SwaggerGenOptions options)
        {
            var enumTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsEnum);

            // Configure each enum type
            foreach (var enumType in enumTypes)
            {
                options.MapType(enumType, () => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(enumType)
                        .Select(name => new OpenApiString(name)) // Corrected: Pass string directly to OpenApiString constructor
                        .ToList<IOpenApiAny>()
                });
            }
        }
    }
}
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RiverLinkReporter.api
{
    public class SwaggerHelper
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.SwaggerDoc("v1", new Info
            {
                Title = "Identity Framework",
                Version = $"v1",
                Description = "An API for testing the Identity Framework"
            });

            //include the XML documentation
            swaggerGenOptions.DescribeAllEnumsAsStrings();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RiverLinkReporter.API.xml");
            swaggerGenOptions.IncludeXmlComments(filePath);
            //filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RiverLinkReporter.Models.xml");
            //swaggerGenOptions.IncludeXmlComments(filePath);

        }

        public static void ConfigureSwagger(SwaggerOptions swaggerOptions)
        {
            swaggerOptions.RouteTemplate = "api-docs/{documentName}/swagger.json";
        }

        public static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
        {
            swaggerUIOptions.SwaggerEndpoint($"/api-docs/v1/swagger.json", $"v1 Docs");
            swaggerUIOptions.RoutePrefix = "api-docs";
        }
    }
}

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerNetApi.Options
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (!type.IsEnum)
            {
                schema.Extensions.Add(
                    "x-ms-enum",
                    new OpenApiObject
                    {
                        ["name"] = new OpenApiString(type.Name),
                        ["modelAsString"] = new OpenApiBoolean(true)
                    }
                );
            };
        }
    }
}

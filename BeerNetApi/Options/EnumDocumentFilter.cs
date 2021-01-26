using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace BeerNetApi.Options
{
    public class EnumDocumentFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Components.Schemas.Remove("LoginModel");
            swaggerDoc.Components.Schemas.Remove("Country");
        }
    }
}

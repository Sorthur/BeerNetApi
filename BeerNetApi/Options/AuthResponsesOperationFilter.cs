using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BeerNetApi.Options
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> filter;

        public SecurityRequirementsOperationFilter (bool includeUnauthorizedAndForbiddenResponses = true, string securitySchemaName = "oauth2")
        {
            Func<IEnumerable<AuthorizeAttribute>, IEnumerable<string>> policySelector = authAttributes =>
                authAttributes
                    .Where(a => !string.IsNullOrEmpty(a.Policy))
                    .Select(a => a.Policy);

            filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(policySelector, includeUnauthorizedAndForbiddenResponses, securitySchemaName);
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Authorization parameter will be added only to Authorized methods
            foreach (var customAttribute in context.MethodInfo.CustomAttributes)
            {
                if (customAttribute.AttributeType == typeof(AuthorizeAttribute))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "Authorization",
                        Description = "Example: 'Bearer 12345abcdef'",
                        In = ParameterLocation.Header,
                        Required = false
                    });
                    break;
                }
            }
            filter.Apply(operation, context);
        }
    }
}
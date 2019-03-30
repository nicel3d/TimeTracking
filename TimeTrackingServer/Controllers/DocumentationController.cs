using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.CodeGeneration.OperationNameGenerators;
using NSwag.CodeGeneration.TypeScript;
using NSwag.SwaggerGeneration.WebApi;

namespace TimeTrackingServer.Controllers
{
    public class DocumentationController : Controller
    {
        private List<Type> _documentationControllerList = new List<Type> {
            typeof(UsersController),
            typeof(ActiviryStaffController),
            typeof(ValuesController)
        };

        [HttpGet]
        public async Task<object> TypeScript()
        {
            var swaggerSettings = new WebApiToSwaggerGeneratorSettings();
            var swaggerGenerator = new WebApiToSwaggerGenerator(swaggerSettings);

            IEnumerable<Type> controllerTypes = _documentationControllerList.ToList();

            var document = await swaggerGenerator.GenerateForControllersAsync(controllerTypes);
            var genSettings = new SwaggerToTypeScriptClientGeneratorSettings
            {
                ClassName = "WSApi",
            };

            var cgenerator = new SwaggerToTypeScriptClientGenerator(document, genSettings);
            cgenerator.BaseSettings.OperationNameGenerator = new SingleClientFromOperationIdOperationNameGenerator();
            var code = cgenerator.GenerateFile();

            return code;
        }
    }
}

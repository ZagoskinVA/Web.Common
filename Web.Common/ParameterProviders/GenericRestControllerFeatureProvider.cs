using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Web.Common.Controllers;
using WebUtilities.Model;

namespace Web.Common.ParameterProviders;

public class GenericRestControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var baseEntityType = typeof(BaseObject);
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
            .Where(p => baseEntityType.IsAssignableFrom(p) && p is {IsInterface: false, IsAbstract: false});
        foreach (var modelType in types)
        {

            var entityType = modelType.GetTypeInfo();
            Type[] typeArgs = {entityType};
            var controllerType =  typeof(CommonController<>).MakeGenericType(typeArgs).GetTypeInfo();
            feature.Controllers.Add(controllerType);
        }
    }
}
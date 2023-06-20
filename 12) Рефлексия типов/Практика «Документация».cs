using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    public string GetApiDescription()
    {
        var type = typeof(T);
        var attributes = type.GetCustomAttributes(typeof(ApiDescriptionAttribute), false);
        if (attributes.Length > 0)
        {
            var apiDescriptionAttribute = (ApiDescriptionAttribute)attributes[0];
            return apiDescriptionAttribute.Description;
        }
        return null;
    }

    //
    public string[] GetApiMethodNames()
    {
        var type = typeof(T);
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        var methodNames = methods
            .Where(m => m.GetCustomAttribute<ApiDescriptionAttribute>
            () != null && m.GetCustomAttribute<ApiMethodAttribute>() != null)
            .Select(m => m.Name)
            .ToArray();

        return methodNames;
    }

    //
    public string GetApiMethodDescription(string methodName)
    {
        var method = typeof(T).GetMethods().Where(s => s.Name == methodName).FirstOrDefault();
        if (method == null) return null;
        var attr = (ApiDescriptionAttribute)Attribute.GetCustomAttribute
            (method, typeof(ApiDescriptionAttribute));
        return attr?.Description;
    }
    //
    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);

        if (method == null)
        {
            return null;
        }

        var paramNames = method.GetParameters().Select(p => p.Name).ToArray();
        return paramNames;
    }


    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        return typeof(T)
            .GetMethods()
            ?.Where(s => s.Name == methodName)
            ?.FirstOrDefault()
            ?.GetParameters()
            ?.Where(s => s.Name == paramName)
            ?.Select(s => (ApiDescriptionAttribute)Attribute.GetCustomAttribute
            (s, typeof(ApiDescriptionAttribute)))
            ?.FirstOrDefault()
            ?.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription
        (string methodName, string paramName)
    {
        var param = typeof(T)
            .GetMethods()
            ?.Where(s => s.Name == methodName)
            ?.FirstOrDefault()
            ?.GetParameters()
            ?.Where(s => s.Name == paramName)
            ?.FirstOrDefault();
        var result = new ApiParamDescription();
        result.ParamDescription = new CommonDescription(paramName);
        if (param != null) FillParamDescription(result, param);
        return result;
    }
    //
    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);
        if (method == null || method.GetCustomAttribute<ApiMethodAttribute>() == null)
        {
            return null;
        }

        var result = new ApiMethodDescription
        {
            MethodDescription = new CommonDescription(methodName)
            {
                Description = method.GetCustomAttribute<ApiDescriptionAttribute>
                ()?.Description
            }
        };

        result.ParamDescriptions = GetApiParamDescriptions(method);
        result.ReturnDescription = GetApiReturnDescription(method);

        return result;
    }

    private ApiParamDescription[] GetApiParamDescriptions(MethodInfo method)
    {
        return method.GetParameters().Select(param =>
        {
            var paramDescription = new ApiParamDescription();
            FillParamDescription(paramDescription, param);
            return paramDescription;
        }).ToArray();
    }

    private ApiParamDescription GetApiReturnDescription(MethodInfo method)
    {
        if (method.ReturnType == typeof(void))
        {
            return null;
        }

        var returnDescription = new ApiParamDescription();
        FillParamDescription(returnDescription, method.ReturnParameter);
        return returnDescription;
    }



    private static ApiParamDescription FillParamDescription
        (ApiParamDescription desc, ParameterInfo param)
    {
        desc.ParamDescription = param.Name == "" ? new CommonDescription
            () : new CommonDescription(param.Name);

        if (param != null)
        {
            FillIntValidation(desc, param);
            FillRequired(desc, param);
            FillDescription(desc, param);
        }
        return desc;
    }

    private static void FillIntValidation(ApiParamDescription desc, ParameterInfo param)
    {
        var intValidationAttr = (ApiIntValidationAttribute)Attribute.GetCustomAttribute
            (param, typeof(ApiIntValidationAttribute));
        if (intValidationAttr != null)
        {
            desc.MinValue = intValidationAttr.MinValue;
            desc.MaxValue = intValidationAttr.MaxValue;
        }
    }

    private static void FillRequired(ApiParamDescription desc, ParameterInfo param)
    {
        var requiredAttr = (ApiRequiredAttribute)Attribute.GetCustomAttribute
            (param, typeof(ApiRequiredAttribute));
        if (requiredAttr != null)
        {
            desc.Required = requiredAttr.Required;
        }
    }

    private static void FillDescription(ApiParamDescription desc, ParameterInfo param)
    {
        var descAttr = (ApiDescriptionAttribute)Attribute.GetCustomAttribute
            (param, typeof(ApiDescriptionAttribute));
        if (descAttr != null)
        {
            desc.ParamDescription.Description = descAttr.Description;
        }
    }
}
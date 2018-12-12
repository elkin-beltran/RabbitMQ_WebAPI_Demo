using System;
using System.Reflection;

namespace RabbitMQ_WebAPI_Demo.Producer.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}
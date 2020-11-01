
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using HotChocolate;
using Microsoft.Extensions.Configuration;
using EMS.Club_Service.API.Context;
using EMS.Club_Service.API.Context.Model;
using EMS.Club_Service.API.GraphQlQueries;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.Configuration;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stripe;
using DirectiveLocation = HotChocolate.Types.DirectiveLocation;


namespace EMS.Club_Service.API
{


    public class ClubType
        : ObjectType<Club>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Club> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(t => t.Description).Directive(new DelegateDirective{
                Name = "Hej"
            });
            descriptor.Directive(new DelegateDirective()
            {
                Name = "Hej"
            });
            descriptor.AddValidationDirectives(null);
        }
    }

    public class DelegateAttribute
        : ObjectFieldDescriptorAttribute
    {
        public string Name;

        public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {

            Console.WriteLine(context);

            descriptor.Extend().OnBeforeCreate(configure =>
            {
                configure.Directives.Add(new DirectiveDefinition(new DelegateDirective()
                {
                    Name = "Hej"
                }));
                configure.ContextData["Test"] = "Hej";
                //configure.AddDirective(new DirectiveNode("delegateunique"));
            });
            //descriptor.Argument("test", argumentDescriptor => argumentDescriptor.Type<str>());
            descriptor.Directive(new DelegateDirective()
            {
                Name = "Hej"
            });
        }
        
    }


    public class DelegateDirectiveType
        : DirectiveType<DelegateDirective>
    {
        protected override void Configure(IDirectiveTypeDescriptor<DelegateDirective> descriptor)
        {
            descriptor.Name("delegateunique");
            descriptor.Location(DirectiveLocation.FieldDefinition).Location(DirectiveLocation.Object);
            descriptor.Repeatable();
        }
    }

    public class DelegateDirective
    {
        public string Name { get; set; }
    }

    public static class Extension
    {
        public static IObjectTypeDescriptor<T> AddValidationDirectives<T>(this IObjectTypeDescriptor<T> descriptor, IModelMetadataProvider metadataProvider)
        {
            descriptor
                .Extend()
                .OnBeforeCreate(d => AddValidationDirectives(d, metadataProvider))
                ;
            descriptor.Extend().OnBeforeCompletion((complete,objectType) => Configure(complete, objectType));
            return descriptor;
        }

        private static void Configure(ICompletionContext conf, ObjectTypeDefinition objectType)
        {
            Console.WriteLine(conf);
        }

        static void AddValidationDirectives(ObjectTypeDefinition definition, IModelMetadataProvider metadataProvider)
        {
            definition.Directives.Add(new DirectiveDefinition(new DelegateDirective()));
            definition.ContextData["Test2"] = "Hej2";
            foreach (var objectFieldDefinition in definition.Fields)
            {
                objectFieldDefinition.ContextData["Test"] = "Hej";
                objectFieldDefinition.AddDirective(new DirectiveNode("delegateunique"));
                objectFieldDefinition.Directives.Add(new DirectiveDefinition(new DelegateDirective()));
            }
        }
    }

    public class Startup : BaseStartUp<ClubContext>
    {

 
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        
        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder.AddDirectiveType<DelegateDirectiveType>()
                    .AddQueryType<ClubQueries>()
                .AddMutationType<ClubMutations>()
                .AddType<ClubType>();
        }

        protected override string GetName()
        {
            return "Club";
        }
    }
}

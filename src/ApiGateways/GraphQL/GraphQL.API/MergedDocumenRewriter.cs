using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Stitching.Delegation;
using HotChocolate.Stitching.Merge;
using Microsoft.Azure.Amqp.Serialization;

namespace EMS.GraphQL.API
{
    public class TypeDelegationExtension
    {
        public string NewFieldName { get; set; }
        public string QueryName { get; set; }
        public string ArgumentFiledName { get; set; }
    }

    public class MergedDocumenRewriter
    {
        private List<TypeDelegationExtension> extensions = new List<TypeDelegationExtension>();
        public MergedDocumenRewriter AddExtension(string newFieldName, string queryName, string argumentFieldName)
        {
            extensions.Add(new TypeDelegationExtension(){
                NewFieldName = newFieldName,
                ArgumentFiledName = argumentFieldName,
                QueryName = queryName
            });
            return this;
        }

        private ObjectTypeDefinitionNode queryDefinition;

        private string GetQuerySchema(FieldDefinitionNode query) => query.Directives.SingleOrDefault(directive => directive.Name.Value == "delegate")?.Arguments
            ?.SingleOrDefault(arg => arg.Name.Value == "schema")?.Value?.Value?.ToString();
        public DocumentNode MergedDocumentRewrite(DocumentNode mergedDocument)
        {
            var definitions = new List<IDefinitionNode>();
            queryDefinition = GetQueryDefinition(mergedDocument.Definitions);


            foreach (var definition in mergedDocument.Definitions)
            {

                if (definition is ObjectTypeDefinitionNode typeDefinition)
                {
                    //Add extensions
                    //fields = ExtendWith(fields, "club", "clubByID");
                    //ExtendWith(typeDefinition, "user", "user", "instructorId"); //Only support single argument delegation in object Query
                    foreach (var extend in extensions)
                    {
                        typeDefinition = ExtendWith(typeDefinition, extend.NewFieldName,extend.QueryName,extend.ArgumentFiledName); //Only support single argument delegation in object Query

                    }
                    definitions.Add(typeDefinition);
                }
                else
                {
                    definitions.Add(definition);
                }
            }

            return mergedDocument.WithDefinitions(definitions);
        }

        private ObjectTypeDefinitionNode GetQueryDefinition(IReadOnlyList<IDefinitionNode> nodes)
        {
            return nodes.OfType<ObjectTypeDefinitionNode>().SingleOrDefault(node => node.Name.Value == "Query");
        }
        
        private ObjectTypeDefinitionNode ExtendWith(ObjectTypeDefinitionNode typeDefinition, string newFieldName, string queryName, string argumentFieldName)
        {
            var field = typeDefinition.Fields.SingleOrDefault(field => field.Name.Value == argumentFieldName);
            if (field == null)
            {
                return typeDefinition;
            }

            var query = GetQueryForDelegation(queryName);
            var querySchema = GetQuerySchema(query);
            if (query == null || querySchema == null)
            {
                return typeDefinition;
            }

            var fieldSchema = typeDefinition.Directives.SingleOrDefault(directive => directive.Name.Value == "source")
                ?.Arguments.SingleOrDefault(arg => arg.Name.Value == "schema")
                ?.Value.Value.ToString();
            if (fieldSchema == querySchema)
            {
                return typeDefinition;
            }


            var newField = new FieldDefinitionNode(null, 
                new NameNode(newFieldName), 
                new StringValueNode("Automatically created delegation for "+ newFieldName), 
                field.Arguments, 
                query.Type, 
                field.Directives)
                .AddDelegationPath(querySchema, GetPathComponent(query, field));

            var fields = typeDefinition.Fields.ToList();
            fields.Add(newField);
            return typeDefinition.WithFields(fields);
        }

        private SelectionPathComponent GetPathComponent(FieldDefinitionNode query, FieldDefinitionNode field)
        {
            return new SelectionPathComponent(query.Name, GetQueryArguments(query, field));
        }

        private IReadOnlyList<ArgumentNode> GetQueryArguments(FieldDefinitionNode query, FieldDefinitionNode field)
        {
            return query.Arguments.Select(
                x => new ArgumentNode(x.Name, new ScopedVariableNode(
                    null,
                    new NameNode(ScopeNames.Fields),
                    field.Name))).ToList();
        }

        private FieldDefinitionNode GetQueryForDelegation(string name)
        {
            var query = queryDefinition.Fields.SingleOrDefault(query => query.Name.Value == name);
            return query;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Stitching.Delegation;
using HotChocolate.Stitching.Merge;

namespace EMS.GraphQL.API
{
    public abstract class HelperDelegateExtension : IDelegateExtension
    {
        protected SelectionPathComponent GetPathComponent(List<FieldDefinitionNode> argumentAsField)
        {
            return new SelectionPathComponent(delegationQuery.Name, GetQueryArguments(delegationQuery, argumentAsField));
        }

        private IReadOnlyList<ArgumentNode> GetQueryArguments(FieldDefinitionNode query, List<FieldDefinitionNode> argumentAsField)
        {
            List<ArgumentNode> args = new List<ArgumentNode>();
            for (int i = 0; i < argumentAsField.Count; i++)
            {
                args.Add(new ArgumentNode(query.Arguments[i].Name, new ScopedVariableNode(
                    null,
                    new NameNode(ScopeNames.Fields),
                    argumentAsField[i].Name)));
            }
            return args.AsReadOnly();
        }


        public FieldDefinitionNode CreateNewField(string name, List<FieldDefinitionNode> arguments)
        {
            return new FieldDefinitionNode(null,
                    new NameNode(name),
                    new StringValueNode("Automatically created delegation for " + name),
                    new List<InputValueDefinitionNode>().AsReadOnly(),
                    delegationQuery.Type,
                    new List<DirectiveNode>().AsReadOnly())
                .AddDelegationPath(GetQuerySchema(), GetPathComponent(arguments));
        }

        protected string GetQuerySchema() => delegationQuery.Directives.SingleOrDefault(directive => directive.Name.Value == "delegate")?.Arguments
            ?.SingleOrDefault(arg => arg.Name.Value == "schema")?.Value?.Value?.ToString();

        protected string GetObjectTypeSchema(ObjectTypeDefinitionNode type) => type.Directives.SingleOrDefault(directive => directive.Name.Value == "delegate")?.Arguments
            ?.SingleOrDefault(arg => arg.Name.Value == "schema")?.Value?.Value?.ToString();
    }
}
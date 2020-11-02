using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using HotChocolate.Language;

namespace EMS.GraphQL.API
{

    public abstract class IDelegateExtension
    {
        protected FieldDefinitionNode delegationQuery;

        public virtual ObjectTypeDefinitionNode GetQueryDefinition(IReadOnlyList<IDefinitionNode> nodes)
        {
            return nodes.OfType<ObjectTypeDefinitionNode>().SingleOrDefault(node => node.Name.Value == "Query");
        }

        public List<IDefinitionNode> Execute(List<IDefinitionNode> objectTypes)
        {
            var definitions = new List<IDefinitionNode>();
            var queries = GetQueryDefinition(objectTypes);
            if (queries == null)
            {
                throw new Exception("No object have the expected query definition");
            }
            delegationQuery = DelegationQuery(queries);
            if (delegationQuery == null)
            {
                throw new Exception("Query for delegation does not exist");
            }


            foreach (var definition in objectTypes)
            {
                if (definition is ObjectTypeDefinitionNode typeDefinition)
                {
                    if (Applyable(typeDefinition, delegationQuery))
                    {
                        typeDefinition = Delegate(typeDefinition, delegationQuery);
                    }
                    definitions.Add(typeDefinition);
                }
                else
                {
                    definitions.Add(definition);
                }
            }

            return definitions;
        }

        public abstract ObjectTypeDefinitionNode Delegate(ObjectTypeDefinitionNode currentObjectTypeDefinition, FieldDefinitionNode delegationQuery);

        public abstract bool Applyable(ObjectTypeDefinitionNode objectTypeDefinition, FieldDefinitionNode queryDefinition);

        public abstract FieldDefinitionNode DelegationQuery(ObjectTypeDefinitionNode queries);


    }
}
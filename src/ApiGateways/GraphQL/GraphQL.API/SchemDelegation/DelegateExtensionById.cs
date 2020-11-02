using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Stitching.Delegation;
using HotChocolate.Stitching.Merge;

namespace EMS.GraphQL.API
{
    public class DelegateExtensionById : HelperDelegateExtension
    {
        private readonly string NewFieldName;
        private readonly string _queryName;
        private readonly string[] _arguments;
        private List<FieldDefinitionNode> _argumentAsField;

        public DelegateExtensionById(string newFieldName, string queryName, params string[] argumentsAsString)
        {
            NewFieldName = newFieldName;
            _queryName = queryName;
            _arguments = argumentsAsString;
        }
        
        public override bool Applyable(ObjectTypeDefinitionNode objectTypeDefinition, FieldDefinitionNode queryDefinition)
        {
            _argumentAsField = new List<FieldDefinitionNode>();
            foreach (var argument in _arguments)
            {
                var field = objectTypeDefinition.Fields.SingleOrDefault(field => field.Name.Value == argument);
                if (field == null)
                {
                    return false;
                }
                _argumentAsField.Add(field);
            }

            var fieldSchema = GetObjectTypeSchema(objectTypeDefinition);

            if (GetQuerySchema() == fieldSchema)
            {
                return false;
            }

            return true;
        }

        public override FieldDefinitionNode DelegationQuery(ObjectTypeDefinitionNode queries)
        {
            return queries.Fields.SingleOrDefault(query => query.Name.Value == _queryName);
        }

        public override ObjectTypeDefinitionNode Delegate(ObjectTypeDefinitionNode currentObjectTypeDefinition, FieldDefinitionNode delegationQuery)
        {
            var newfields = currentObjectTypeDefinition.Fields.ToList();
            newfields.Add(CreateNewField(NewFieldName, _argumentAsField));
            return currentObjectTypeDefinition.WithFields(newfields);
        }
    }
}
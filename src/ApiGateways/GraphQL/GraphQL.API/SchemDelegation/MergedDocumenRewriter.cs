using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using Microsoft.Azure.Amqp.Serialization;

namespace EMS.GraphQL.API
{
    public class MergedDocumenRewriter
    {
        private List<IDelegateExtension> extensions = new List<IDelegateExtension>();
        public MergedDocumenRewriter AddExtension(IDelegateExtension extension)
        {
            extensions.Add(extension);
            return this;
        }
        
        public DocumentNode MergedDocumentRewrite(DocumentNode mergedDocument)
        {
            var definitions = mergedDocument.Definitions.ToList();

            foreach (var extend in extensions)
            {
                definitions = extend.Execute(definitions); //Only support single argument delegation in object Query
            }

            return mergedDocument.WithDefinitions(definitions);
        }
    }
}
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;

namespace DynamicOdata.Service
{
    public interface IDataService
    {
        EdmEntityObjectCollection Get(IEdmCollectionType collectionType, ODataQueryOptions queryOptions);

        EdmEntityObject Get(string key, IEdmEntityType entityType);

        int Count(IEdmCollectionType collectionType, ODataQueryOptions queryOptions);
    }
}

using Microsoft.OData.Edm;
using Microsoft.OData.Edm;

namespace DynamicOdata.Service
{
    public interface IEdmModelBuilder
    {
        EdmModel GetModel();
    }
}

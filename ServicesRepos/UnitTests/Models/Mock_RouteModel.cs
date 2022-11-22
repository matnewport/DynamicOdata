using Shared.Kernel.Interfaces;
using System.ComponentModel;

namespace UnitTests
{
    public sealed class Mock_RouteModel : IRouteModel
    {
        [Description("RouteId")]
        public string RouteId { get; set; }
    }
}

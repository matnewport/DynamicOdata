using Shared.Kernel.Interfaces;
using System.ComponentModel;

namespace Tamp.Repositories.Models
{
    public sealed class RouteModel : IRouteModel
    {
        [Description("RouteId")]
        public string RouteId { get; set; }
    }
}

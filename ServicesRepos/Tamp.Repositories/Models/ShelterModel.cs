using Dapper;
using GenericCrud;
using Shared.Kernel.Interfaces;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Tamp.Repositories.Models
{
    [Table("Shelter")]
    public sealed class ShelterModel : IShelterModel, IHasId<int>
    {
        [Key]
        [Description("ShelterId")]
        public int Id { get; set; }
    }
   
}

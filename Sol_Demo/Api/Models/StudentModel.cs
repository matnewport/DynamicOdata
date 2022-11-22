using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Api.Models
{
    [DataContract]
    //[Page(MaxTop = 100)]
    public class StudentModel
    {
        [DataMember(EmitDefaultValue = false)]
        public String Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FullName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int Score { get; set; }
    }
}
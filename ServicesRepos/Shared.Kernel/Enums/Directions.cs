using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Shared.Kernel.Enums
{
    //[DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Directions
    {
        [EnumMember(Value = "North")]
        North = 0,

        [EnumMember(Value = "South")]
        South = 1,

        [EnumMember(Value = "East")]
        East = 2,

        [EnumMember(Value = "West")]
        West = 3
    }
}
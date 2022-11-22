using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Shared.Kernel.Enums
{
    //[DataContract]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    //[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))] //null
    //[Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumMemberConverter))] 0/1
    // [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    //[System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))] //0/1
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PickType
    {
        [EnumMember(Value = "CurrentPick")]
        CurrentPick = 1,

        [EnumMember(Value = "NextPick")]
        NextPick = 2


    }
}

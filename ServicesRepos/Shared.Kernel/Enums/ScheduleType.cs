using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Shared.Kernel.Enums
{
    //[DataContract]
    public enum ScheduleType
    {
        Weekday = 0,

        Saturday = 5,

        Sunday = 6,

        Holiday = 7,
        Reduced = 21,
    }
}
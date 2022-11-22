using Shared.Kernel.Interfaces;
using System.Runtime.Serialization;

namespace Tamp.Repositories.Models
{
    [DataContract]
    public sealed class PaddlePrintModel : IPaddlePrintModel
    {
        [DataMember]
        public int Idx { get; set; }

        [DataMember]
        public int DutyNumber { get; set; }

        [DataMember]
        public string Effective_Date { get; set; }

        [DataMember]
        public string ScheduleTypeName { get; set; }

        public string BlockNumber { get; set; }

        [DataMember]
        public string Vehicle_Type { get; set; }

        [DataMember]
        public string Block_Display { get; set; }

        public int InternalBlockNumber { get; set; }

        [DataMember]
        public int RouteID { get; set; }

        [DataMember]
        public string RouteText { get; set; }

        [DataMember]
        public string Terminal { get; set; }

        [DataMember]
        public string DirectionID { get; set; }

        [DataMember]
        public string DirectionText { get; set; }

        [DataMember]
        public int TripNumber { get; set; }

        [DataMember]
        public string TripNumText { get; set; }

        [DataMember]
        public string TripTypeName { get; set; }

        [DataMember]
        public int TripNumSort { get; set; }

        [DataMember]
        public string TripPatternID { get; set; }

        [DataMember]
        public string FromToVia { get; set; }

        [DataMember]
        public string ViaVariant { get; set; }

        [DataMember]
        public string VariantDescription { get; set; }

        [DataMember]
        public string PlaceSequence { get; set; }

        [DataMember]
        public int StopSequence { get; set; }

        [DataMember]
        public string StopID { get; set; }

        [DataMember]
        public string AlternateName { get; set; }

        [DataMember]
        public string DriverText { get; set; }

        [DataMember]
        public string PublicText { get; set; }

        [DataMember]
        public int PassingTime { get; set; }

        [DataMember]
        public string Formatted_Passing_Time { get; set; }

        [DataMember]
        public string PieceReportPlace { get; set; }

        [DataMember]
        public int PieceReportTime { get; set; }

        [DataMember]
        public int PieceReportTimeCalc { get; set; }

        [DataMember]
        public string Formatted_Piece_Report_Time { get; set; }

        [DataMember]
        public string PieceStartPlace { get; set; }

        [DataMember]
        public int PieceStartTime { get; set; }

        [DataMember]
        public string Formatted_Piece_Start_Time { get; set; }

        [DataMember]
        public string PieceEndPlace { get; set; }

        [DataMember]
        public int PieceEndTime { get; set; }

        [DataMember]
        public string Formatted_Piece_End_Time { get; set; }

        [DataMember]
        public string Platform_Time { get; set; }

        [DataMember]
        public string Spread_Time { get; set; }

        [DataMember]
        public string PieceClearPlace { get; set; }

        [DataMember]
        public int PieceClearTime { get; set; }

        [DataMember]
        public string Formatted_Piece_Clear_Time { get; set; }

        [DataMember]
        public int Total_Plat_1 { get; set; }

        [DataMember]
        public int Total_Spread_1 { get; set; }

        [DataMember]
        public int TripIndex { get; set; }

        [DataMember]
        public string TripText { get; set; }

        [DataMember]
        public string TripStopText { get; set; }

        [DataMember]
        public string InstructionText { get; set; }
    }
}
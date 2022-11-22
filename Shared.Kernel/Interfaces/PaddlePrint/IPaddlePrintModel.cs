using System.Runtime.Serialization;

namespace Shared.Kernel.Interfaces
{
    public interface IPaddlePrintModel
    {
        int Idx { get; set; }
        string AlternateName { get; set; }
        int TripNumSort { get; set; }
        string BlockNumber { get; set; }
        string DirectionID { get; set; }
        string DirectionText { get; set; }
        string DriverText { get; set; }
        int DutyNumber { get; set; }
        string Effective_Date { get; set; }
        string Formatted_Passing_Time { get; set; }
        string Formatted_Piece_Clear_Time { get; set; }
        string Formatted_Piece_End_Time { get; set; }
        string Formatted_Piece_Report_Time { get; set; }
        string Formatted_Piece_Start_Time { get; set; }
        string FromToVia { get; set; }
        int InternalBlockNumber { get; set; }
        int PassingTime { get; set; }
        string PieceClearPlace { get; set; }
        int PieceClearTime { get; set; }
        string PieceEndPlace { get; set; }
        int PieceEndTime { get; set; }
        string PieceReportPlace { get; set; }
        int PieceReportTime { get; set; }
        int PieceReportTimeCalc { get; set; }
        string PieceStartPlace { get; set; }
        int PieceStartTime { get; set; }
        string PlaceSequence { get; set; }
        string Platform_Time { get; set; }
        string PublicText { get; set; }
        int RouteID { get; set; }
        string RouteText { get; set; }
        string ScheduleTypeName { get; set; }
        string Spread_Time { get; set; }
        string StopID { get; set; }
        int StopSequence { get; set; }
        string Terminal { get; set; }
        int Total_Plat_1 { get; set; }
        int Total_Spread_1 { get; set; }
        int TripIndex { get; set; }
        int TripNumber { get; set; }
        string TripNumText { get; set; }
        string TripPatternID { get; set; }
        string TripTypeName { get; set; }
        string VariantDescription { get; set; }
        string Vehicle_Type { get; set; }
        string ViaVariant { get; set; }
        string TripText { get; set; }
        string TripStopText { get; set; }
        string InstructionText { get; set; }
    }
}
using Shared.Kernel.Interfaces;

namespace UnitTests
{
    public sealed class Mock_PaddlePrintModel : IPaddlePrintModel
    {
        public int DutyNumber { get; set; }
        public string Effective_Date { get; set; }
        public string ScheduleTypeName { get; set; }
        public int BlockNumber { get; set; }
        public string Vehicle_Type { get; set; }
        public int Block_Display { get; set; }
        public int InternalBlockNumber { get; set; }
        public int RouteID { get; set; }
        public string RouteText { get; set; }
        public string Terminal { get; set; }
        public string DirectionID { get; set; }
        public string DirectionText { get; set; }
        public int TripNumber { get; set; }
        public string TripNumText { get; set; }
        public string TripTypeName { get; set; }
        public int TripNumSort { get; set; }
        public string TripPatternID { get; set; }
        public string FromToVia { get; set; }
        public string ViaVariant { get; set; }
        public string VariantDescription { get; set; }
        public string PlaceSequence { get; set; }
        public int StopSequence { get; set; }
        public string StopID { get; set; }
        public string AlternateName { get; set; }
        public string DriverText { get; set; }
        public string PublicText { get; set; }
        public int PassingTime { get; set; }
        public string Formatted_Passing_Time { get; set; }
        public string PieceReportPlace { get; set; }
        public int PieceReportTime { get; set; }
        public int PieceReportTimeCalc { get; set; }
        public string Formatted_Piece_Report_Time { get; set; }
        public string PieceStartPlace { get; set; }
        public int PieceStartTime { get; set; }
        public string Formatted_Piece_Start_Time { get; set; }
        public string PieceEndPlace { get; set; }
        public int PieceEndTime { get; set; }
        public string Formatted_Piece_End_Time { get; set; }
        public string Platform_Time { get; set; }
        public string Spread_Time { get; set; }
        public string PieceClearPlace { get; set; }
        public int PieceClearTime { get; set; }
        public string Formatted_Piece_Clear_Time { get; set; }
        public int Total_Plat_1 { get; set; }
        public int Total_Spread_1 { get; set; }
        public int TripIndex { get; set; }
    }
}

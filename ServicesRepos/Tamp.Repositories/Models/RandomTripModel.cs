namespace Tamp.Repositories
{
    public enum Direction { 
        NORTH,
        SOUTH, 
        EAST,
        WEST
    }
  
    public class RandomTripModel
    {
        public string LineId { get; set; }
        public int BlockNumber { get; set; }
        public string Service { get; set; }
        public string Direction { get; set; }
        public string Garage { get; set; }
        public string DayOfWeek { get; set; }
        public int RunNumber { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public float Miles { get; set; }
        public int Trip { get; set; }
        public string Car { get; set; }
        public string Type { get; set; }

    
    }
    
}

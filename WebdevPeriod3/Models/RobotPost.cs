using System;

namespace WebdevPeriod3.Models
{
    //TODO: DTO or not? If not move to entities
    public class RobotPost
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Replies { get; set; }
    }
}

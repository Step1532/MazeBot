namespace MazeGenerator.Models
{
    public class GameEvent
    {
        public Coordinate Position { get; set; }
        public EventTypeEnum Type { get; set; }

        public GameEvent(EventTypeEnum type, Coordinate position)
        {
            Type = type;
            Position = position;
        }

        public GameEvent()
        {
            
        }
    }
}
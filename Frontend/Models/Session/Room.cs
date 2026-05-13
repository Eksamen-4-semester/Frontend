namespace Frontend.Models.Session;

public class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; }

    public int Capacity { get; set; }
    
    public int fitnessCenterId { get; set; }
}
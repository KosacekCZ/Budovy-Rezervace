using System.ComponentModel.DataAnnotations;

namespace Budovy_Rezervace.Models;

public class Room
{
    [Required]
    public string RoomId { get; set; }
    [Required]
    public int RoomNum { get; set; }
    [Required]
    public string? Location { get; set; }
    public string? Description { get; set; }

    public Room(string id, int roomNum, string? location, string? description)
    {
        RoomId = id;
        roomNum = RoomNum;
        location = Location;
        description = Description;

    }
    
}
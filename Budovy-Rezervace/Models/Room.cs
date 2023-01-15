using System.ComponentModel.DataAnnotations;

namespace Budovy_Rezervace.Models;

public class Room
{
    [Required]
    public string RoomId { get; set; }

    public Room(string id)
    {
        RoomId = id;
        
    }
    
}
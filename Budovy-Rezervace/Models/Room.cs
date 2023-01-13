using System.ComponentModel.DataAnnotations;

namespace Budovy_Rezervace.Models;

public class Room
{
    [Required]
    public int RoomID { get; set; }
    
}
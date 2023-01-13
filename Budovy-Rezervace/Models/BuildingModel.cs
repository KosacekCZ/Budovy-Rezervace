using System.ComponentModel.DataAnnotations;

namespace Budovy_Rezervace.Models;

public class BuildingModel
{
    [Required]
    public int BuildingID { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Adress { get; set; }
    public List<Room> Rooms { get; set; }

}
using System.ComponentModel.DataAnnotations;

namespace Budovy_Rezervace.Models;

public class BuildingModel
{
    [Required]
    public string BuildingId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Adress { get; set; }

    public Dictionary<string, Room> Rooms { get; set; }

    public void AddRoom(string id, Room r)
    {
        Rooms.Add(id, r);
    }
    
    public void RemoveRoom(string id)
    {
        Rooms.Remove(id);
    }

    public BuildingModel(string id, string name, string adress, Dictionary<string, Room> rooms)
    {
        BuildingId = id;
        Name = name;
        Adress = adress;
        Rooms = rooms;
    }

    public override string ToString()
    {
        return $"{BuildingId}, {Name}, {Adress}\n";
    }


}
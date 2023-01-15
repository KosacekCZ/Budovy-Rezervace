using Microsoft.AspNetCore.Mvc;
using Budovy_Rezervace.Models;

namespace Budovy_Rezervace.Controllers;

public class HomeController : Controller
{
    public Dictionary<string, BuildingModel> buildings = new Dictionary<string, BuildingModel>();

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult BuildingScheme(string pid)
    {
        // Temporary restructure data to Dictionary
        LoadDataToDictionary();
        // Pre-process selective data by building PID
        ViewData["pid"] = pid;
        ViewData["rooms"] = buildings[pid].Rooms;
        ViewData["buildingName"] = buildings[pid].Name;
        ViewData["buildingAdress"] = buildings[pid].Adress;
        
        return View("BuildingScheme");
    }

    [HttpPost]
    public IActionResult CreateBuilding(string buildingName, string buildingAdress)
    {
        // Save data to CSV, using ' | ' separator. format [id, buildingName, adress]
        string id = GenerateId();
        string parsedBuildingdata = $"{id} | {buildingName} | {buildingAdress} \n";
        string dataPath = $"Data/Buildings/{id}";
        Directory.CreateDirectory(dataPath);
        System.IO.File.AppendAllText("Data/Buildings/buildings.csv", parsedBuildingdata);
        
        return View("Index");
    }

    [HttpPatch]
    public IActionResult EditBuilding() 
    {
        return View("Index");
    }

    [HttpDelete]
    public IActionResult DeleteBuilding(string buildingId)
    {
        LoadDataToDictionary();
        buildings.Remove(buildingId);
        string NewData = "";
        foreach (var entry in buildings)
        {
            NewData += $"{entry.Key} | {entry.Value.Name} | {entry.Value.Adress}\n";
        }

        System.IO.File.CreateText("Data/Buildings/buildings.csv");

        return View("Index");
    }

    public string GenerateId()
    {
        return Guid.NewGuid().ToString("N");
    }

    public void LoadDataToDictionary()
    {
        if (buildings.Count == 0)
        {
            foreach (string data in System.IO.File.ReadLines("Data/Buildings/buildings.csv").Skip(1))
            {
                string[] buildingData = data.Split('|');
                buildings.Add(buildingData[0], 
                    new BuildingModel(buildingData[0], 
                        buildingData[1], 
                        buildingData[2], 
                        new Dictionary<string, Room>()));
                Console.WriteLine(data);
            }
            Console.WriteLine(buildings.Count);
        }
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budovy_Rezervace.Models;

namespace Budovy_Rezervace.Controllers;

public class HomeController : Controller
{
    public Dictionary<string, BuildingModel> buildings = new Dictionary<string, BuildingModel>();

    public IActionResult Index()
    {
        loadDataToDictionary();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult BuildingScheme(string pid)
    {
        // Pre-process selective data by building PID
        ViewData["pid"] = pid;
        ViewData["rooms"] = buildings[pid].Rooms;
        ViewData["buildingName"] = buildings[pid].Name;
        ViewData["buildingAdress"] = buildings[pid].Adress;
        
        return View("BuildingScheme");
    }

    public IActionResult AddBuilding()
    {
        return View("CreateHouse");
    }

    [HttpPost]
    public IActionResult CreateBuilding(string buildingName, string buildingAdress)
    {
        // Save data to CSV, using ' | ' separator. format [id, buildingName, adress]
        string parsedBuildingdata = $"{generateID()} | {buildingName} | {buildingAdress} \n";
        System.IO.File.AppendAllText("csv/buildings.csv", parsedBuildingdata);
        
        return View("Index");
    }

    [HttpPatch]
    public IActionResult EditBuilding() 
    {
        return View("Index");
    }

    [HttpDelete]
    public IActionResult DeleteBuilding()
    {
        return View("Index");
    }

    public string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }

    public void loadDataToDictionary()
    {
        
    }
}
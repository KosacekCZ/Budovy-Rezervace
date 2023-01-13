using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Budovy_Rezervace.Models;

namespace Budovy_Rezervace.Controllers;

public class HomeController : Controller
{
    public List<BuildingModel> budovy = new List<BuildingModel>();
    
    public IActionResult Index()
    {
        return View(budovy);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult RedirectToAddBuildingPage()
    {
        return View("CreateHouse");
    }

    [HttpPost]
    public IActionResult CreateBuilding(string buildingName, string buildingAdress)
    {
        string parsedBuildingdata = $"{generateID()} | {buildingName} | {buildingAdress} \n";
        // Save data to CSV, using ' | ' separator. format [id, buildingName, adress]
        System.IO.File.AppendAllText("csv/buildings.csv", parsedBuildingdata);
        return View("Index");
    }

    public IActionResult EditBuilding() 
    {
        return View("Index");
    }

    public IActionResult DeleteBuilding()
    {
        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }
}
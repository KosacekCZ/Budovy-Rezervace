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

    public IActionResult Schedule()
    {
        return View();
    }

    public IActionResult BuildingScheme(string pid)
    {
        LoadBuildingScheme(pid);
        return View("BuildingScheme");
    }

    [HttpPost]
    public IActionResult CreateBuilding(string buildingName, string buildingAdress)
    {
        // Save data to CSV, using ' | ' separator. format [id, buildingName, adress]
        string id = GenerateId();
        string parsedBuildingdata = $"{id}|{buildingName}|{buildingAdress}\n";
        string dataPath = $"Data/Buildings/{id}";
        Directory.CreateDirectory(dataPath);
        System.IO.File.WriteAllText($"{dataPath}/rooms.csv", "PID | Room Number | Location | Description\n");
        System.IO.File.AppendAllText("Data/Buildings/buildings.csv", parsedBuildingdata);

        return View("Index");
    }

    [HttpPatch]
    public IActionResult EditBuilding()
    {
        return View("Index");
    }

    [HttpPost]
    public IActionResult DeleteBuilding(string buildingId)
    {
        Console.WriteLine(buildingId);
        Dictionary<string, BuildingModel> temp = TemporaryDataLoader();
        temp.Remove(buildingId);
        Directory.Delete($"Data/Buildings/{buildingId}", true);
        
        // Write new File header to CSV, re-create file
        System.IO.File.WriteAllText("Data/Buildings/buildings.csv", 
            "ID | Building Name | Building Adress\n");

        foreach (var entry in temp)
        {
            System.IO.File.AppendAllText("Data/Buildings/buildings.csv", 
                $"{entry.Key.Trim(' ')}|{entry.Value.Name}|{entry.Value.Adress}\n");
        }
        buildings.Clear();
        temp.Clear();
        return View("Index");
    }

    [HttpPost]
    public IActionResult CreateRoom(string pid, int roomNum, string location, string descr)
    {
        // Create headers for data
        string data = $"{GenerateId()}|{roomNum}|{location}|{descr}\n";
        string dataPath = $"Data/Buildings/{pid}/";
        // Write data to CSV
        System.IO.File.AppendAllText($"{dataPath}/rooms.csv", data);
        // Create directory structure for room schedules
        Directory.CreateDirectory($"{dataPath}/{roomNum}");
        CreateScheduleCsv($"{dataPath}/{roomNum}/schedule.csv");
        // Return
        LoadBuildingScheme(pid);
        return View("BuildingScheme");
    }

    public void LoadBuildingScheme(string pid)
    {
        // Temporary restructure data to Dictionary
        LoadDataToDictionary();
        // Pre-process selective data by building PID
        ViewData["pid"] = pid;
        ViewData["rooms"] = buildings[pid].Rooms;
        ViewData["buildingName"] = buildings[pid].Name;
        ViewData["buildingAdress"] = buildings[pid].Adress;
    }

    private string GenerateId()
    {
        return Guid.NewGuid().ToString("N");
    }

    private void LoadDataToDictionary()
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
               // Console.WriteLine(data);
            }

           // Console.WriteLine(buildings.Count);
        }
    }

    private Dictionary<string, BuildingModel> TemporaryDataLoader()
    {
        Dictionary<string, BuildingModel> temp = new Dictionary<string, BuildingModel>();
        foreach (string data in System.IO.File.ReadLines("Data/Buildings/buildings.csv").Skip(1))
        {
            string[] buildingData = data.Split('|');
            temp.Add(buildingData[0],
                new BuildingModel(buildingData[0],
                    buildingData[1],
                    buildingData[2],
                    new Dictionary<string, Room>()));
        }
        return temp;
    }

    public IActionResult DumpCsvData()
    {
        Response.WriteAsync("<script>alert('system alert')</script>");
        /*Directory.Delete("Data/Buildings", true);
        Directory.CreateDirectory("Data/buildings/");
        System.IO.File.WriteAllText("Data/Buildings/buildings.csv", "ID | Building Name | Building Adress\n");*/
        return Index();
    }

    private void CreateScheduleCsv(String path)
    {
        System.IO.File.Create(path);
        System.IO.File.WriteAllText(path, "Date | Time Start | Time End | Event Name | Event Description");
    }
}
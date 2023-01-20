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
        System.IO.File.Create($"{dataPath}/rooms.csv");
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

        using (var fs = new FileStream("Data/Buildings/buildings.csv", FileMode.Truncate)){}
        using (StreamWriter wr = new StreamWriter("Data/Buildings/buildings.csv", append: true))
        {
            wr.WriteLine("ID | Building Name | Building Adress");
            wr.Close();
        }

        foreach (var entry in temp)
        {
            using (StreamWriter wr = new StreamWriter("Data/Buildings/buildings.csv", append: true))
            {
                wr.WriteLine($"{entry.Key.Trim(' ')}|{entry.Value.Name}|{entry.Value.Adress}");
                wr.Close();
            }
        }

        buildings.Clear();
        temp.Clear();
        return View("Index");
    }

    [HttpPost]
    public IActionResult CreateRoom(string pid, int roomNum, string location, string descr)
    {
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
                Console.WriteLine(data);
            }

            Console.WriteLine(buildings.Count);
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
}
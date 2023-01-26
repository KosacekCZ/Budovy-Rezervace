using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Budovy_Rezervace.Controllers;

public class EventController : Controller
{
    [HttpPost]
    public IActionResult CreateEvent(string pid, int rid, DateTime eventStart, DateTime eventEnd, string eventName, string eventDescr)
    {
        ViewData["pid"] = pid; // Place ID
        ViewData["rid"] = rid; // Room ID
        
        // path to CSV
        string path = $"Data/Buildings/{pid}/{rid.ToString()}/schedule.csv";
        
        // Write data to CSV
        StashEventData(path, eventStart, eventEnd, eventName, eventDescr);
        return View("Schedule");
    }

    private void StashEventData(string path, DateTime start, DateTime end, string name, string description)
    {
        
        var writeData = $"{start.ToString(CultureInfo.InvariantCulture)}|{end.ToString(CultureInfo.InvariantCulture)}|{name}|{description}\n";
        System.IO.File.AppendAllText(path, writeData);
    }
}
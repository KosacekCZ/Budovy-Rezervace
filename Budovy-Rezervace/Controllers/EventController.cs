using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Budovy_Rezervace.Controllers;

public class EventController : Controller
{
    [HttpPost]
    public IActionResult CreateEvent(string pid, int rid, DateTime eventStart, DateTime eventEnd, string eventName, string eventDescr, string status)
    {
        ViewData["pid"] = pid; // Place ID
        ViewData["rid"] = rid; // Room ID
        
        // path to CSV
        string path = $"Data/Buildings/{pid}/{rid.ToString()}/schedule.csv";
        
        // Write data to CSV
        if (!CheckCollision(eventStart, eventEnd, pid, rid))
        {
            StashEventData(path, eventStart, eventEnd, eventName, eventDescr, status);
        }
        else
        {
            ViewData["?Error"] = true;
            ViewData["ErrorMessage"] = "Created event collides with already existing event!";
            return View("Schedule");
        }

        ViewData["?Error"] = false;
        ViewData["ErrorMessage"] = null; 
        return View("Schedule");
    }

    private void StashEventData(string path, DateTime start, DateTime end, string name, string description, string status)
    {
        
        var writeData = $"{start.ToString(CultureInfo.InvariantCulture)}|{end.ToString(CultureInfo.InvariantCulture)}|{name}|{description}|{status}\n";
        System.IO.File.AppendAllText(path, writeData);
    }

    public void UpdateEventData()
    {
        
    }

    private bool CheckCollision(DateTime start, DateTime end, string pid, int rid)
    {
        foreach (var eventData in System.IO.File.ReadAllLines($"Data/Buildings/{pid}/{rid}/schedule.csv").Skip(1))
        {
            var e = eventData.Split("|");
            var ts = e[0].Split(" ")[1].Split(":");
            var te = e[1].Split(" ")[1].Split(":");
            
            if (start.Date.ToString() == eventData.Split("|")[0].Split(" ")[0])
            {
                if ((start.Hour >= int.Parse(ts[0]) && start.Hour <= int.Parse(te[0])) || (end.Hour <= int.Parse(te[0]) && end.Hour >= int.Parse(ts[0])))
                {
                    // Console.WriteLine($"HourCollision: {(start.Hour >= int.Parse(ts[0]) && start.Hour <= int.Parse(te[0])) || (end.Hour <= int.Parse(te[0]) && end.Hour >= int.Parse(ts[0]))}");
                    if ((end.Minute <= int.Parse(te[1]) && end.Minute >= int.Parse(ts[1])) || (start.Minute >= int.Parse(ts[1]) && start.Minute <= int.Parse(te[1])))
                    {
                        // Console.WriteLine($"MinuteCollision: {(end.Minute <= int.Parse(te[1]) && end.Minute >= int.Parse(ts[1])) || (start.Minute >= int.Parse(ts[1]) && start.Minute <= int.Parse(te[1]))}");
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
}
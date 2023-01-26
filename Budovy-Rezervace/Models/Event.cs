using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Budovy_Rezervace.Models;

public class Event
{
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public double BarStart { get; set; }
    public double BarLength { get; set; }

    public Event(string start, string end, string name, string description)
    {
        //Console.WriteLine(start);
        //Console.WriteLine(end);
        // Console.WriteLine(int.Parse(start.Split(':')[0]) + " " + int.Parse(start.Split(':')[1][..2]));

        // DateTime( yyyy/mm/dd/hh/mm/ss )
        var startDate = start.Split(" ")[0].Split("/");
        var endDate = end.Split(" ")[0].Split("/");
        var startTime = start.Split(" ")[1].Split(":");
        var endTime = end.Split(" ")[1].Split(":");

        // Console.WriteLine($"{(start.Split(" ")[1].Contains("am") ? startTime[0] : startTime[0] + 12)}:{startTime[1]}:{startTime[2][..2]}");
        //Console.WriteLine($"{(end.Split(" ")[1].Contains("am") ? endTime[0] : int.Parse(endTime[0]) + 12)}:{endTime[1]}:{endTime[2][..2]}");

            EventStart = new DateTime(
            int.Parse(startDate[2]),
            int.Parse(startDate[0]),
            int.Parse(startDate[1]),
            int.Parse(startTime[0]),
            int.Parse(startTime[1]),
            int.Parse(startTime[2][..2])
            );
            
        EventEnd = new DateTime(
                int.Parse(endDate[2]),
                int.Parse(endDate[0]),
                int.Parse(endDate[1]),
                int.Parse(endTime[0]),
                int.Parse(endTime[1]),
                int.Parse(endTime[2][..2])
        );
            
        EventName = name;
        EventDescription = description;
    }

    public double calcStart(DateTime start)
    {
        return (start.Hour * 60 + start.Minute) / (24 * 60);
    }

    public double calcEnd(DateTime end)
    {
        return (end.Hour * 60 + end.Minute) / (24 * 60);
    }
    
    private double EventLength(string start, string end)
    {
        if (start.Equals(new Regex("")))
        {
        }

        return 0.0;
    }
}
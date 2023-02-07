using Newtonsoft.Json;
using System.Net;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        var baseUrl = $"https://jsonmock.hackerrank.com/api/football_matches";
        
        HttpClient client;

        HttpClientHandler handler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        client = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };

        HttpResponseMessage response = client.GetAsync($"?year={year}&team1={team}").Result;
        response.EnsureSuccessStatusCode();
        string result = response.Content.ReadAsStringAsync().Result;        
        var matches = JsonConvert.DeserializeObject<Match>(result);
        var totalGoals = matches.Data.Sum(p => Convert.ToInt32(p.Team1goals));

        for (int i = 2; i <= matches.Total_pages; i++)
        {
            response = client.GetAsync($"?year={year}&team1={team}&page={i}").Result;
            response.EnsureSuccessStatusCode();
            result = response.Content.ReadAsStringAsync().Result;
            matches = JsonConvert.DeserializeObject<Match>(result);
            totalGoals += matches.Data.Sum(p => Convert.ToInt32(p.Team1goals));
        }

        response = client.GetAsync($"?year={year}&team2={team}").Result;
        response.EnsureSuccessStatusCode();
        result = response.Content.ReadAsStringAsync().Result;
        matches = JsonConvert.DeserializeObject<Match>(result);
        totalGoals += matches.Data.Sum(p => Convert.ToInt32(p.Team2goals));

        for (int i = 2; i <= matches.Total_pages; i++)
        {
            response = client.GetAsync($"?year={year}&team2={team}&page={i}").Result;
            response.EnsureSuccessStatusCode();
            result = response.Content.ReadAsStringAsync().Result;
            matches = JsonConvert.DeserializeObject<Match>(result);
            totalGoals += matches.Data.Sum(p => Convert.ToInt32(p.Team2goals));
        }

        return totalGoals;
    }

    public class MatchData
    {
        public string Competition { get; set; }
        public int Year { get; set; }
        public string Round { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public string Team1goals { get; set; }
        public string Team2goals { get; set; }
    }

    public class Match
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public List<MatchData> Data { get; set; }
    }
}
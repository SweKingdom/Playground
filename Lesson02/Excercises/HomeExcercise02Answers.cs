using Models.Music;
using PlayGround.Extensions;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson02;

public static class HomeExcericee02Answers
{
    public static void Entry(string[] args = null)
    {      
        System.Console.WriteLine("=== LINQ Music Groups Exercises ===\n");
        var seedGenerator = new SeedGenerator();
        var musicgroups = seedGenerator.ItemsToList<MusicGroup>(50);
    
        //See how clean it becomes when I made the Serialization to Json
        //musicgroups.SerializeJson("musicgroups.json");
        //musicgroups = musicgroups.DeSerializeJson("musicgroups.json");


        // EASY: Basic Filtering & Projection (Questions 1-3)
        
        // Q1: Find all Rock music groups
        // Use: Where, Select
        // Expected: Filter by Genre == MusicGenre.Rock, project to group names
        System.Console.WriteLine("Q1: All Rock music groups:");
        // TODO: var rockGroups = ...
        var rockGroups = musicgroups
            .Where(g => g.Genre == MusicGenre.Rock)
            .Select(g => (g.Name, g.Genre));
        rockGroups.ToList().ForEach(g => System.Console.WriteLine(g));   
        
        // Q2: Get the names of all albums released after 2010
        // Use: SelectMany, Where, Select
        // Expected: Flatten all albums, filter by year, project to album names
        System.Console.WriteLine("\nQ2: Albums released after 2010:");
        // TODO: var recentAlbums = ...
        musicgroups
            .SelectMany(g => g.Albums)
            .Where(a => a.ReleaseYear > 2010)
            .Select(a => (a.Name, a.ReleaseYear))
            .ToList().ForEach(a => System.Console.WriteLine(a));
        
        // Q3: Find the first 5 music groups ordered by establishment year
        // Use: OrderBy, Take
        // Expected: Sort by EstablishedYear ascending, take first 5
        System.Console.WriteLine("\nQ3: First 5 oldest music groups:");
        // TODO: var oldestGroups = ...
        var oldestGroups = musicgroups
            .OrderBy(g => g.EstablishedYear)
            .Take(5)
            .Select(g => (g.Name, g.EstablishedYear));
        oldestGroups.ToList().ForEach(g => System.Console.WriteLine(g));

        // MEDIUM: Aggregation & Grouping (Questions 4-6)
        
        // Q4: Calculate total copies sold across all albums for each genre
        // Use: GroupBy, Sum, Select
        // Expected: Group by Genre, sum CopiesSold from all albums in each group
        System.Console.WriteLine("\nQ4: Total copies sold by genre:");
        // TODO: var copiesByGenre = ...
        var copiesByGenre = musicgroups
            .GroupBy(g => g.Genre)
            .Select(grp => 
                (Genre: grp.Key, 
                 TotalCopiesSold: grp.Sum(g => g.Albums.Sum(a => a.CopiesSold))
                )
            );
        copiesByGenre.ToList().ForEach(g => System.Console.WriteLine(g));
        
        // Q5: Find all music groups that have more than 3 artists
        // Use: Where, Count
        // Expected: Filter groups where Artists.Count > 3
        System.Console.WriteLine("\nQ5: Groups with more than 3 artists:");
        // TODO: var largeGroups = ...
        var largeGroups = musicgroups
            .Where(g => g.Artists.Count > 3)
            .Select(g => (g.Name, ArtistCount: g.Artists.Count));
        largeGroups.ToList().ForEach(g => System.Console.WriteLine(g));
        
        // Q6: Get the average number of tracks per album for each music group
        // Use: Select, SelectMany, Average
        // Expected: For each group, calculate average track count across their albums
        System.Console.WriteLine("\nQ6: Average tracks per album by group:");
        // TODO: var avgTracks = ...
        var avgTracks = musicgroups
            .Select(g => 
                (g.Name, 
                 AverageTracks: g.Albums.Any() 
                    ? g.Albums.Average(a => a.Tracks.Count) 
                    : 0)
                );
        avgTracks.ToList().ForEach(g => System.Console.WriteLine(g));

        // ADVANCED: Complex Queries & Multiple Operations (Questions 7-10)
        
        // Q7: Find artists who appear in Jazz groups with albums that sold over 500,000 copies
        // Use: Where, SelectMany, Distinct/DistinctBy
        // Expected: Filter Jazz groups, get albums with high sales, flatten artists, remove duplicates
        System.Console.WriteLine("\nQ7: Artists in successful Jazz groups:");
        // TODO: var successfulJazzArtists = ...
        var successfulJazzArtists = musicgroups
            .Where(g => g.Genre == MusicGenre.Jazz)
            .Where(g => g.Albums.Any(a => a.CopiesSold > 500_000))
            .SelectMany(g => g.Artists)
            .Distinct()
            .Select(a => (a.FirstName, a.LastName, a.BirthDay));
        successfulJazzArtists.ToList().ForEach(a => System.Console.WriteLine(a));
        
        // Q8: For each genre, find the music group with the most albums and show album count
        // Use: GroupBy, Select, OrderByDescending, First
        // Expected: Group by genre, for each genre find group with max album count
        System.Console.WriteLine("\nQ8: Most prolific group per genre:");
        // TODO: var mostProlificByGenre = ...
        var mostProlificByGenre = musicgroups
            .GroupBy(g => g.Genre)
            .Select(grp => 
                grp.OrderByDescending(g => g.Albums.Count).First()
            )
            .Select(g => (g.Genre, g.Name, AlbumCount: g.Albums.Count));
        mostProlificByGenre.ToList().ForEach(g => System.Console.WriteLine(g));
        

        mostProlificByGenre = musicgroups
            .GroupBy(g => g.Genre)
            .SelectMany(grp => 
            {
                var maxAlbums = grp.Max(g => g.Albums.Count);
                return grp.Where(g => g.Albums.Count == maxAlbums);
            })
            .Select(g => (g.Genre, g.Name, AlbumCount: g.Albums.Count));
        mostProlificByGenre.ToList().ForEach(g => System.Console.WriteLine(g));

        // Q9: Find the top 10 longest tracks across all music groups with their group and album info
        // Use: SelectMany (nested), OrderByDescending, Take, Select
        // Expected: Flatten to tracks with context, sort by duration, take 10
        System.Console.WriteLine("\nQ9: Top 10 longest tracks:");
        // TODO: var longestTracks = ...
        var longestTracks = musicgroups
            .SelectMany(g => 
                g.Albums.SelectMany(a => 
                    a.Tracks.Select(t => 
                        (GroupName: g.Name, AlbumName: a.Name, TrackName: t.Name, t.DurationSeconds)
                    )
                )
            )
            .OrderByDescending(t => t.DurationSeconds)
            .Take(10);
        
        // Q10: Calculate percentage of albums per decade (1970s, 1980s, etc.) for Metal groups
        // Use: Where, SelectMany, GroupBy, Count, Select with calculations
        // Expected: Filter Metal groups, group albums by decade, calculate percentages
        System.Console.WriteLine("\nQ10: Album distribution by decade for Metal groups:");
        // TODO: var metalByDecade = ...
        var metalByDecade = musicgroups
            .Where(g => g.Genre == MusicGenre.Metal)
            .SelectMany(g => g.Albums)
            .GroupBy(a => (a.ReleaseYear / 10) * 10)
            .Select(grp => 
                (Decade: grp.Key, 
                 AlbumCount: grp.Count())
            );
        var totalMetalAlbums = metalByDecade.Sum(d => d.AlbumCount);
        var metalByDecadePercent = metalByDecade
            .Select(d => 
                (d.Decade, 
                 d.AlbumCount, 
                 Percentage: totalMetalAlbums > 0 
                    ? (double)d.AlbumCount / totalMetalAlbums * 100 
                    : 0)
            );
        metalByDecadePercent.ToList().ForEach(d => System.Console.WriteLine(d));

        /*
         * BONUS CHALLENGES:
         * 
         * Q11: Find music groups where all albums have at least one track longer than 5 minutes
         * Use: All, Any, SelectMany
         * 
         * Q12: Create a lookup of artists by their birth decade (only for artists with known birthdays)
         * Use: Where, ToLookup, GroupBy
         * 
         * Q13: Find pairs of music groups established in the same year
         * Use: Join or GroupBy, SelectMany
         * 
         * Q14: Calculate the "productivity score" for each group 
         * (total tracks * average copies sold / years active)
         * Use: Select, SelectMany, Sum, Average, complex calculations
         * 
         * Q15: Find the album with the most diverse track durations (highest standard deviation)
         * Use: SelectMany, Select, complex aggregation
         */
    }
}
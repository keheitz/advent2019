using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2019
{
    internal class AsteroidMap
    {
        public string MapData { get; set; } = ".#..#!.....!#####!....#!...##";
        //".#..#!.....!#####!....#!...##";  //3,4
        //"......#.#.!#..#.#....!..#######.!.#.#.###..!.#..#.....!..#....#.#!#..#....#.!.##.#..###!##...#..#.!.#....####"; //5,8
        //"#....#.....#...#.#.....#.#..#....#!#..#..##...#......#.....#..###.#.#!#......#.#.#.....##....#.#.....#..!..#.#...#.......#.##..#...........!.##..#...##......##.#.#...........!.....#.#..##...#..##.....#...#.##.!....#.##.##.#....###.#........####!..#....#..####........##.........#!..#...#......#.#..#..#.#.##......#!.............#.#....##.......#...#!.#.#..##.#.#.#.#.......#.....#....!.....##.###..#.....#.#..###.....##!.....#...#.#.#......#.#....##.....!##.#.....#...#....#...#..#....#.#.!..#.............###.#.##....#.#...!..##.#.........#.##.####.........#!##.#...###....#..#...###..##..#..#!.........#.#.....#........#.......!#.......#..#.#.#..##.....#.#.....#!..#....#....#.#.##......#..#.###..!......##.##.##...#...##.#...###...!.#.....#...#........#....#.###....!.#.#.#..#............#..........#.!..##.....#....#....##..#.#.......#!..##.....#.#......................!.#..#...#....#.#.....#.........#..!........#.............#.#.........!#...#.#......#.##....#...#.#.#...#!.#.....#.#.....#.....#.#.##......#!..##....#.....#.....#....#.##..#..!#..###.#.#....#......#...#........!..#......#..#....##...#.#.#...#..#!.#.##.#.#.....#..#..#........##...!....#...##.##.##......#..#..##....";
        public List<Asteroid> AsteroidLocations { get; set; } = new List<Asteroid>();

        public AsteroidMap()
        {
            Console.WriteLine("******Asteroid Position Mapping*********");
            List<string> mapLines = MapData.Split("!").ToList();
            int lineCount = 0, posCount = 0;
            foreach (var line in mapLines)
            {
                posCount = 0;
                char[] positions = line.ToCharArray();
                foreach (var pos in positions)
                {
                    if (pos == '#') //hashtags are asteroids, we only care about the position of these
                    {
                        Asteroid asteroid = new Asteroid(posCount, lineCount);
                        AsteroidLocations.Add(asteroid);
                        Console.WriteLine(asteroid.Position.CombinedCoords);
                    }
                    posCount++;
                }
                lineCount++;
            }
        }
        internal int FindBestDetectionCount()
        {
            Console.WriteLine("------------Counts--------------");
            foreach (var asteroid in AsteroidLocations)
            {
                List<InterAsteroidPath> paths = new List<InterAsteroidPath>();
                foreach(var destination in AsteroidLocations)
                {
                    if(asteroid.Position.CombinedCoords == destination.Position.CombinedCoords)
                    {
                        //keep going, we can't detect the asteroid we are on
                        continue;
                    }
                    paths.Add(new InterAsteroidPath(asteroid, destination));
                }
                asteroid.DetectableAsteroidCount = paths.Select(x => x.Path.Slope).Distinct().ToList().Count();
                Console.WriteLine($" Position - {asteroid.Position.CombinedCoords} and Count - {asteroid.DetectableAsteroidCount}");
                if (asteroid.Position.CombinedCoords == "3,4")
                {
                    int pathnum = 1;
                    foreach (var path in paths.OrderBy(x => x.Path.Slope))
                    {
                        Console.WriteLine($"{pathnum} Path to: {path.EndingAsteroid.Position.CombinedCoords}, Stats:{path.Distance}, {path.Path.Slope}, {path.Path.YIntercept}: {path.Path.TravelX},{path.Path.TravelY}");
                        pathnum++;
                    }
                }
            }
            return AsteroidLocations.Max(x => x.DetectableAsteroidCount);
        }
    }

    internal class InterAsteroidPath
    {
        public InterAsteroidPath(Asteroid asteroid, Asteroid destination)
        {
            StartingAsteroid = asteroid;
            EndingAsteroid = destination;
            Path = FindPath(asteroid.Position, destination.Position);
            Distance = Math.Abs(Path.TravelX) + Math.Abs(Path.TravelY);
        }

        private AsteroidPath FindPath(Position position1, Position position2)
        {
            AsteroidPath path = new AsteroidPath();
            path.TravelX = position2.CoordinateX - position1.CoordinateX;
            path.TravelY = position2.CoordinateY - position1.CoordinateY;
            path.Slope = path.TravelY / (path.TravelX != 0 ? path.TravelX : 1);
            path.YIntercept = (path.Slope * position1.CoordinateX) - position1.CoordinateY;
            return path;
        }

        public Asteroid StartingAsteroid { get; set; }
        public Asteroid EndingAsteroid { get; set; }
        public int Distance { get; set; }
        public AsteroidPath Path { get; set; }
    }

    public class AsteroidPath
    {
        public int TravelX { get; set; }
        public int TravelY { get; set; }
        public decimal Slope { get; set; }
        public decimal YIntercept { get; set; }

    }

    internal class Asteroid
    {
        public Asteroid(int x, int y)
        {
            Position = new Position(x, y);
        }
        public Position Position { get; set; }

        public int DetectableAsteroidCount { get; set; }
    }

    public class Position
    {
        public Position(int x, int y)
        {
            CoordinateX = x;
            CoordinateY = y;
        }
        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public string CombinedCoords => $"{CoordinateX},{CoordinateY}";
    }
}
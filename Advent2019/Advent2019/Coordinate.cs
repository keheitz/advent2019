namespace Advent2019
{
    public class Coordinate
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int StepsToReach { get; set; }

        public override string ToString()
        {
            return $"{XCoordinate}|{YCoordinate}";
        }

        public Coordinate()
        {   
        }

        public Coordinate(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }

        public Coordinate(int x, int y, int steps)
        {
            XCoordinate = x;
            YCoordinate = y;
            StepsToReach = steps;
        }
    }
}
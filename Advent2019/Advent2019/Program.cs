using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Day: ");

            int advent_day = Convert.ToInt32(Console.ReadLine());
            int solution = 0;

            switch (advent_day)
            {
                case 1:
                    FuelCounter counter = new FuelCounter(3);
                    counter.CalculateTotal();
                    solution = counter.Total_Fuel;
                    break;
                case 2:
                    IntcodeComputer comp = new IntcodeComputer(AvailablePrograms.GravityAssist);
                    solution = comp.FindNounAndVerbForOutput(19690720);
                    break;
                case 3:
                    FuelManagementPanel panel = new FuelManagementPanel();
                    solution = panel.FindShortestCombinedStepsToIntersection();
                    break;
                case 4:
                    PasswordTester tester = new PasswordTester(256310,732736);
                    solution = tester.GetPotentialPasswordCount();
                    break;
                case 5:
                    IntcodeComputer test = new IntcodeComputer(AvailablePrograms.TEST);
                    solution = test.GetTESTDiagnosticCode();
                    break;
                case 6:
                    OrbitMap map = new OrbitMap();
                    map.ParseOrbits();
                    //solution = map.GetTotalNumberOfOrbits();
                    solution = map.GetOrbitalTransfersNeeded("YOU", "SAN");
                    break;
                case 7:
                    //I got the first part and then kept botching the second...
                    PhaseSettingPermutations permutations = new PhaseSettingPermutations(new int[5] { 0, 1, 2, 3, 4 });
                    Dictionary<int[], long> permutationOutputs = new Dictionary<int[], long>();
                    foreach(var permutation in permutations.PhaseSettingList)
                    {
                        Console.WriteLine("[{0}]", string.Join(", ", permutation));
                        //AmplificationCircuit circuit = new AmplificationCircuit(AmplifierConfigs.CircularList, permutation);
                        AmplificationCircuit circuit = new AmplificationCircuit(AmplifierConfigs.Series, permutation);
                        long output = circuit.RunSeries(AmplifierConfigs.Series);
                        Console.WriteLine("output at time - " + output.ToString());
                        permutationOutputs.Add(permutation, output);
                    }
                    long thisSolved = permutationOutputs.Values.Max();
                    Console.WriteLine("SOLUTION: " + thisSolved);
                    break;
                case 8:
                    SpaceImageFormat imgFormat = new SpaceImageFormat(25,6);
                    imgFormat.InitializeWithRoverPassword();
                    //solution = imgFormat.CheckForCorruptionValue();
                    imgFormat.PrintDecodedImage();
                    break;
                case 9:
                    IntcodeComputer boost = new IntcodeComputer(AvailablePrograms.BOOST);
                    boost.Process();
                    solution = (int)boost.Outputs.Last();
                    break;
                case 10:
                    AsteroidMap asteroidMap = new AsteroidMap();
                    solution = asteroidMap.FindBestDetectionCount();
                    break;
                default:
                    Console.WriteLine("No existing program for this day yet...");
                    break;
            }

            Console.WriteLine(solution);
        }
    }
}

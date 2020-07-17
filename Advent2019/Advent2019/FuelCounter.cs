using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2019
{
    public class FuelCounter
    {
        private List<int> module_masses { get; set; } = new List<int>();

        public List<int> initial_fuel_quantities { get; set; } = new List<int>();

        private int divisor;

        public int Total_Fuel { get; set; }

        public FuelCounter(int divide_by)
        {
            string input = "66910,78957,58510,142350,105820,87317,100743,51390,92804,80752,70169,111892,104715,143166,126158,78712,139223,133863,85912,53883,64812,102254,52482,91855,117520,140253,76706,106693,57948,57578,115640,131273,115373,145219,100889,106447,72347,120250,56898,146689,138246,85068,55292,124814,136750,51820,70396,92806,86093,70467,122356,148530,85569,100492,87062,123225,73872,102104,91194,95077,88352,114906,141056,87220,106517,88867,95883,130158,76702,134241,50561,119258,61669,140396,145201,76914,102281,56618,145968,99542,116789,135633,114646,84253,50650,69127,95446,55357,81180,126940,133743,52261,117429,82291,110373,67626,58014,125342,129508,96332";
            module_masses = input.Split(',').Select(int.Parse).ToList();
            divisor = divide_by;
        }

        public void CalculateTotal()
        {
            initial_fuel_quantities = module_masses.Select(x => calculate_fuel(x)).ToList();
            foreach(int quantity in initial_fuel_quantities)
            {
                Total_Fuel += quantity;
                int fuel_to_add = RecurseForFuelWeight(quantity);
            }
        }

        private int RecurseForFuelWeight(int mass)
        {
            int fuel_to_add = calculate_fuel(mass);

            if (fuel_to_add > 0)
            {
                Total_Fuel += fuel_to_add;
                return RecurseForFuelWeight(fuel_to_add);
            }
            return Total_Fuel;
        }

        private int calculate_fuel(int mass)
        {
            return mass / divisor - 2;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2019
{
    internal class IntcodeComputer
    {
        private long[] memory { get; set; }
        private long instruction_pointer = 0;
        private long relativeBase = 0;
        private int loadedProgram;
        private List<int> inputParameters = new List<int>();
        private int inputCount = 0;
        private static int[] modeMask = new int[] { 0, 100, 1000, 10000 };
        private ParameterModes GetMode(long addr, int i)
        {
            return (ParameterModes)(memory[instruction_pointer] / modeMask[i] % 10);
        }

        public bool Ended { get; set; } = false;
        public List<long> Outputs { get; set; } = new List<long>();

        public IntcodeComputer(int program)
        {
            initializeProgram(program);
        }
        public IntcodeComputer(int program, long phase, long signal) //overload for Amplification
        {
            initializeProgram(program, phase, signal);
        }

        private void initializeProgram(int program, long phase = 0, long signal = 0)
        {
            instruction_pointer = 0;
            string input = "";
            switch (program)
            {
                case AvailablePrograms.GravityAssist:
                    loadedProgram = AvailablePrograms.GravityAssist;
                    input = "1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,9,1,19,1,9,19,23,1,23,5,27,2,27,10,31,1,6,31,35,1,6,35,39,2,9,39,43,1,6,43,47,1,47,5,51,1,51,13,55,1,55,13,59,1,59,5,63,2,63,6,67,1,5,67,71,1,71,13,75,1,10,75,79,2,79,6,83,2,9,83,87,1,5,87,91,1,91,5,95,2,9,95,99,1,6,99,103,1,9,103,107,2,9,107,111,1,111,6,115,2,9,115,119,1,119,6,123,1,123,9,127,2,127,13,131,1,131,9,135,1,10,135,139,2,139,10,143,1,143,5,147,2,147,6,151,1,151,5,155,1,2,155,159,1,6,159,0,99,2,0,14,0";
                    break;
                case AvailablePrograms.TEST:
                    loadedProgram = AvailablePrograms.TEST;
                    input = "3,225,1,225,6,6,1100,1,238,225,104,0,2,106,196,224,101,-1157,224,224,4,224,102,8,223,223,1001,224,7,224,1,224,223,223,1002,144,30,224,1001,224,-1710,224,4,224,1002,223,8,223,101,1,224,224,1,224,223,223,101,82,109,224,1001,224,-111,224,4,224,102,8,223,223,1001,224,4,224,1,223,224,223,1102,10,50,225,1102,48,24,224,1001,224,-1152,224,4,224,1002,223,8,223,101,5,224,224,1,223,224,223,1102,44,89,225,1101,29,74,225,1101,13,59,225,1101,49,60,225,1101,89,71,224,1001,224,-160,224,4,224,1002,223,8,223,1001,224,6,224,1,223,224,223,1101,27,57,225,102,23,114,224,1001,224,-1357,224,4,224,102,8,223,223,101,5,224,224,1,224,223,223,1001,192,49,224,1001,224,-121,224,4,224,1002,223,8,223,101,3,224,224,1,223,224,223,1102,81,72,225,1102,12,13,225,1,80,118,224,1001,224,-110,224,4,224,102,8,223,223,101,2,224,224,1,224,223,223,4,223,99,0,0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,7,677,226,224,102,2,223,223,1005,224,329,101,1,223,223,108,226,226,224,102,2,223,223,1006,224,344,101,1,223,223,1108,226,677,224,102,2,223,223,1006,224,359,1001,223,1,223,107,677,677,224,1002,223,2,223,1005,224,374,1001,223,1,223,1107,226,677,224,102,2,223,223,1005,224,389,1001,223,1,223,107,677,226,224,1002,223,2,223,1005,224,404,101,1,223,223,8,226,677,224,102,2,223,223,1005,224,419,101,1,223,223,7,226,677,224,1002,223,2,223,1005,224,434,101,1,223,223,1007,677,677,224,102,2,223,223,1006,224,449,1001,223,1,223,107,226,226,224,1002,223,2,223,1006,224,464,1001,223,1,223,1007,226,226,224,102,2,223,223,1006,224,479,1001,223,1,223,1008,226,226,224,102,2,223,223,1006,224,494,101,1,223,223,7,677,677,224,102,2,223,223,1005,224,509,1001,223,1,223,108,677,226,224,102,2,223,223,1005,224,524,101,1,223,223,1108,677,226,224,1002,223,2,223,1006,224,539,101,1,223,223,1108,677,677,224,102,2,223,223,1005,224,554,101,1,223,223,8,677,226,224,102,2,223,223,1005,224,569,101,1,223,223,8,677,677,224,102,2,223,223,1005,224,584,101,1,223,223,1107,226,226,224,102,2,223,223,1006,224,599,101,1,223,223,108,677,677,224,102,2,223,223,1006,224,614,101,1,223,223,1008,677,226,224,1002,223,2,223,1005,224,629,1001,223,1,223,1107,677,226,224,102,2,223,223,1005,224,644,101,1,223,223,1008,677,677,224,1002,223,2,223,1005,224,659,101,1,223,223,1007,677,226,224,1002,223,2,223,1005,224,674,1001,223,1,223,4,223,99,226";
                    break;
                case AvailablePrograms.Amplification:
                    loadedProgram = AvailablePrograms.Amplification;
                    inputParameters.Add((int)phase);
                    inputParameters.Add((int)signal);
                    //input = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0"; //43210 test case
                    input = $"3,8,1001,8,10,8,105,1,0,0,21,46,63,76,97,118,199,280,361,442,99999,3,9,102,4,9,9,101,2,9,9,1002,9,5,9,101,4,9,9,102,2,9,9,4,9,99,3,9,101,5,9,9,102,3,9,9,101,3,9,9,4,9,99,3,9,1001,9,2,9,102,3,9,9,4,9,99,3,9,1002,9,5,9,101,4,9,9,1002,9,3,9,101,2,9,9,4,9,99,3,9,1002,9,5,9,101,3,9,9,1002,9,5,9,1001,9,5,9,4,9,99,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,99,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,99,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,101,2,9,9,4,9,99,3,9,101,1,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,2,9,9,4,9,99";
                    break;
                case AvailablePrograms.BOOST:
                    loadedProgram = AvailablePrograms.BOOST;
                    input = "1102,34463338,34463338,63,1007,63,34463338,63,1005,63,53,1102,1,3,1000,109,988,209,12,9,1000,209,6,209,3,203,0,1008,1000,1,63,1005,63,65,1008,1000,2,63,1005,63,904,1008,1000,0,63,1005,63,58,4,25,104,0,99,4,0,104,0,99,4,17,104,0,99,0,0,1102,1,1,1021,1101,0,21,1009,1101,0,28,1005,1102,1,27,1015,1102,39,1,1016,1102,1,30,1003,1102,25,1,1007,1102,195,1,1028,1101,0,29,1010,1102,26,1,1004,1102,1,555,1024,1102,32,1,1014,1101,0,23,1019,1102,1,31,1008,1101,652,0,1023,1102,20,1,1000,1101,0,821,1026,1102,814,1,1027,1102,1,36,1017,1101,0,38,1006,1102,1,37,1011,1102,33,1,1001,1102,35,1,1013,1102,190,1,1029,1102,1,22,1018,1101,0,0,1020,1102,1,34,1012,1102,24,1,1002,1101,0,655,1022,1102,1,546,1025,109,37,2106,0,-9,4,187,1106,0,199,1001,64,1,64,1002,64,2,64,109,-32,1202,1,1,63,1008,63,38,63,1005,63,225,4,205,1001,64,1,64,1106,0,225,1002,64,2,64,109,6,1206,10,241,1001,64,1,64,1106,0,243,4,231,1002,64,2,64,109,-12,1207,2,32,63,1005,63,259,1106,0,265,4,249,1001,64,1,64,1002,64,2,64,109,2,2101,0,0,63,1008,63,33,63,1005,63,291,4,271,1001,64,1,64,1106,0,291,1002,64,2,64,109,21,1205,-1,305,4,297,1106,0,309,1001,64,1,64,1002,64,2,64,109,-10,2108,29,-7,63,1005,63,329,1001,64,1,64,1106,0,331,4,315,1002,64,2,64,109,-15,2107,26,10,63,1005,63,347,1106,0,353,4,337,1001,64,1,64,1002,64,2,64,109,13,21107,40,41,2,1005,1012,375,4,359,1001,64,1,64,1106,0,375,1002,64,2,64,109,7,21107,41,40,-5,1005,1012,391,1105,1,397,4,381,1001,64,1,64,1002,64,2,64,109,-6,21102,42,1,2,1008,1013,40,63,1005,63,421,1001,64,1,64,1105,1,423,4,403,1002,64,2,64,109,-10,2107,23,1,63,1005,63,441,4,429,1105,1,445,1001,64,1,64,1002,64,2,64,109,3,1201,5,0,63,1008,63,21,63,1005,63,467,4,451,1106,0,471,1001,64,1,64,1002,64,2,64,109,18,21108,43,43,-5,1005,1017,489,4,477,1105,1,493,1001,64,1,64,1002,64,2,64,109,-29,1207,7,21,63,1005,63,511,4,499,1106,0,515,1001,64,1,64,1002,64,2,64,109,23,21108,44,46,-6,1005,1010,531,1106,0,537,4,521,1001,64,1,64,1002,64,2,64,109,11,2105,1,-3,4,543,1001,64,1,64,1106,0,555,1002,64,2,64,109,-3,1205,-4,571,1001,64,1,64,1105,1,573,4,561,1002,64,2,64,109,-7,2108,21,-8,63,1005,63,595,4,579,1001,64,1,64,1105,1,595,1002,64,2,64,109,-1,1208,-8,28,63,1005,63,615,1001,64,1,64,1106,0,617,4,601,1002,64,2,64,109,-12,1202,4,1,63,1008,63,29,63,1005,63,641,1001,64,1,64,1106,0,643,4,623,1002,64,2,64,109,18,2105,1,1,1105,1,661,4,649,1001,64,1,64,1002,64,2,64,109,-6,2102,1,-8,63,1008,63,31,63,1005,63,687,4,667,1001,64,1,64,1106,0,687,1002,64,2,64,109,-7,21102,45,1,6,1008,1015,45,63,1005,63,709,4,693,1106,0,713,1001,64,1,64,1002,64,2,64,109,-6,2101,0,0,63,1008,63,31,63,1005,63,737,1001,64,1,64,1105,1,739,4,719,1002,64,2,64,109,7,1208,-8,24,63,1005,63,761,4,745,1001,64,1,64,1105,1,761,1002,64,2,64,109,-12,2102,1,10,63,1008,63,32,63,1005,63,781,1106,0,787,4,767,1001,64,1,64,1002,64,2,64,109,16,1206,6,801,4,793,1106,0,805,1001,64,1,64,1002,64,2,64,109,14,2106,0,-1,1001,64,1,64,1106,0,823,4,811,1002,64,2,64,109,-18,1201,-7,0,63,1008,63,27,63,1005,63,847,1001,64,1,64,1105,1,849,4,829,1002,64,2,64,109,-8,21101,46,0,10,1008,1012,46,63,1005,63,875,4,855,1001,64,1,64,1106,0,875,1002,64,2,64,109,13,21101,47,0,-3,1008,1012,44,63,1005,63,899,1001,64,1,64,1105,1,901,4,881,4,64,99,21101,27,0,1,21102,1,915,0,1105,1,922,21201,1,11564,1,204,1,99,109,3,1207,-2,3,63,1005,63,964,21201,-2,-1,1,21101,942,0,0,1105,1,922,22101,0,1,-1,21201,-2,-3,1,21101,0,957,0,1106,0,922,22201,1,-1,-2,1105,1,968,21202,-2,1,-2,109,-3,2105,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
                    break;
            }
            memory = Array.ConvertAll(input.Split(','), long.Parse);
        }

        internal void ProvideSignal(int input)
        {
            inputParameters[1] = input;
        }

        internal int FindNounAndVerbForOutput(int desired_output)
        {
            int solution = 0;
            for (int i = 0; i < 99; i++)
            {
                for (int j = 0; j < 99; j++)
                {
                    setProgramState(i, j);
                    Process();
                    if (memory[0] == desired_output)
                    {
                        solution = 100 * i + j;
                        break;
                    }
                    else
                    {
                        initializeProgram(AvailablePrograms.GravityAssist);
                    }
                }
                if(memory[0] == desired_output)
                {
                    break;
                }
                initializeProgram(AvailablePrograms.GravityAssist);
            }
            return solution;
        }

        internal int GetTESTDiagnosticCode()
        {
            initializeProgram(AvailablePrograms.TEST);
            Process();
            return 0;
        }

        internal void SetProgramAlarm()
        {
            setProgramState(12, 2);
        }

        private void setProgramState(int noun, int verb)
        {
            memory[1] = noun;
            memory[2] = verb;
        }

        internal void Process()
        {
            while (!Ended)
            {
                long num1 = 0, num2 = 0, update_place = 0;
                int opcode = 0;
                long instruction_val = memory[instruction_pointer];
                string instruct = instruction_val.ToString("0000");
                int[] instruction = instruct.Select(c => Convert.ToInt32(c.ToString())).ToArray();
                opcode = instruction[instruction.Length - 1] + instruction[instruction.Length - 2] * 10;
                int param_count = OpCodeDictionary.GetOpcodeInstructionCount(opcode) - 1;
                long addr(int i)
                {
                    return GetMode(instruction_pointer, i) switch
                    {
                        ParameterModes.Position => memory[instruction_pointer + i],
                        ParameterModes.Immediate => instruction_pointer + i,
                        ParameterModes.Relative => relativeBase + memory[instruction_pointer + i],
                        _ => throw new ArgumentException()
                    };
                }
                long arg(int i) => memory[addr(i)];
                if (opcode != OpCodes.End)
                {
                    num1 = memory[instruction_pointer + 1];
                    num2 = memory[instruction_pointer + 2];
                }
                bool pointer_modified = false;
                if (opcode == OpCodes.Add || opcode == OpCodes.Multiply || opcode == OpCodes.LessThan || opcode == OpCodes.Equal)
                {
                    update_place = memory[instruction_pointer + 3]; //Parameters that an instruction writes to will never be in immediate mode.
                }
                switch (opcode)
                {
                    case OpCodes.Add:
                        memory[addr(3)] = arg(1) + arg(2);
                        break;
                    case OpCodes.Multiply:
                        memory[addr(3)] = arg(1) * arg(2);
                        break;
                    case OpCodes.SaveInput:
                        if (loadedProgram == AvailablePrograms.Amplification)
                        {
                            int input = 0;
                            if (inputCount > 0) { input = inputParameters[1]; }
                            else { input = inputParameters[inputCount]; }
                            memory[addr(1)] = input;
                        }
                        else
                        {
                            Console.WriteLine("Please Enter Input:");
                            int input = Convert.ToInt32(Console.ReadLine());
                            memory[addr(1)] = input;
                        }
                        inputCount++;
                        break;
                    case OpCodes.ReturnOutput:
                        Outputs.Add(arg(1));
                        break;
                    case OpCodes.JumpIfTrue:
                        instruction_pointer = arg(1) != 0 ? arg(2) : instruction_pointer + 3;
                        pointer_modified = true;
                        break;
                    case OpCodes.JumpIfFalse:
                        instruction_pointer = arg(1) == 0 ? arg(2) : instruction_pointer + 3;
                        pointer_modified = true;
                        break;
                    case OpCodes.LessThan:
                        memory[addr(3)] = arg(1) < arg(2) ? 1 : 0;
                        break;
                    case OpCodes.Equal:
                        memory[addr(3)] = arg(1) == arg(2) ? 1 : 0;
                        break;
                    case OpCodes.AdjustRelativeBaseOffset:
                        relativeBase += arg(1);
                        break;
                    case OpCodes.End:
                        Ended = true;
                        return;
                    default: throw new ArgumentException("invalid opcode " + opcode);
                }
                if (!pointer_modified)
                {
                    instruction_pointer = instruction_pointer + OpCodeDictionary.GetOpcodeInstructionCount(opcode);
                }
            }
        }

        internal long ReturnValue(int v)
        {
            return memory[v];
        }
    }

    static class OpCodes
    {
        public const int Add = 1;
        public const int Multiply = 2;
        public const int SaveInput = 3;
        public const int ReturnOutput = 4;
        public const int JumpIfTrue = 5;
        public const int JumpIfFalse = 6;
        public const int LessThan = 7;
        public const int Equal = 8;
        public const int AdjustRelativeBaseOffset = 9;
        public const int End = 99;
    }

    public static class OpCodeDictionary
    {
        private static Dictionary<int, int> OpCode_Dict = new Dictionary<int, int>
        {
            {OpCodes.Add, 4 },
            {OpCodes.Multiply, 4 },
            {OpCodes.SaveInput, 2 },
            {OpCodes.ReturnOutput, 2 },
            {OpCodes.JumpIfTrue, 3 },
            {OpCodes.JumpIfFalse, 3 },
            {OpCodes.LessThan, 4 },
            {OpCodes.Equal, 4 },
            {OpCodes.AdjustRelativeBaseOffset, 2 },
            {OpCodes.End, 0 }
        };

        public static int GetOpcodeInstructionCount(int opcode)
        {
            // Try to get the result in the static Dictionary
            int count = 0;
            OpCode_Dict.TryGetValue(opcode, out count);
            return count;
        }
    }

    static class AvailablePrograms
    {
        public const int GravityAssist = 0;
        public const int TEST = 1;
        public const int Amplification = 2;
        public const int BOOST = 3;
    }

    enum ParameterModes
    {
        Position = 0,
        Immediate = 1,
        Relative = 2
    }
}
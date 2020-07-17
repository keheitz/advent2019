using System;
using System.Collections.Generic;

namespace Advent2019
{
    internal class AmplificationCircuit
    {
        List<Amplifier> AmplifierSeries { get; set; } = new List<Amplifier>();
        LinkedList<Amplifier> AmplifierCircuit { get; set; } = new LinkedList<Amplifier>();

        public AmplificationCircuit(AmplifierConfigs config, int[] settings)
        {
            if(config == AmplifierConfigs.Series)
            {
                foreach (var setting in settings)
                {
                    Amplifier amplifier = new Amplifier(setting);
                    AmplifierSeries.Add(amplifier);
                }
            }
            else if(config == AmplifierConfigs.CircularList)
            {
                foreach (var setting in settings)
                {
                    Amplifier amplifier = new Amplifier(setting);
                    AmplifierCircuit.AddLast(new LinkedListNode<Amplifier>(amplifier));
                }
            }
        }

        public long RunSeries(AmplifierConfigs config)
        {
            long lastOutput = 0;
            if(config == AmplifierConfigs.Series)
            {
                foreach (var amplifier in AmplifierSeries)
                {
                    amplifier.Input = lastOutput;
                    amplifier.Computer = new IntcodeComputer(AvailablePrograms.Amplification, amplifier.Phase, amplifier.Input);
                    amplifier.Computer.Process();
                    lastOutput = amplifier.Computer.Outputs[amplifier.Computer.Outputs.Count - 1];
                }
            }
            return lastOutput;
        }

        public int RunCircuit()
        {
            return 0;
        }
    }

    internal class Amplifier
    {
        public IntcodeComputer Computer { get; set; }

        public long Phase { get; set; }

        public long Input { get; set; }

        public Amplifier(int phase)
        {
            Phase = phase;
        }
    }

    internal class PhaseSettingPermutations
    {
        public List<int[]> PhaseSettingList { get; set; } = new List<int[]>();

        public PhaseSettingPermutations(int[] settings)
        {
            HeapPermutation(settings, settings.Length);
        }

        public void HeapPermutation(int[] A, int n)
        {
            if (n == 1)
            {
               PhaseSettingList.Add((int[])A.Clone());
            }
            else
            {
                for (int i = 0; i < n - 1; i++)
                {
                    HeapPermutation(A, n - 1);

                    if (n % 2 == 0)
                    {
                        A = Swap(A, i, n - 1);
                    }
                    else
                    {
                        A = Swap(A, 0, n - 1);
                    }

                }
                HeapPermutation(A, n - 1);
            }
        }


        static int[] Swap(int[] A, int x, int y)
        {
            int temp;
            temp = A[x];
            A[x] = A[y];
            A[y] = temp;

            return A;
        }
    }

    public enum AmplifierConfigs
    {
        Series = 0,
        CircularList = 1
    }

}
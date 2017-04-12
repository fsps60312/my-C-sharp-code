using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Accord.Controls;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;

namespace SampleApplication1
{
    class StopWatch
    {
        DateTime startTime;
        TimeSpan answer;
        public void Start()
        {
            answer = new TimeSpan(0, 0, 0);
            startTime = DateTime.Now;
        }
        public void Pause()
        {
            answer += DateTime.Now - startTime;
        }
        public void Resume()
        {
            startTime = DateTime.Now;
        }
        public TimeSpan ReadTime()
        {
            return answer + (DateTime.Now - startTime);
        }
    }
    class Program
    {
        static Random rand = new Random();
        static string Train2(int n)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(n);
            ans.Append('\t');
            Console.WriteLine($"n = {n}");
            StopWatch learningTime = new StopWatch(), calculatingTime = new StopWatch();
            learningTime.Start(); learningTime.Pause();
            calculatingTime.Start(); calculatingTime.Pause();
            int errorCount = 0;
            for (int counter = 0; counter < 10; counter++)
            {
                //Accord.MachineLearning.VectorMachines.Learning.
                Accord.Neuro.Networks.DeepBeliefNetwork neuroNetwork = new Accord.Neuro.Networks.DeepBeliefNetwork(2, 5, 10, 20, 50, 100, 199);
                Accord.Neuro.Learning.DeepNeuralNetworkLearning deepLearning = new Accord.Neuro.Learning.DeepNeuralNetworkLearning(neuroNetwork);
                Console.WriteLine(deepLearning.Algorithm);
                //Console.WriteLine(deepLearning.LayerCount);
                //deepLearning.LayerCount = neuroNetwork.Layers.Length;
                //deepLearning.LayerIndex = 0;
                //Console.Write(deepLearning.Algorithm);
                Console.WriteLine(deepLearning.LayerCount);
                learningTime.Resume();
                for (int i = 0; i < n; i++)
                {
                    int a = rand.Next(0, 100), b = rand.Next(0, 100);
                    double[] output = new double[199];
                    for (int j = 0; j <= 198; j++) output[j] = (j == a + b ? 1.0 : 0.0);
                    double[] input = new double[2];
                    input[0] = a / 100.0;
                    input[1] = b / 100.0;
                    Console.WriteLine(i.ToString());
                    deepLearning.Run(input, output);
                };
                learningTime.Pause();
                calculatingTime.Resume();
                for (int i = 0; i < 100000; i++)
                {
                    int a = rand.Next(0, 100), b = rand.Next(0, 100);
                    double[] output = neuroNetwork.Compute(new double[] { a / 100.0, b / 100.0 });
                    int best = 0;
                    for (int j = 0; j <= 198; j++) if (output[j] > output[best]) best = j;
                    if (best != a+b) errorCount++;
                }
                calculatingTime.Pause();
            }
            {
                var t = learningTime.ReadTime();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                Console.WriteLine($"learning time = {t}");
            }
            {
                var t = calculatingTime.ReadTime();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                ans.Append(errorCount);
                Console.WriteLine($"calculating time = {t}, error={errorCount}");
            }
            return ans.ToString();
        }
        static string Train(int n)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(n);
            ans.Append('\t');
            Console.WriteLine($"n = {n}");
            StopWatch learningTime = new StopWatch(), calculatingTime = new StopWatch();
            learningTime.Start();learningTime.Pause();
            calculatingTime.Start(); calculatingTime.Pause();
            int errorCount = 0;
            for (int counter = 0; counter < 10; counter++)
            {
                List<double[]> inputs = new List<double[]>();
                List<bool> outputs = new List<bool>();
                for (int i = 0; i < n; i++)
                {
                    double a = rand.Next(0, 100) / 100.0, b = rand.Next(0, 100) / 100.0;
                    //Console.WriteLine($"{a},{b}");
                    inputs.Add(new double[] { a, b });
                    outputs.Add((a + b) / 2.0 < 0.5);
                }
                // Now, we can create the sequential minimal optimization teacher
                var learn = new SequentialMinimalOptimization<Gaussian>()
                {
                    UseComplexityHeuristic = true,
                    UseKernelEstimation = true
                };
                learningTime.Resume();
                // And then we can obtain a trained SVM by calling its Learn method
                SupportVectorMachine<Gaussian> svm = learn.Learn(inputs.ToArray(), outputs.ToArray());
                learningTime.Pause();
                // Finally, we can obtain the decisions predicted by the machine:
                calculatingTime.Resume();
                for (int i = 0; i < 100000; i++)
                {
                    //Console.WriteLine($"{inputs[i][0]} X {inputs[i][1]} = {svm.Decide(inputs[i])} ({outputs[i]})");
                    double a = rand.Next(0, 100) / 100.0, b = rand.Next(0, 100) / 100.0;
                    if (svm.Decide(new double[] { a, b }) != ((a + b) / 2.0 < 0.5)) errorCount++;
                }
                calculatingTime.Pause();
            }
            {
                var t = learningTime.ReadTime();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                Console.WriteLine($"learning time = {t}");
            }
            {
                var t = calculatingTime.ReadTime();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                ans.Append(errorCount);
                Console.WriteLine($"calculating time = {t}, error={errorCount}");
            }
            return ans.ToString();
        }
        [MTAThread]
        static void Main(string[] args)
        {
            //{
            //    Accord.Neuro.Networks.DeepBeliefNetwork neuroNetwork = new Accord.Neuro.Networks.DeepBeliefNetwork(2, 2);
            //    Accord.Neuro.Learning.DeepNeuralNetworkLearning deepLearning = new Accord.Neuro.Learning.DeepNeuralNetworkLearning(neuroNetwork);
            //    deepLearning.Run(new double[2], new double[2]);
            //}
            //Accord.Neuro.Networks.DeepBeliefNetwork neuroNetwork = new Accord.Neuro.Networks.DeepBeliefNetwork(2, 5, 10);
            //neuroNetwork.Randomize();
            //Console.WriteLine(neuroNetwork.OutputCount);
            //foreach (var a in neuroNetwork.Machines)
            //{
            //    Console.WriteLine(a.InputsCount);
            //    a.Randomize();
            //}
            //Accord.Neuro.Learning.DeepNeuralNetworkLearning deepLearning = new Accord.Neuro.Learning.DeepNeuralNetworkLearning(neuroNetwork);
            //Console.WriteLine(deepLearning.LayerCount);
            ////deepLearning.Algorithm = new Accord.Neuro.Learning.ActivationNetworkLearningConfigurationFunction((Accord.Neuro.ActivationNetwork network, int index) => { return new Accord.Neuro.Learning.DeepNeuralNetworkLearning(neuroNetwork); });
            //deepLearning.Run(new double[2], new double[10]);
            // As an example, we will try to learn a decision machine 
            // that can replicate the "exclusive-or" logical function:
            StreamWriter writer = new StreamWriter("data.txt", false, Encoding.UTF8);
            Train(5000);
            for (int i=10;i<=5000;i+=Math.Max(10,i/50))
            {
                writer.WriteLine(Train(i));
            }
            writer.Close();
            Console.WriteLine("done!");
            Console.ReadKey();
        }
    }
}

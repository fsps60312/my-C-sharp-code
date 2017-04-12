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
        public void Start()
        {
            startTime = DateTime.Now;
        }
        public TimeSpan Stop()
        {
            return DateTime.Now - startTime;
        }
    }
    class Program
    {
        static Random rand = new Random();
        static StopWatch stopWatch = new StopWatch();
        [MTAThread]
        /*static string Train2(int n)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(n);
            ans.Append('\t');
            Console.WriteLine($"n = {n}");
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();
            for (int i = 0; i < n; i++)
            {
                double a = rand.Next(0, 100) / 100.0, b = rand.Next(0, 100) / 100.0;
                //Console.WriteLine($"{a},{b}");
                inputs.Add(new double[] { a, b });
                outputs.Add(new double[] { (a + b) / 2.0 });
            }

            stopWatch.Start();
            Accord.Neuro.Learning.DeepNeuralNetworkLearning aaaa = new Accord.Neuro.Learning.DeepNeuralNetworkLearning(new Accord.Neuro.Networks.DeepBeliefNetwork(2, new Accord.Neuro.Networks.RestrictedBoltzmannMachine[] { }));
            // And then we can obtain a trained SVM by calling its Learn method
            {
                double result=aaaa.RunEpoch(inputs.ToArray(), outputs.ToArray());
                Console.WriteLine($"run result = {result}");
                var t = stopWatch.Stop();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                Console.WriteLine($"learning time = {t}");
            }
            // Finally, we can obtain the decisions predicted by the machine:
            stopWatch.Start();
            int errorCount = 0;
            for (int i = 0; i < 100000; i++)
            {
                //Console.WriteLine($"{inputs[i][0]} X {inputs[i][1]} = {svm.Decide(inputs[i])} ({outputs[i]})");
                double a = rand.Next(0, 100) / 100.0, b = rand.Next(0, 100) / 100.0;
                if (svm.Decide(new double[] { a, b }) != ((a + b) / 2.0 < 0.5)) errorCount++;
            }
            {
                var t = stopWatch.Stop();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                ans.Append(errorCount);
                Console.WriteLine($"calculating time = {t}, error={errorCount}");
            }
            return ans.ToString();
        }*/
        static string Train(int n)
        {
            StringBuilder ans = new StringBuilder();
            ans.Append(n);
            ans.Append('\t');
            Console.WriteLine($"n = {n}");
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

            stopWatch.Start();
            // And then we can obtain a trained SVM by calling its Learn method
            SupportVectorMachine<Gaussian> svm = learn.Learn(inputs.ToArray(), outputs.ToArray());
            {
                var t = stopWatch.Stop();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                Console.WriteLine($"learning time = {t}");
            }
            // Finally, we can obtain the decisions predicted by the machine:
            stopWatch.Start();
            int errorCount = 0;
            for (int i = 0; i < 100000; i++)
            {
                //Console.WriteLine($"{inputs[i][0]} X {inputs[i][1]} = {svm.Decide(inputs[i])} ({outputs[i]})");
                double a = rand.Next(0, 100) / 100.0, b = rand.Next(0, 100) / 100.0;
                if (svm.Decide(new double[] { a, b }) != ((a + b) / 2.0 < 0.5)) errorCount++;
            }
            {
                var t = stopWatch.Stop();
                ans.Append(t.TotalMilliseconds);
                ans.Append('\t');
                ans.Append(errorCount);
                Console.WriteLine($"calculating time = {t}, error={errorCount}");
            }
            return ans.ToString();
        }
        static void Main(string[] args)
        {
            // As an example, we will try to learn a decision machine 
            // that can replicate the "exclusive-or" logical function:
            StreamWriter writer = new StreamWriter("data.txt", false, Encoding.UTF8);
            for(int i=10;i<=5000;i+=Math.Max(10,i/50))
            {
                writer.WriteLine(Train(i));
            }
            writer.Close();
            Console.WriteLine("done!");
            Console.ReadKey();
        }
    }
}

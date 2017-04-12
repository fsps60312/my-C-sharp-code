using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.MachineLearning.VectorMachines;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Controls;

namespace Accord_Practice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            double[][] inputs =
            {
                /* 1.*/ new double[] { 0, 0 },
                /* 2.*/ new double[] { 1, 0 }, 
                /* 3.*/ new double[] { 0, 1 }, 
                /* 4.*/ new double[] { 1, 1 },
            };
            int[] outputs =
            { 
                /* 1. 0 xor 0 = 0: */ -1,
                /* 2. 1 xor 0 = 1: */ +1,
                /* 3. 0 xor 1 = 1: */ +1,
                /* 4. 1 xor 1 = 0: */ -1,
            };
            // Create a new machine with a polynomial kernel and two inputs 
            var ksvm = new KernelSupportVectorMachine(new Gaussian(), 2);

            // Create the learning algorithm with the given inputs and outputs
            var smo = new SequentialMinimalOptimization(machine: ksvm, inputs: inputs, outputs: outputs)
            {
                Complexity = 100 // Create a hard-margin SVM 
            };


            // Teach the machine 
            double error = smo.Run();

            Console.WriteLine("error:" + error);

            // Show results on screen 
            ScatterplotBox.Show("Training data", inputs, outputs);

            ScatterplotBox.Show("SVM results", inputs,
                inputs.Apply(p => System.Math.Sign(ksvm.Compute(p))));

            Console.ReadKey();

        }
    }
}

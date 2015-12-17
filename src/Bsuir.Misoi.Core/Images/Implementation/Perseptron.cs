using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class Perceptron
    {
        [Serializable]
        private struct TrainedPerceptron
        {
            public int inputLayerSize;
            public int hiddenLayerSize;
            public int outputLayerSize;
            public double[] v0;
            public double[,] v;
            public double[] w0;
            public double[,] w;
        }

        private const double ErrorTreshold = 0.02;
        private const double LearningSpeed = 0.1;

        private int InputLayerSize { get; set; }
        private int HiddenLayerSize { get; set; }
        private int OutputLayerSize { get; set; }
        private int[] X { get; set; }
        private double[] V0 { get; set; }
        private double[,] V { get; set; }
        private double[] Z_in { get; set; }
        private double[] Z { get; set; }
        private double[] W0 { get; set; }
        private double[,] W { get; set; }
        private double[] Y_in { get; set; }
        private double[] Y { get; set; }

        public Perceptron(int inputLayerSize, int hiddenLayerSize, int outputLayerSize)
        {
            InputLayerSize = inputLayerSize;
            HiddenLayerSize = hiddenLayerSize;
            OutputLayerSize = outputLayerSize;
            X = new int[InputLayerSize];
            V0 = new double[HiddenLayerSize];
            V = new double[InputLayerSize, HiddenLayerSize];
            Z_in = new double[HiddenLayerSize];
            Z = new double[HiddenLayerSize];
            W0 = new double[OutputLayerSize];
            W = new double[HiddenLayerSize, OutputLayerSize];
            Y_in = new double[OutputLayerSize];
            Y = new double[OutputLayerSize];
        }

        private void Initialize()
        {
            Random random = new Random();
            double beta = 0.7 * Math.Pow(HiddenLayerSize, 1.0 / InputLayerSize);
            for (int i = 0; i < HiddenLayerSize; i++)
            {
                V0[i] = random.NextDouble() * 2 * beta - beta;
            }
            for (int i = 0; i < OutputLayerSize; i++)
            {
                W0[i] = random.NextDouble() * 2 * beta - beta;
            }
            for (int i = 0; i < InputLayerSize; i++)
            {
                for (int j = 0; j < HiddenLayerSize; j++)
                {
                    V[i, j] = random.NextDouble() - 0.5;
                }
            }
            double[] vNorms = new double[HiddenLayerSize];
            for (int j = 0; j < HiddenLayerSize; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < InputLayerSize; i++)
                {
                    sum += Math.Pow(V[i, j], 2);
                }
                vNorms[j] = Math.Sqrt(sum);
            }
            for (int i = 0; i < InputLayerSize; i++)
            {
                for (int j = 0; j < HiddenLayerSize; j++)
                {
                    V[i, j] = beta * V[i, j] / vNorms[j];
                }
            }
            for (int i = 0; i < HiddenLayerSize; i++)
            {
                for (int j = 0; j < OutputLayerSize; j++)
                {
                    W[i, j] = random.NextDouble() - 0.5;
                }
            }
            double[] wNorms = new double[OutputLayerSize];
            for (int j = 0; j < OutputLayerSize; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < HiddenLayerSize; i++)
                {
                    sum += Math.Pow(W[i, j], 2);
                }
                wNorms[j] = Math.Sqrt(sum);
            }
            for (int i = 0; i < HiddenLayerSize; i++)
            {
                for (int j = 0; j < OutputLayerSize; j++)
                {
                    W[i, j] = beta * W[i, j] / wNorms[j];
                }
            }
        }

        public double[] Classify(int[] input)
        {
            X = input;
            for (int i = 0; i < HiddenLayerSize; i++)
            {
                Z_in[i] = V0[i];
            }
            for (int i = 0; i < InputLayerSize; i++)
            {
                for (int j = 0; j < HiddenLayerSize; j++)
                {
                    Z_in[j] += X[i] * V[i, j];
                }
            }
            for (int i = 0; i < HiddenLayerSize; i++)
            {
                Z[i] = ActivationFunction(Z_in[i]);
            }
            for (int i = 0; i < OutputLayerSize; i++)
            {
                Y_in[i] = W0[i];
            }
            for (int j = 0; j < HiddenLayerSize; j++)
            {
                for (int k = 0; k < OutputLayerSize; k++)
                {
                    Y_in[k] += Z[j] * W[j, k];
                }
            }
            for (int i = 0; i < OutputLayerSize; i++)
            {
                Y[i] = ActivationFunction(Y_in[i]);
            }
            return Y;
        }

        public void Train(Dictionary<int[], double[]> samples)
        {
            Initialize();
            double maxError;
            do
            {
                maxError = 0;
                foreach (int[] sample in samples.Keys)
                {
                    Classify(sample);
                    double[] sigmaOutput = new double[OutputLayerSize];
                    for (int i = 0; i < OutputLayerSize; i++)
                    {
                        sigmaOutput[i] = (samples[sample][i] - Y[i]) * ActivationFunctionDerivative(Y_in[i]);
                    }
                    for (int i = 0; i < OutputLayerSize; i++)
                    {
                        W0[i] += LearningSpeed * sigmaOutput[i];
                    }
                    for (int i = 0; i < OutputLayerSize; i++)
                    {
                        maxError = Math.Max(maxError, Math.Abs(samples[sample][i] - Y[i]));
                    }
                    double[] sigmaHiddenIn = new double[HiddenLayerSize];
                    double[] sigmaHidden = new double[HiddenLayerSize];
                    for (int j = 0; j < HiddenLayerSize; j++)
                    {
                        for (int k = 0; k < OutputLayerSize; k++)
                        {
                            sigmaHiddenIn[j] += sigmaOutput[k] * W[j, k];
                        }
                    }
                    for (int i = 0; i < HiddenLayerSize; i++)
                    {
                        sigmaHidden[i] = sigmaHiddenIn[i] * ActivationFunctionDerivative(Z_in[i]);
                    }
                    for (int i = 0; i < HiddenLayerSize; i++)
                    {
                        V0[i] += LearningSpeed * sigmaHidden[i];
                    }
                    for (int j = 0; j < HiddenLayerSize; j++)
                    {
                        for (int k = 0; k < OutputLayerSize; k++)
                        {
                            W[j, k] += LearningSpeed * sigmaOutput[k] * Z[j];
                        }
                    }
                    for (int i = 0; i < InputLayerSize; i++)
                    {
                        for (int j = 0; j < HiddenLayerSize; j++)
                        {
                            V[i, j] += LearningSpeed * sigmaHidden[j] * X[i];
                        }
                    }
                }
            }
            while (maxError > ErrorTreshold);
        }

        private double ActivationFunction(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        private double ActivationFunctionDerivative(double x)
        {
            return ActivationFunction(x) * (1.0 - ActivationFunction(x));
        }

        public void Save(string filename)
        {
            TrainedPerceptron trainedPerceptron = new TrainedPerceptron()
            {
                inputLayerSize = InputLayerSize,
                hiddenLayerSize = HiddenLayerSize,
                outputLayerSize = OutputLayerSize,
                v0 = V0,
                v = V,
                w0 = W0,
                w = W
            };
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, trainedPerceptron);
            stream.Close();
        }

        public static Perceptron FromFile(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            TrainedPerceptron trainedPerceptron = (TrainedPerceptron)formatter.Deserialize(stream);
            stream.Close();
            Perceptron perceptron = new Perceptron(trainedPerceptron.inputLayerSize, trainedPerceptron.inputLayerSize, trainedPerceptron.outputLayerSize)
            {
                V0 = trainedPerceptron.v0,
                V = trainedPerceptron.v,
                W0 = trainedPerceptron.w0,
                W = trainedPerceptron.w,
            };
            return perceptron;
        }
    }
}

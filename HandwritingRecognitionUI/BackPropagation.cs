using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace BackPropagation
{
    class BackPropagation
    {
        private static readonly char[] numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

        public static void Train(int numInput, int numHidden, int numOutput, string trainPath, string weightsPath)
        {
            try
            {
                NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);

                //A total of 10294 weights and biases
                int totalWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
                // arbitrary weights and biases
                double[] weights = GenerateRandomWeights(totalWeights);

                Console.WriteLine("Loading neural network weights and biases");
                nn.SetWeights(weights);

                double eta = 0.95;  // learning rate - controls the maginitude of the increase in the change in weights. found by trial and error.
                double alpha = 0.2; // momentum - to discourage oscillation. found by trial and error.
                Console.WriteLine("Setting learning rate (eta) = " + eta.ToString("F2") + " and momentum (alpha) = " + alpha.ToString("F2"));

                Console.WriteLine("\nEntering main back-propagation compute-update cycle");
                Console.WriteLine("Stopping when sum absolute error <= 0.01 or 10000 iterations\n");
                //string trainPath = @"..\..\..\Train\";
                //string trainPath = @"..\..\..\Train\";
                DirectoryInfo dir = new DirectoryInfo(trainPath);
                FileInfo[] files = dir.GetFiles();
                int ctr = 0;
                string name = String.Empty;
                var testObject = files[ctr];
                double[] xValues = null;
                using (FileStream str = testObject.Open(FileMode.Open))
                {
                    Bitmap bitmap = new Bitmap(str);

                    xValues = GenerateInput(bitmap);
                    bitmap.Dispose();

                    //Helpers.ShowVector(xValues, 2, true);
                    name = testObject.Name;
                }
                double[] tValues = GenerateTargetValues(numOutput, name);
                Console.WriteLine("Target outputs to learn are:");
                //Helpers.ShowVector(tValues, 4, true);

                double[] yValues = nn.ComputeOutputs(xValues); // prime the back-propagation loop
                double error = Error(tValues, yValues);
                double[] errors = new double[files.Length];
                //while (ctr < files.Length && error > 0.01)
                while (ctr < 1500 && error > 0.1)
                {
                    int i = 0;
                    foreach (var file in files)
                    {
                        using (FileStream stream = file.Open(FileMode.Open))
                        {
                            Console.WriteLine("Computing new outputs:");
                            Bitmap image = new Bitmap(stream);

                            xValues = GenerateInput(image);
                            image.Dispose();
                            Console.WriteLine("\nComputing new error");
                            tValues = GenerateTargetValues(numOutput, file.Name);
                            Console.WriteLine("Updating weights and biases using back-propagation");
                            nn.UpdateWeights(tValues, eta, alpha);
                            yValues = nn.ComputeOutputs(xValues);
                            errors[i] = Error(tValues, yValues);
                            i++;
                            Console.WriteLine("Error = " + error.ToString("F4"));
                        }
                    }
                    error = 0.0;
                    for (int j = 0; j < errors.Length; j++)
                    {
                        error += errors[j];
                    }
                    error /= errors.Length;
                    ctr++;
                }
                Console.WriteLine("===================================================");
                Console.WriteLine("\nBest weights and biases found:");
                double[] bestWeights = nn.GetWeights();
                string path = @"..\..\..\" + weightsPath + ".txt";
                WriteWeightsToFile(bestWeights, path);

                Console.WriteLine("End Neural Network Back-Propagation\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal: " + ex.Message);
                Console.ReadLine();
            }
        }

        public static string Test(int numInput, int numHidden, int numOutput, string trainPath)
        {
            try
            {
                int totalWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
                string path = @"..\..\..\trained.txt";
                double[] weights = LoadWeightsFromFile(path, totalWeights);

                Console.WriteLine("Loading neural network weights and biases");
                NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);
                nn.SetWeights(weights);

                //string trainPath = @"..\..\..\Test\";
                DirectoryInfo dir = new DirectoryInfo(trainPath);
                FileInfo[] files = dir.GetFiles();
                List<Interval> intervals = CreateIntervals();
                StringBuilder recognizedLetters = new StringBuilder();
                foreach (var file in files)
                {
                    using (FileStream stream = file.Open(FileMode.Open))
                    {
                        Console.WriteLine("Computing outputs for image {0}: ", file.Name);
                        Bitmap image = new Bitmap(stream);

                        double[] xValues = GenerateInput(image);
                        image.Dispose();
                        double[] yValues = nn.ComputeOutputs(xValues);
                        double result = 0.0;
                        for (int i = 0; i < yValues.Length; i++)
                        {
                            result += yValues[i];
                        }
                        result = (double) (result/yValues.Length);
                        recognizedLetters.Append(ReturnLetter(result, intervals));
                    }
                }
                return recognizedLetters.ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static string ReturnLetter(double value, List<Interval> intervals)
        {
            StringBuilder result = new StringBuilder();
            foreach (var interval in intervals)
            {
                if (value >= interval.Min && value <= interval.Max)
                {
                    result.Append(interval.Letter + ", ");
                }
            }
            return result.ToString();
        }

        private static double[] LoadWeightsFromFile(string filename, int totalWeights)
        {
            double[] weights = new double[totalWeights];
            int i = 0;
            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    while (reader.Peek() >= 0)
                    {
                        var line = reader.ReadLine();
                        double value;
                        if (Double.TryParse(line, out value))
                        {
                            weights[i++] = value;
                        }
                        else
                        {
                            Console.Write("Invalid value encountered!");
                        }
                    }
                    return weights;
                }

            }
            return null;
        }

        public static List<Interval> CreateIntervals()
        {
            List<Interval> intervals = new List<Interval>();
            intervals.Add(new Interval { Min = 0.0, Max = 0.033, Letter = "А" });
            intervals.Add(new Interval { Min = 0.033, Max = 0.066, Letter = "Б" });
            intervals.Add(new Interval { Min = 0.066, Max = 0.1, Letter = "В" });
            intervals.Add(new Interval { Min = 0.1, Max = 0.13, Letter = "Г" });
            intervals.Add(new Interval { Min = 0.13, Max = 0.16, Letter = "Д" });
            intervals.Add(new Interval { Min = 0.16, Max = 0.2, Letter = "Е" });
            intervals.Add(new Interval { Min = 0.2, Max = 0.23, Letter = "Ж" });
            intervals.Add(new Interval { Min = 0.23, Max = 0.26, Letter = "З" });
            intervals.Add(new Interval { Min = 0.26, Max = 0.3, Letter = "И" });
            intervals.Add(new Interval { Min = 0.3, Max = 0.33, Letter = "Й" });
            intervals.Add(new Interval { Min = 0.33, Max = 0.36, Letter = "К" });
            intervals.Add(new Interval { Min = 0.36, Max = 0.4, Letter = "Л" });
            intervals.Add(new Interval { Min = 0.4, Max = 0.43, Letter = "М" });
            intervals.Add(new Interval { Min = 0.43, Max = 0.46, Letter = "Н" });
            intervals.Add(new Interval { Min = 0.46, Max = 0.5, Letter = "О" });
            intervals.Add(new Interval { Min = 0.5, Max = 0.53, Letter = "П" });
            intervals.Add(new Interval { Min = 0.53, Max = 0.56, Letter = "Р" });
            intervals.Add(new Interval { Min = 0.56, Max = 0.6, Letter = "С" });
            intervals.Add(new Interval { Min = 0.6, Max = 0.63, Letter = "Т" });
            intervals.Add(new Interval { Min = 0.63, Max = 0.66, Letter = "У" });
            intervals.Add(new Interval { Min = 0.66, Max = 0.7, Letter = "Ф" });
            intervals.Add(new Interval { Min = 0.7, Max = 0.73, Letter = "Х" });
            intervals.Add(new Interval { Min = 0.73, Max = 0.76, Letter = "Ц" });
            intervals.Add(new Interval { Min = 0.76, Max = 0.8, Letter = "Ч" });
            intervals.Add(new Interval { Min = 0.8, Max = 0.83, Letter = "Ш" });
            intervals.Add(new Interval { Min = 0.83, Max = 0.86, Letter = "Щ" });
            intervals.Add(new Interval { Min = 0.86, Max = 0.9, Letter = "Ъ" });
            intervals.Add(new Interval { Min = 0.9, Max = 0.93, Letter = "ъ (ер малък)" });
            intervals.Add(new Interval { Min = 0.93, Max = 0.96, Letter = "Ю" });
            intervals.Add(new Interval { Min = 0.96, Max = 1.0, Letter = "Я" });
            return intervals;
        }

        private static void WriteWeightsToFile(double[] weights, string path)
        {
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    for (int i = 0; i < weights.Length; i++)
                    {
                        sw.WriteLine(weights[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("File trained.txt already exists!!!");
            }
        }

        private static double[] GenerateRandomWeights(int totalNumOfWeights)
        {
            double[] weights = new double[totalNumOfWeights];
            Random rand = new Random();
            for (int i = 0; i < totalNumOfWeights; i++)
            {
                //[-0.05;0.05]
                double roundedRandom = Math.Round(rand.NextDouble(), 3, MidpointRounding.AwayFromZero);
                weights[i] = -0.05 + (double)(roundedRandom / 10);
            }
            return weights;
        }

        private static double[] GenerateInput(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            double[] inputs = new double[width * height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = image.GetPixel(i, j);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));
                    //inputs will be in the range of [-1;1]
                    //(double)(-1 + 2 * ((double)grayScale / 255))
                    //(double)grayScale / 255
                    inputs[j + i * width] = (double)(-0.05 + ((double)grayScale / 2550));
                }
            }

            return inputs;
        }

        private static double[] GenerateTargetValues(int numOfOutputs, string fileName)
        {
            string letter = fileName.Replace("tn_", "").Replace(".bmp", "").Trim(numbers);
            double expectedResult;
            switch (letter)
            {
                case "a":
                    expectedResult = 0.0;
                    break;
                case "be":
                    expectedResult = 0.033;
                    break;
                case "ve":
                    expectedResult = 0.066;
                    break;
                case "ge":
                    expectedResult = 0.1;
                    break;
                case "de":
                    expectedResult = 0.13;
                    break;
                case "e":
                    expectedResult = 0.16;
                    break;
                case "j":
                    expectedResult = 0.2;
                    break;
                case "z":
                    expectedResult = 0.23;
                    break;
                case "i":
                    expectedResult = 0.26;
                    break;
                case "ik":
                    expectedResult = 0.3;
                    break;
                case "k":
                    expectedResult = 0.33;
                    break;
                case "l":
                    expectedResult = 0.36;
                    break;
                case "m":
                    expectedResult = 0.4;
                    break;
                case "n":
                    expectedResult = 0.43;
                    break;
                case "o":
                    expectedResult = 0.46;
                    break;
                case "p":
                    expectedResult = 0.5;
                    break;
                case "r":
                    expectedResult = 0.53;
                    break;
                case "s":
                    expectedResult = 0.56;
                    break;
                case "t":
                    expectedResult = 0.6;
                    break;
                case "u":
                    expectedResult = 0.63;
                    break;
                case "f":
                    expectedResult = 0.66;
                    break;
                case "h":
                    expectedResult = 0.7;
                    break;
                case "ts":
                    expectedResult = 0.73;
                    break;
                case "ch":
                    expectedResult = 0.76;
                    break;
                case "sh":
                    expectedResult = 0.8;
                    break;
                case "st":
                    expectedResult = 0.83;
                    break;
                case "uh":
                    expectedResult = 0.86;
                    break;
                case "erm":
                    expectedResult = 0.9;
                    break;
                case "iu":
                    expectedResult = 0.93;
                    break;
                case "ia":
                    expectedResult = 0.96;
                    break;
                default:
                    expectedResult = 1;
                    break;
            }
            double[] targets = new double[numOfOutputs];
            for (int i = 0; i < numOfOutputs; i++)
            {
                targets[i] = expectedResult;
            }
            return targets;
        }

        static double Error(double[] target, double[] output) // sum absolute error. could put into NeuralNetwork class.
        {
            double sum = 0.0;
            for (int i = 0; i < target.Length; ++i)
                sum += Math.Abs(target[i] - output[i]);
            return sum;
        }

        

    } // class BackPropagation

    public class Interval
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public string Letter { get; set; }
    }

    public class NeuralNetwork
    {
        private int numInput;
        private int numHidden;
        private int numOutput;

        private double[] inputs;
        private double[][] ihWeights; // input-to-hidden
        private double[] ihSums;
        private double[] ihBiases;
        private double[] ihOutputs;

        private double[][] hoWeights;  // hidden-to-output
        private double[] hoSums;
        private double[] hoBiases;
        private double[] outputs;

        private double[] oGrads; // output gradients for back-propagation
        private double[] hGrads; // hidden gradients for back-propagation

        private double[][] ihPrevWeightsDelta;  // for momentum with back-propagation
        private double[] ihPrevBiasesDelta;

        private double[][] hoPrevWeightsDelta;
        private double[] hoPrevBiasesDelta;

        public NeuralNetwork(int numInput, int numHidden, int numOutput)
        {
            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            inputs = new double[numInput];
            ihWeights = Helpers.MakeMatrix(numInput, numHidden);
            ihSums = new double[numHidden];
            ihBiases = new double[numHidden];
            ihOutputs = new double[numHidden];
            hoWeights = Helpers.MakeMatrix(numHidden, numOutput);
            hoSums = new double[numOutput];
            hoBiases = new double[numOutput];
            outputs = new double[numOutput];

            oGrads = new double[numOutput];
            hGrads = new double[numHidden];

            ihPrevWeightsDelta = Helpers.MakeMatrix(numInput, numHidden);
            ihPrevBiasesDelta = new double[numHidden];
            hoPrevWeightsDelta = Helpers.MakeMatrix(numHidden, numOutput);
            hoPrevBiasesDelta = new double[numOutput];
        }

        public void UpdateWeights(double[] tValues, double eta, double alpha) // update the weights and biases using back-propagation, with target values, eta (learning rate), alpha (momentum)
        {
            // assumes that SetWeights and ComputeOutputs have been called and so all the internal arrays and matrices have values (other than 0.0)
            if (tValues.Length != numOutput)
                throw new Exception("target values not same Length as output in UpdateWeights");

            // 1. compute output gradients
            for (int i = 0; i < oGrads.Length; ++i)
            {
                double derivative = (1 - outputs[i]) * outputs[i]; // (1 / 1 + exp(-x))'  -- using output value of neuron
                oGrads[i] = derivative * (tValues[i] - outputs[i]);
            }

            // 2. compute hidden gradients
            for (int i = 0; i < hGrads.Length; ++i)
            {
                double derivative = (1 - ihOutputs[i]) * ihOutputs[i]; // (1 / 1 + exp(-x))'  -- using output value of neuron
                double sum = 0.0;
                for (int j = 0; j < numOutput; ++j) // each hidden delta is the sum of numOutput terms
                    sum += oGrads[j] * hoWeights[i][j]; // each downstream gradient * outgoing weight
                hGrads[i] = derivative * sum;
            }

            // 3. update input to hidden weights (gradients must be computed right-to-left but weights can be updated in any order
            for (int i = 0; i < ihWeights.Length; ++i) // 0..2 (3)
            {
                for (int j = 0; j < ihWeights[0].Length; ++j) // 0..3 (4)
                {
                    double delta = eta * hGrads[j] * inputs[i]; // compute the new delta
                    ihWeights[i][j] += delta; // update
                    ihWeights[i][j] += alpha * ihPrevWeightsDelta[i][j]; // add momentum using previous delta. on first pass old value will be 0.0 but that's OK.
                    ihPrevWeightsDelta[i][j] = delta;
                }
            }

            // 3b. update input to hidden biases
            for (int i = 0; i < ihBiases.Length; ++i)
            {
                double delta = eta * hGrads[i] * 1.0; // the 1.0 is the constant input for any bias; could leave out
                ihBiases[i] += delta;
                ihBiases[i] += alpha * ihPrevBiasesDelta[i];
                ihPrevBiasesDelta[i] = delta;
            }

            // 4. update hidden to output weights
            for (int i = 0; i < hoWeights.Length; ++i)  // 0..3 (4)
            {
                for (int j = 0; j < hoWeights[0].Length; ++j) // 0..1 (2)
                {
                    double delta = eta * oGrads[j] * ihOutputs[i];  // see above: ihOutputs are inputs to next layer
                    hoWeights[i][j] += delta;
                    hoWeights[i][j] += alpha * hoPrevWeightsDelta[i][j];
                    hoPrevWeightsDelta[i][j] = delta;
                }
            }

            // 4b. update hidden to output biases
            for (int i = 0; i < hoBiases.Length; ++i)
            {
                double delta = eta * oGrads[i] * 1.0;
                hoBiases[i] += delta;
                hoBiases[i] += alpha * hoPrevBiasesDelta[i];
                hoPrevBiasesDelta[i] = delta;
            }
        } // UpdateWeights

        public void SetWeights(double[] weights)
        {
            // copy weights and biases in weights[] array to i-h weights, i-h biases, h-o weights, h-o biases
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("The weights array length: " + weights.Length + " does not match the total number of weights and biases: " + numWeights);

            int k = 0; // points into weights param

            for (int i = 0; i < numInput; ++i)
                for (int j = 0; j < numHidden; ++j)
                    ihWeights[i][j] = weights[k++];

            for (int i = 0; i < numHidden; ++i)
                ihBiases[i] = weights[k++];

            for (int i = 0; i < numHidden; ++i)
                for (int j = 0; j < numOutput; ++j)
                    hoWeights[i][j] = weights[k++];

            for (int i = 0; i < numOutput; ++i)
                hoBiases[i] = weights[k++];
        }

        public double[] GetWeights()
        {
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
                for (int j = 0; j < ihWeights[0].Length; ++j)
                    result[k++] = ihWeights[i][j];
            for (int i = 0; i < ihBiases.Length; ++i)
                result[k++] = ihBiases[i];
            for (int i = 0; i < hoWeights.Length; ++i)
                for (int j = 0; j < hoWeights[0].Length; ++j)
                    result[k++] = hoWeights[i][j];
            for (int i = 0; i < hoBiases.Length; ++i)
                result[k++] = hoBiases[i];
            return result;
        }

        public double[] ComputeOutputs(double[] xValues)
        {
            if (xValues.Length != numInput)
                throw new Exception("Inputs array length " + inputs.Length + " does not match NN numInput value " + numInput);

            for (int i = 0; i < numHidden; ++i)
                ihSums[i] = 0.0;
            for (int i = 0; i < numOutput; ++i)
                hoSums[i] = 0.0;

            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
                this.inputs[i] = xValues[i];

            for (int j = 0; j < numHidden; ++j)  // compute input-to-hidden weighted sums
                for (int i = 0; i < numInput; ++i)
                    ihSums[j] += this.inputs[i] * ihWeights[i][j];

            for (int i = 0; i < numHidden; ++i)  // add biases to input-to-hidden sums
                ihSums[i] += ihBiases[i];

            for (int i = 0; i < numHidden; ++i)   // determine input-to-hidden output
                ihOutputs[i] = SigmoidFunction(ihSums[i]);

            for (int j = 0; j < numOutput; ++j)   // compute hidden-to-output weighted sums
                for (int i = 0; i < numHidden; ++i)
                    hoSums[j] += ihOutputs[i] * hoWeights[i][j];

            for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
                hoSums[i] += hoBiases[i];

            for (int i = 0; i < numOutput; ++i)   // determine hidden-to-output result
                this.outputs[i] = SigmoidFunction(hoSums[i]);

            double[] result = new double[numOutput]; // could define a GetOutputs method instead
            this.outputs.CopyTo(result, 0);

            return result;
        } // ComputeOutputs

        private static double StepFunction(double x) // an activation function that isn't compatible with back-propagation bcause it isn't differentiable
        {
            if (x > 0.0) return 1.0;
            else return 0.0;
        }

        private static double SigmoidFunction(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            else return 1.0 / (1.0 + Math.Exp(-x));
        }
    } // class NeuralNetwork

    // ===========================================================================

    public class Helpers
    {
        public static double[][] MakeMatrix(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        public static void ShowVector(double[] vector, int decimals, bool blankLine)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                if (i > 0 && i % 12 == 0) // max of 12 values per row 
                    Console.WriteLine("");
                if (vector[i] >= 0.0) Console.Write(" ");
                Console.Write(vector[i].ToString("F" + decimals) + " "); // n decimals
            }
            if (blankLine) Console.WriteLine("\n");
        }

        public static void ShowMatrix(double[][] matrix, int numRows, int decimals)
        {
            int ct = 0;
            if (numRows == -1) numRows = int.MaxValue; // if numRows == -1, show all rows
            for (int i = 0; i < matrix.Length && ct < numRows; ++i)
            {
                for (int j = 0; j < matrix[0].Length; ++j)
                {
                    if (matrix[i][j] >= 0.0) Console.Write(" "); // blank space instead of '+' sign
                    Console.Write(matrix[i][j].ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
                ++ct;
            }
            Console.WriteLine("");
        }

    } // class Helpers

} // ns

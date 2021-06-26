using System;
using System.Collections.Generic;
using System.IO;

namespace PA_LR2
{
    class Program
    {
        static void Main(string[] args)
        {
            Muravyi muravyi = new Muravyi(10, 250, 2, 4, 0.6);
            muravyi.Algoritm();
            Console.WriteLine($"Мин длина {muravyi.Lmin}");
            for (int i = 0; i < muravyi.pathMin.Count; i++)
            {
                Console.Write($"{muravyi.pathMin[i] + 1} ");
            }
        }
    }

    class File
    {
        public string path;
        public File(string _path)
        {
            path = _path;
        }
        public void Write(int n)
        {
            Random random = new Random();
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                int k = 1;
                sw.WriteLine(n);
                for (int i = k; i <= n; i++)
                {
                    for (int j = k; j <= n; j++)
                    {
                        if (i == j)
                        {
                            sw.WriteLine($"{i} {j} {0}");
                        }
                        else
                        {
                            sw.WriteLine($"{i} {j} {random.Next(1, 50)}");
                        }
                    }

                    k++;
                }
            }
        }
        public List<((int, int), int)> Read()
        {
            List<((int, int), int)> result = new List<((int, int), int)>();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "250")
                    {
                        ((int, int), int) temp = ConvertForRead(line);
                        result.Add(temp);
                    }
                }
            }

            return result;
        }
        private ((int, int), int) ConvertForRead(string line)
        {
            string[] array = line.Split(' ');
            ((int, int), int) result = ((Convert.ToInt32(array[0]), Convert.ToInt32(array[1])), Convert.ToInt32(array[2]));
            return result;
        }
    }// изменить на 250 в read

    class Matrix
    {
        public double[,] CreateMatrix(List<((int, int), int)> EdgesList, double size)
        {
            double[,] matrix = new double[Convert.ToInt32(size), Convert.ToInt32(size)];

            for (int i = 0; i < EdgesList.Count; i++)
                if (EdgesList[i].Item1.Item1 - 1 != EdgesList[i].Item1.Item2 - 1)
                {
                    matrix[EdgesList[i].Item1.Item2 - 1, EdgesList[i].Item1.Item1 - 1] = (EdgesList[i].Item2);
                    matrix[EdgesList[i].Item1.Item1 - 1, EdgesList[i].Item1.Item2 - 1] = (EdgesList[i].Item2);
                }
            
            
            return matrix;
        }

        public double[,] FeramonMatrix(int size)
        {
            double[,] matrix = new double[size, size];
            
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = 0.11;

            return matrix;
        }
    }

    class Muravyi
    {
        private int kolvo;
        private int size;
        private double Alpha;
        private double Beta;
        private double Ro;
        public double Lmin = Double.PositiveInfinity;
        public List<int> pathMin = new List<int>();
        private double[,] adjasMatrix;
        private double[,] feramonMatrix;

        public Muravyi(int _kolvo, int _size, double _alpha, double _beta, double _ro)
        {
            kolvo = _kolvo;
            size = _size;
            Alpha = _alpha;
            Beta = _beta;
            Ro = _ro;
            File file = new File("index.txt");
            // file.Write(size);
            List<((int, int), int)> EdgesList = file.Read();
            Matrix matrix = new Matrix();
            adjasMatrix = matrix.CreateMatrix(EdgesList, size);
            feramonMatrix = matrix.FeramonMatrix(size);

        }

        public void Algoritm()
        {
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                if (i % 20 == 0 && i != 0)
                {
                    Console.WriteLine($"Мин длина {Lmin}");
                }

                for (int j = 0; j < kolvo; j++)
                {
                    (double, List<int>) min = StepMuravei(random.Next(0, size));
                    ReInitFeramon(min);
                    if (Lmin > min.Item1)
                    {
                        Lmin = min.Item1;
                        pathMin = min.Item2;
                    }
                    // Console.WriteLine($"Мин длина {Lmin}");
                }
            }
        }

        private void ReInitFeramon((double, List<int>) min)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    feramonMatrix[i, j] = (1 - Ro) * feramonMatrix[i, j];
                }
            }

            if (!double.IsPositiveInfinity(Lmin))
            {
                for (int i = 0; i < min.Item2.Count - 1; i++)
                {
                    if (Lmin != min.Item1)
                    {
                        feramonMatrix[min.Item2[i], min.Item2[i + 1]] = feramonMatrix[min.Item2[i], min.Item2[i + 1]] + (Lmin/min.Item1);
                        feramonMatrix[min.Item2[i + 1], min.Item2[i]] = feramonMatrix[min.Item2[i + 1], min.Item2[i]] + (Lmin/min.Item1);
                    }
                }
            }
        }

        private (double, List<int>) StepMuravei(int start)
        {
            (double, List<int>) result = (0, new List<int>());
            result.Item2.Add(start);
            RecursiveStep(ref result, result.Item2[^1]);
            return result;
        }

        private void RecursiveStep(ref (double, List<int>) result, int start)
        {
            for (int i = 0; i < size; i++)
            {
                if (start == i)
                {
                    if (result.Item2[0] == result.Item2[^1] && result.Item2.Count > 1)
                        return;
                    (int, double) temp = SearchP(i, result);
                    result.Item2.Add(temp.Item1);
                    result = (result.Item1 += adjasMatrix[i, temp.Item1], result.Item2);
                    RecursiveStep(ref result, result.Item2[^1]);
                }
            }
        }

        private bool SearchInList(List<int> list, int n) // если не найдет то вернет true
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] == n)
                    return false;
            return true;
        }

        private (int, double) SearchP(int k, (double, List<int>) result)
        {
            List<(int, double)> list = new List<(int, double)>();
            double sum = 0;
            for (int i = 0; i < size; i++)
                if (k != i && SearchInList(result.Item2, i))
                    sum += Math.Pow(1/adjasMatrix[k, i], Beta)*Math.Pow(feramonMatrix[k,i], Alpha);
            for (int i = 0; i < size; i++)
                if (k != i && SearchInList(result.Item2, i))
                    list.Add((i, Math.Round(Math.Pow(1/adjasMatrix[k, i], Beta)*Math.Pow(feramonMatrix[k,i], Alpha)/sum, 3)*1000));

            if (list.Count == 0)
            {
                list.Add((result.Item2[0], 1));
            }
            
            return MaxItem(list);
        }

        private (int, double) MaxItem(List<(int, double)> list)
        {
            (int, double) max = (0, 0);
            if (list.Count > 1)
            {
                List<int> pairList = new List<int>();
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < Convert.ToInt32(list[i].Item2); j++)
                    {
                        pairList.Add(list[i].Item1);
                    }
                }
                Random random = new Random();
                int num = random.Next(0, pairList.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Item1 == pairList[num])
                    {
                        max = list[i];
                        return max;
                    }
                }
            }
            else
            {
                return list[0];
            }
            return max;
            // for (int i = 0; i < list.Count; i++)
            // {
            //     if (max.Item2 < list[i].Item2)
            //     {
            //         max = list[i];
            //     }
            // }
            // return max;
        }
    }
}
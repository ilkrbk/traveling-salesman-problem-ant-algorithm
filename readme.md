# Traveling Salesman Problem

### Run the program

When initializing a class, you can specify different parameters, the result depends on it.

Only changes to the second parameter must match the size of the graph.

```
Muravyi muravyi = new Muravyi(10, 250, 2, 4, 0.6);
```

In the File class in the Write function, a graph is created in the format

```
CountVertex
VertexStart VertexFinish Distance
..... .....
VertexStart VertexFinish Distance
```

And also the result depends on the number of iterations in the loop.

```
public void Algoritm()
{
    Random random = new Random();
    for (int i = 0; i < (#### number of iterations ####); i++)
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
        }
    }
}
```

### Result in console 


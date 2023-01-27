using System.Collections;
using System.Collections.Generic;
using DeBroglie;
using DeBroglie.Models;
using DeBroglie.Topo;
using UnityEngine;
using Resolution = DeBroglie.Resolution;

public class TestGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 5;
    void Start()
    {
        ITopoArray<char> sample = TopoArray.Create(new []
        {
            new []{'#', '#', '#', '#'},
            new []{'#', '-', '-', '#'},
            new []{'#', '+', '+', '#'},
            new []{'#', '-', '-', '#'},
            new []{'#', '#', '#', '#'}
        }, periodic: false);

        var model = new AdjacentModel(sample.ToTiles());
        var topology = new GridTopology(width, height, false);
        var propagator = new TilePropagator(model, topology);
        var resolution = propagator.Run();
        if (resolution == Resolution.Decided)
        {
            var result = propagator.ToValueArray<char>();
            for (var y = 0; y < height; y++)
            {
                var outputStr = "";
                for (var x = 0; x < width; x++)
                {
                    outputStr += result.Get(x, y);
                }
                Debug.Log(outputStr);
            }
        }
        else
        {
            Debug.LogError("Propagation not decided");
        }
    }

}

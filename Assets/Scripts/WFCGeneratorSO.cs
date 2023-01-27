using System.Collections;
using System.Collections.Generic;
using DeBroglie;
using DeBroglie.Models;
using DeBroglie.Topo;
using UnityEngine;
using Resolution = DeBroglie.Resolution;

namespace MapTools
{
    [CreateAssetMenu(menuName = "AIV/WFC Generator", fileName = "WFCGenerator")]
    public class WFCGeneratorSO : AbstractTileGeneratorSO
    {
        public override void Generate(Transform container, int width, int height)
        {
            var sample = GetSample();

            var model = new AdjacentModel(sample.ToTiles());
            var topology = new GridTopology(width, height, false);
            var propagator = new TilePropagator(model, topology);
            var resolution = propagator.Run();
            if (resolution == Resolution.Decided)
            {
                var result = propagator.ToValueArray<char>();
                
                // TODO - Create tile map
            }
            else
            {
                Debug.LogError("Resolution was not decided.");
            }
        }

        private ITopoArray<char> GetSample()
        {
            /*** TEMPORANEO ***/
            ITopoArray<char> sample = TopoArray.Create(new []
            {
                new []{'#', '#', '#', '#'},
                new []{'#', '-', '-', '#'},
                new []{'#', '+', '+', '#'},
                new []{'#', '-', '-', '#'},
                new []{'#', '#', '#', '#'}
            }, periodic: false);
            /********************/

            return sample;
        }
    }
}

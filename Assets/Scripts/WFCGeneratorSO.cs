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
        public string folderPath = "";
        
        [Range(.1f, 100f)]
        public float spacing = 2;

        private WFCTile[,] _mapAr;

        public TextAsset sampleAsset;

        public override void Generate(Transform container, int width, int height)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogError("Wang generator: No path was set");
                return;
            }

            _mapAr = new WFCTile[width, height];
            
            var sample = GetSample();

            var model = new AdjacentModel(sample.ToTiles());
            var topology = new GridTopology(width, height, false);
            var propagator = new TilePropagator(model, topology);
            var resolution = propagator.Run();
            if (resolution == Resolution.Decided)
            {
                var result = propagator.ToValueArray<string>();
                
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var tileId = result.Get(x, y);
                        var filePath = folderPath + tileId;
                        var prefab = Resources.Load<WFCTileInfo>(filePath);
                        var tile = new WFCTile(tileId, prefab);
                        _mapAr[x, y] = tile;
                    }
                }
                DrawMap(container, width, height);
            }
            else
            {
                Debug.LogError("Resolution was not decided.");
            }
        }

        private void DrawMap(Transform container, int width, int height)
        {
            for (var row = 0; row < height ; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var go = Instantiate(_mapAr[col, row].Prefab, container);
                    go.name = "Tile (" + col + ", " + row + ")";
                    go.transform.localPosition = new Vector3(col * spacing, 0, row * spacing);
                }
            } 
        }
        
        private ITopoArray<string> GetSample()
        {
            var str = sampleAsset.text;
            var rows = str.Split('\n');
            var rowNum = rows.Length;
            var colNum = rows[0].Split(',').Length;
            string[,] ar = new string[colNum, rowNum];
            
            for (var y = 0; y < rows.Length; y++)
            {
                var row = rows[y];
                var cols = row.Split(',');
                for (var x = 0; x < cols.Length; x++)
                {
                    Debug.Log(x + " - " + y);
                    ar[x, y] = cols[x];
                }
            }
            
            var sample = TopoArray.Create(ar, periodic: false);
            return sample;
        }
    }
}

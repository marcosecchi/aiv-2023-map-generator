using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    public enum WangGenerationType
    {
        Random,
        Checkerboard,
        PerfectMaze
    }
    
    [CreateAssetMenu(menuName = "AIV/Wang Tile Generator", fileName = "WangTileGenerator")]
    public partial class WangTileGeneratorSO : AbstractTileGeneratorSO
    {
        public string folderPath = "";
        
        [Range(.1f, 100f)]
        public float spacing = 1;

        public WangGenerationType generationType;

        private WangTile[,] _mapAr;
        
        public override void Generate(Transform container, int width, int height)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogError("Wang generator: No path was set");
                return;
            }

            _mapAr = new WangTile[width, height];
            switch (generationType)
            {
                case WangGenerationType.Checkerboard:
                    GenerateCheckerboard(width, height);
                    break;
                case WangGenerationType.PerfectMaze:
                    GeneratePerfectMaze(width, height);
                    break;
                default:
                    GenerateRandom(width, height);
                   break;
            }
            DrawMap(container, width, height);
        }

        private void DrawMap(Transform container, int width, int height)
        {
            for (var row = 0; row < height ; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var go = Instantiate(_mapAr[col, row].Info, container);
                    go.transform.localPosition = new Vector3(col * spacing, 0, row * spacing);
                }
            }
        }
    }
}

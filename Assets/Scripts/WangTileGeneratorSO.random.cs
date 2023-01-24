using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    public partial class WangTileGeneratorSO : AbstractTileGeneratorSO
    {
        private void GenerateRandom(int width, int height)
        {
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var tileId = Random.Range(0, 15);

                    var path = folderPath + " " + tileId;
                    var tilePrefabs = Resources.LoadAll<WangTileInfo>(path);
                    var tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                    
                    var tile = new WangTile(tileId, tilePrefab);
                    _mapAr[col, row] = tile;
                }
            }
        }
    }
}

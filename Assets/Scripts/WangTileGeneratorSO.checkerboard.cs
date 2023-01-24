using UnityEngine;

namespace MapTools
{
    public partial class WangTileGeneratorSO : AbstractTileGeneratorSO
    {
        private void GenerateCheckerboard(int width, int height)
        {
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    if (row % 2 == col % 2)
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

            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    if (row % 2 != col % 2)
                    {
                        var tileId = 0;
                        if (row > 0)
                            if (_mapAr[col, row - 1].HasDoor(DoorDirection.North))
                                tileId += 4;
                        if (row < height - 1)
                            if (_mapAr[col, row + 1].HasDoor(DoorDirection.South))
                                tileId += 1;
                        if (col > 0)
                            if (_mapAr[col - 1, row].HasDoor(DoorDirection.East))
                                tileId += 8;
                        if (col < width - 1)
                            if (_mapAr[col + 1, row].HasDoor(DoorDirection.West))
                                tileId += 2;

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
}

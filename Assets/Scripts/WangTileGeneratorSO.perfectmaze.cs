using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    public enum SelectionMethod
    {
        AlwayFirst,
        AlwayLast,
        FirstPercentSelection,
        LastPercentSelection,
        Random
    }
    
    public partial class WangTileGeneratorSO : AbstractTileGeneratorSO
    {
        private List<Vector2Int> _visited;

        [Range(0, 1f)]
        public float selectionPercent = .5f;

        public SelectionMethod method;
        
        private void GeneratePerfectMaze(int width, int height)
        {
            _visited = new List<Vector2Int>();

            var pos = new Vector2Int(0, 0);
            _mapAr[pos.x, pos.y] = new WangTile(4);
            _visited.Add(pos);

            while (_visited.Count > 0)
            {
                pos = _visited[GetVisitedElement(method)];
                var possibleDirections = GetAvailableExits(pos, width, height);
                if (possibleDirections.Length == 0)
                {
                    var path = folderPath + " " + _mapAr[pos.x, pos.y].Value;
                    var tilePrefabs = Resources.LoadAll<WangTileInfo>(path);
                    var tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
                    _mapAr[pos.x, pos.y].Info = tilePrefab;
                    _visited.Remove(pos);
                    continue;
                }

                var newDirection = possibleDirections[Random.Range(0, possibleDirections.Length)];
                _mapAr[pos.x, pos.y].AddDoor(newDirection);
                var newTile = new WangTile(0);
                var newPos = pos;
                switch (newDirection)
                {
                    case DoorDirection.North:
                        newTile.AddDoor(DoorDirection.South);
                        newPos.y += 1;
                        break;
                    case DoorDirection.East:
                        newTile.AddDoor(DoorDirection.West);
                        newPos.x += 1;
                        break;
                    case DoorDirection.South:
                        newTile.AddDoor(DoorDirection.North);
                        newPos.y -= 1;
                        break;
                    default:
                        newTile.AddDoor(DoorDirection.East);
                        newPos.x -= 1;
                        break;
                }
                _visited.Add(newPos);
                _mapAr[newPos.x, newPos.y] = newTile;
            }
        }

        private DoorDirection[] GetAvailableExits(Vector2Int pos, int width, int height)
        {
            var directions = new List<DoorDirection>();

            if (pos.x > 0 && _mapAr[pos.x - 1, pos.y] == null)
            {
                directions.Add(DoorDirection.West);
            }
            if(pos.x < width - 1 && _mapAr[pos.x + 1, pos.y] == null)
            {
                directions.Add(DoorDirection.East);
            }
            if (pos.y > 0 && _mapAr[pos.x, pos.y -1] == null)
            {
                directions.Add(DoorDirection.South);
            }
            if(pos.y < height - 1 && _mapAr[pos.x, pos.y + 1] == null)
            {
                directions.Add(DoorDirection.North);
            }
            
            return directions.ToArray();
        }
        
        private int GetVisitedElement(SelectionMethod selMethod)
        {
            switch (selMethod)
            {
                case SelectionMethod.AlwayFirst:
                    return 0;
                case SelectionMethod.AlwayLast:
                    return _visited.Count - 1;
                case SelectionMethod.FirstPercentSelection:
                    return Random.Range(0, 1f) < selectionPercent ? 0 : Random.Range(0, _visited.Count);
                case SelectionMethod.LastPercentSelection:
                    return Random.Range(0, 1f) < selectionPercent ? _visited.Count - 1 : Random.Range(0, _visited.Count);
                default:
                    return Random.Range(0, _visited.Count);
            }
            
        }
    }
}

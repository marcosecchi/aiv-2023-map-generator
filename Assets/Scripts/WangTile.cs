using UnityEngine;

namespace MapTools
{
    public enum DoorDirection
    {
        North = 1,
        East = 2,
        South = 4,
        West = 8
    }

    public class WangTile
    {
        private int _value;

        public WangTile(int value, WangTileInfo info = null)
        {
            value = Mathf.Clamp(value, 0, 15);
            _value = value;
            Info = info;
        }

    //    public WangTileInfo Info => _info;
        public int Value => _value;

        public WangTileInfo Info { get; set; }

        public bool HasDoor(DoorDirection direction)
        {
            switch (direction)
            {
                case DoorDirection.North:
                    return (_value & 1) == 1;
                case DoorDirection.East:
                    return (_value & 2) == 2;
                case DoorDirection.South:
                    return (_value & 4) == 4;
                case DoorDirection.West:
                    return (_value & 8) == 8;
                default:
                    return false;
            }
        }

        public bool AddDoor(DoorDirection direction)
        {
            if (HasDoor(direction)) return false;

            _value |= (int)direction;
            return true;
        }
    }
}

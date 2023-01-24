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
        private WangTileInfo _info;

        public WangTile(int value, WangTileInfo info = null)
        {
            value = Mathf.Clamp(value, 0, 15);
            _value = value;
            _info = info;
        }

        public WangTileInfo Info => _info;

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

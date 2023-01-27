using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    public class WFCTile
    {
        public WFCTile(string value, WFCTileInfo prefab)
        {
            Value = value;
            Prefab = prefab;
        }

        public string Value { get; }
        public WFCTileInfo Prefab { get; }
    }
}

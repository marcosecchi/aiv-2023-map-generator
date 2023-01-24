using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    public abstract class AbstractTileGeneratorSO : ScriptableObject
    {
        public abstract void Generate(Transform container, int width, int height);
    }
}

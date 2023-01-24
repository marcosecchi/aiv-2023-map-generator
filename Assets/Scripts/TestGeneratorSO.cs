using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapTools
{
    [CreateAssetMenu(menuName = "AIV/Test Generator", fileName = "TestGenerator")]
    public class TestGeneratorSO : AbstractTileGeneratorSO
    {
        public GameObject prefab;
        
        [Range(.1f, 100f)]
        public float spacing;
        
        public override void Generate(Transform container, int width, int height)
        {
            for (var row = 0; row < height; row++)
            {
                for (var col = 0; col < width; col++)
                {
                    var go = Instantiate(prefab);
                    go.transform.SetParent(container);
                    go.name = "Tile (" + col + ", " + row + ")";
                    go.transform.localPosition = new Vector3(col * spacing, Random.Range(-.5f, .5f), row * spacing);
                }
                
            }
        }
    }
    
}

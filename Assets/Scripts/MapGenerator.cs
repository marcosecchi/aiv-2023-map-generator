using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapTools
{
    // The tool window to generate the maps
    public class MapGenerator : EditorWindow
    {
        // Map size
        private int _width;
        private int _height;

        // Map container
        private Transform _container;

        // A reference to the map generator
        private AbstractTileGeneratorSO _mapGenerator;

        // Map size
        private BoxCollider _boundsCollider;
        private Vector2 _mapStart;
        private Vector2 _mapEnd;
        private Vector2Int _numLevels;

        // Opens the tool window
        [MenuItem("Window/AIV/Map Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MapGenerator));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
            //_width = EditorGUILayout.IntField("Width", _width);
            _width = EditorGUILayout.IntSlider(
                                "Width",
                                _width,
                                1,
                                100);
            _height = EditorGUILayout.IntSlider(
                                "Height",
                                _height,
                                1,
                                100);

            _container = (Transform)EditorGUILayout.ObjectField(
                                "Container",
                                _container,
                                typeof(Transform),
                                true);

            _mapGenerator = (AbstractTileGeneratorSO)EditorGUILayout.ObjectField(
                                "Generator",
                                _mapGenerator,
                                typeof(AbstractTileGeneratorSO),
                                false
                            );
            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("Generation", EditorStyles.boldLabel);

            GUI.enabled = (_container != null) && (_mapGenerator != null);
            if (GUILayout.Button("Generate"))
            {
//                EditorUtility.DisplayProgressBar("Map Generation", "Generating map", 0);
                _mapGenerator.Generate(_container, _width, _height);
//                EditorUtility.ClearProgressBar();
            }

            if (_container == null)
            {
                EditorGUILayout.HelpBox("You must select a container!", MessageType.Warning);
            }
            if (_mapGenerator == null)
            {
                EditorGUILayout.HelpBox("You must select a map generator!", MessageType.Warning);
            }
            
            GUI.enabled = (_container != null) && _container.childCount > 0;
            if (GUILayout.Button("Clear All"))
            {
     //           EditorUtility.DisplayProgressBar("Map Generation", "Cleaning up the map container", 0);
                _container.DestroyAllChildrenImmediate();
     //           EditorUtility.ClearProgressBar();
            }

            GUI.enabled = true;

            EditorGUILayout.Space(10);
            
            EditorGUILayout.LabelField("Levels Generator", EditorStyles.boldLabel);
            
            _boundsCollider = (BoxCollider)EditorGUILayout.ObjectField(
                "Bounds",
                _boundsCollider,
                typeof(BoxCollider),
                true
            );

            _numLevels = EditorGUILayout.Vector2IntField(
                "Number of Levels",
                _numLevels);
            GUI.enabled = (_boundsCollider != null && _numLevels is { x: > 0, y: > 0 });
            if (GUILayout.Button("Generate Levels"))
            {
                GenerateLevels();
            }
        }

        private void GenerateLevels()
        {
            var currentScene = SceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(currentScene, currentScene.path);
            var currentScenePath = currentScene.path;
            // make currentscene additive
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Additive);
            var container = _container;


            var bounds = _boundsCollider.bounds;
            _mapStart = bounds.min;
            _mapEnd = bounds.max;
            var levelSizeX = (bounds.max.x - bounds.min.y) / _numLevels.x;
            var levelSizeZ = (bounds.max.z - bounds.min.z) / _numLevels.y;

            // check if a folder exists
            if (!AssetDatabase.IsValidFolder("Assets/Scenes/_GENERATED"))
            {
                AssetDatabase.CreateFolder("Assets/Scenes", "_GENERATED");
            }
            
            for (var x = 0; x < _numLevels.x; x++)
            {
                for (var y = 0; y < _numLevels.y; y++)
                {
                    var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                    scene.name = "Level (" + x + " - " + y + ")";

                    var boundsX = x * _numLevels.x;
                    var boundsZ = y * _numLevels.y;
                    
                    var children = new List<GameObject>();
                    foreach (Transform child in container)
                    {
                        children.Add(child.gameObject);
                    }
                    
                    // move all children to the new scene
                    foreach (var child in children)
                    {
                        if (child.transform.position.x >= boundsX && child.transform.position.x <= boundsX + levelSizeX &&
                            child.transform.position.z >= boundsZ && child.transform.position.z <= boundsZ + levelSizeZ)
                        {
                            child.transform.SetParent(null);
                            EditorSceneManager.MoveGameObjectToScene(child, scene);
                        }
                    }
                    EditorSceneManager.SaveScene(scene, "assets/scenes/_GENERATED/" + scene.name + ".unity");
                }
            }

            EditorSceneManager.OpenScene(currentScenePath);

        }
    }
}

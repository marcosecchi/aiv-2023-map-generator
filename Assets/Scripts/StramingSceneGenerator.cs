using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapTools
{
    public class StramingSceneGenerator : EditorWindow
    {

        private Transform _container;

        private Vector2Int _numLevels;

        private BoxCollider _boundsCollider;
        
        [MenuItem("Window/AIV/Streaming Scene Generator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(StramingSceneGenerator));
        }

        private void OnGUI()
        {
            _container = (Transform)EditorGUILayout.ObjectField(
                "Container",
                _container,
                typeof(Transform),
                true);

            _numLevels = EditorGUILayout.Vector2IntField(
                "Num Levels",
                _numLevels);

            _boundsCollider = (BoxCollider)EditorGUILayout.ObjectField(
                "Bounds collider",
                _boundsCollider,
                typeof(BoxCollider),
                true
                );

            if (GUILayout.Button("Generate"))
            {
                Generate();
            }
        }

        private void Generate()
        {
            var activeScene = SceneManager.GetActiveScene();
            var activeScenePath = activeScene.path;
            EditorSceneManager.SaveScene(activeScene, activeScenePath);
           // EditorSceneManager.OpenScene(activeScenePath, OpenSceneMode.Additive);

            var bounds = _boundsCollider.bounds;
            var mapStart = bounds.min;
            var mapEnd = bounds.max;
            var levelSizeX = (mapEnd.x - mapStart.x) / _numLevels.x;
            var levelSizeY = (mapEnd.z - mapStart.z) / _numLevels.y;

            if (!AssetDatabase.IsValidFolder(C.SCENE_FOLDER_PATH + "/" + C.GENERATED_FOLDER))
            {
                AssetDatabase.CreateFolder(C.SCENE_FOLDER_PATH, C.GENERATED_FOLDER);
            }

            for (var x = 0; x < _numLevels.x; x++)
            {
                for (var y = 0; y < _numLevels.y; y++)
                {
                    var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                    scene.name = C.BASE_SCENE_NAME + " (" + x + ", " + y + ")";

                    var children = new List<GameObject>();
                    foreach (Transform child in _container)
                    {
                        children.Add(child.gameObject);
                    }

                    var boundsX = x * levelSizeX + mapStart.x;
                    var boundsY = y * levelSizeY + mapStart.z;
                    foreach (var child in children)
                    {
                        if (
                            child.transform.position.x >= boundsX &&
                            child.transform.position.x <= boundsX + levelSizeX &&
                            child.transform.position.z >= boundsY &&
                            child.transform.position.z <= boundsY + levelSizeY)
                        {
                            child.transform.SetParent(null);
                            EditorSceneManager.MoveGameObjectToScene(child, scene);
                        }
                    }

                    EditorSceneManager.SaveScene(
                        scene,
                        C.SCENE_FOLDER_PATH + "/" + C.GENERATED_FOLDER + "/" + scene.name + ".unity"
                        );
                    
                }
            }

            EditorSceneManager.OpenScene(activeScenePath);

        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
                                2,
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
                EditorUtility.DisplayProgressBar("Map Generation", "Generating map", 0);
                _mapGenerator.Generate(_container, _width, _height);
                EditorUtility.ClearProgressBar();
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
                EditorUtility.DisplayProgressBar("Map Generation", "Cleaning up the map container", 0);
                while (_container.childCount> 0)
                {
                    DestroyImmediate(_container.GetChild(0).gameObject);
                }
                EditorUtility.ClearProgressBar();
            }
        }

        private void Generate()
        {
        
        }
    }
}

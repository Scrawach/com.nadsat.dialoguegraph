using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Exporters
{
    public class PngExporter
    {
        private readonly EditorWindow _window;
        private readonly GraphView _graph;
        private readonly string _savePath = "/Screenshots";

        private bool _isProcessing;
        private IEnumerator _processing;
        private float _lastTime;

        public PngExporter(EditorWindow window, GraphView graph)
        {
            _window = window;
            _graph = graph;
        }

        public void Export() =>
            StartExport();

        private void StartExport() =>
            EditorApplication.update += OnEditorUpdate;

        private void StopExport() =>
            EditorApplication.update -= OnEditorUpdate;

        private void OnEditorUpdate()
        {
            if (_processing == null)
                _processing = Processing();

            var t = 1f;
            if (_processing.Current is float value)
            {
                Debug.Log($"{_processing.Current}");
                t = Mathf.Max(t, value);
            }

            if (_lastTime + t < Time.realtimeSinceStartup)
            {
                _lastTime = Time.realtimeSinceStartup;
                Debug.Log($"{_lastTime}!");
                _isProcessing = _processing.MoveNext();
            }
            
            if (!_isProcessing)
                StopExport();
        }

        private IEnumerator Processing()
        {
            const float offset = 50;
            var windowScreen = _graph.worldBound;
            windowScreen.position += _window.position.position;

            var graphPosition = _graph.viewTransform.position;
            var nodesArea = GetGraphArea(_graph, offset);

            _graph.viewTransform.position = -1 * nodesArea.position;
            var numberOfTiles = Vector2Int.CeilToInt(nodesArea.size / _graph.worldBound.size);
            return ScreenTiles(windowScreen, _window, _graph, numberOfTiles);
        }

        private IEnumerator ScreenTiles(Rect screenPosition, EditorWindow window, GraphView view, Vector2Int numberOfTiles)
        {
            var tileSize = Vector2Int.CeilToInt(view.worldBound.size);
            var pixels = new Color[tileSize.x * tileSize.y * numberOfTiles.x * numberOfTiles.y];
            for (var x = 0; x < numberOfTiles.x; x++)
            for (var y = 0; y < numberOfTiles.y; y++)
            {
                var position = -1 * new Vector2(x, y) * tileSize;
                view.viewTransform.position += (Vector3) position;
                window.Repaint();
                yield return 1.25f;
                var screenPixels = ReadScreenPixels(screenPosition);
                var savedPath = SaveAsPng(tileSize, screenPixels, "result");
                Debug.Log($"{savedPath}");
            }
        }
        
        private static Rect GetGraphArea(GraphView view, float offset)
        {
            var area = view.nodes.First().GetPosition();
            
            foreach (var rect in view.nodes.Select(node => node.GetPosition()))
            {
                area.xMax = Mathf.Max(area.xMax, rect.xMax + offset);
                area.yMax = Mathf.Max(area.yMax, rect.yMax + offset);
                area.xMin = Mathf.Min(area.xMin, rect.xMin - offset);
                area.yMin = Mathf.Min(area.yMin, rect.yMin - offset);
            }

            return area;
        }
        
        private Color[] ReadScreenPixels(Rect readRect) =>
            UnityEditorInternal.InternalEditorUtility.ReadScreenPixel(readRect.position, (int) readRect.width, (int) readRect.height);

        public string SaveAsPng(Vector2Int size, Color[] pixels, string filename)
        {
            var texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
            texture.SetPixels(pixels, 0);
            texture.Apply();

            var bytes = texture.EncodeToPNG();
            Object.DestroyImmediate(texture, true);

            Directory.CreateDirectory(Application.dataPath + _savePath);
            var path = GetUniqueName(filename);
            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
            return path;
        }
        
        private string GetUniqueName(string name)
        {
            var path = $"{Application.dataPath}{_savePath}/{name}.png";
            var counter = 0;
            
            while (File.Exists(path))
            {
                path = $"{Application.dataPath}{_savePath}/{name}{counter:000}.png";
                counter++;
            }

            return path;
        }
    }
}
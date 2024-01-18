using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Exporters
{
    public class PngExporter
    {
        private readonly EditorWindow _window;
        private readonly GraphView _graph;
        private readonly string _savePath = "/Screenshots";

        public PngExporter(EditorWindow window, GraphView graph)
        {
            _window = window;
            _graph = graph;
        }

        public void Export()
        {
            const float offset = 50;
            var windowScreen = _graph.worldBound;
            windowScreen.position += _window.position.position;

            var graphPosition = _graph.viewTransform.position;
            var nodesArea = GetGraphArea(_graph, offset);

            _graph.viewTransform.position = -1 * nodesArea.position;
            var numberOfTiles = nodesArea.size / _graph.worldBound.size;
            Debug.Log($"{numberOfTiles}");
            
            Debug.Log($"{windowScreen}");
            var pixels = ReadScreenPixels(windowScreen);
            var savedPath = SaveAsPng(new Vector2Int((int) windowScreen.size.x, (int) windowScreen.size.y), pixels, "result");
            Debug.Log($"{savedPath}");
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
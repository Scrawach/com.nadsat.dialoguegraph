using System;
using UnityEngine;

namespace Runtime.Nodes
{
    [Serializable]
    public class Vector2Data
    {
        public float X;
        public float Y;

        public Vector2Data() { }
        
        public Vector2Data(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Rect(Vector2Data data) =>
            new Rect(data, Vector2.zero);

        public static implicit operator Vector2Data(Rect data) =>
            new Vector2Data(data.position.x, data.position.y);

        public static implicit operator Vector2(Vector2Data data) =>
            new Vector2(data.X, data.Y);
    }
}
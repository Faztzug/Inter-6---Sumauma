using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JW
{
    [System.Serializable]

    public class Orbit
    {
        public float xAxis;
        public float yAxis;

        public Orbit(float xAxis, float yAxis)
        {
            this.xAxis = xAxis; this.yAxis = yAxis;
        }

        public Vector2 Evaluate(float t)
        {
            float angle = Mathf.Deg2Rad * 360 * t;
            float x = Mathf.Sin(angle) * xAxis;
            float y = Mathf.Cos(angle) * yAxis;
            return new Vector2(x, y);
        }
    }
}

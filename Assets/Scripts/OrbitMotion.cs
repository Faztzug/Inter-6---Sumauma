using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JW
{
    public class OrbitMotion : MonoBehaviour
    {
        public Transform orbitingObject;
        public Orbit path;

        [Range(0f, 1f)]
        public float orbitProgress = 0f;
        public float orbitPeriod = 3f;
        public bool orbitActive = true;
        void Start()
        {
            if (orbitingObject == null)
            {
                orbitActive = false;
                return;
            }
            SetOrbitPos();
            StartCoroutine(AnimateOrbit());
        }
        void SetOrbitPos()
        {
            Vector2 orbitPos = path.Evaluate(orbitProgress);
            orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
        }

        IEnumerator AnimateOrbit()
        {
            if (orbitPeriod < 0.1f)
            {
                orbitPeriod = 0.1f;
            }
            float orbitSpeed = 1f / orbitPeriod;
            while (orbitActive)
            {
                orbitProgress += Time.deltaTime * orbitSpeed;
                orbitProgress %= 1f;
                SetOrbitPos();
                yield return null;
            }
        }
    }
}

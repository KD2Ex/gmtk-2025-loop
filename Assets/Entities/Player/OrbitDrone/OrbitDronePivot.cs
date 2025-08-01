using System;
using UnityEngine;

namespace Entities.OrbitDrone
{
    public class OrbitDronePivot : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        
        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
    }
}
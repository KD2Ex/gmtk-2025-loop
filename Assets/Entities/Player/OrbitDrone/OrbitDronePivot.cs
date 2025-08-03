using System;
using Attacks;
using UnityEngine;

namespace Entities.OrbitDrone
{
    public class OrbitDronePivot : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Attack attack;
        [SerializeField] private float radius;
        
        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
    }
}
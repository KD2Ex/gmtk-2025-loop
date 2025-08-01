using System;
using UnityEngine;

namespace Entities.Modifiers
{
    public class ModifiersController : MonoBehaviour
    {
        protected Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}
using UnityEngine;

namespace Damage
{
    public class DamageMessage
    {
        public float damage;
        public float knockbackForce;
        public float stunForce;
        public Vector2 dir;
        public Transform source;
    }
}
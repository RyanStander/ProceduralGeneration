using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [HideInInspector]public Rigidbody projectileRigidbody;
        public float damageRadius = 1;
        
        private void Reset()
        {
            projectileRigidbody = GetComponent<Rigidbody>();
        }
    }
}

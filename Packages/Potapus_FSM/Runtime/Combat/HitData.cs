using UnityEngine;

namespace EnemyFSM.Combat
{
    // Everything an attack needs to tell a target: how much damage, which direction to fly, who hit it.
    public struct HitData
    {
        public float      Damage;
        public Vector3    Force;
        public Vector3    HitPoint;
        public GameObject Attacker;

        public HitData(float damage, Vector3 force, GameObject attacker) : this()
        {
            Damage   = damage;
            Force    = force;
            Attacker = attacker;
            HitPoint = Vector3.zero;
        }
    }
}

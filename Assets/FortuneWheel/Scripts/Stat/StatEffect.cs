using UnityEngine;

namespace FortuneWheel.Scripts.Stat
{
    [System.Serializable]
    public abstract class StatEffect
    {
        public float amount;
        public abstract void Apply(GameObject target);
    }
}
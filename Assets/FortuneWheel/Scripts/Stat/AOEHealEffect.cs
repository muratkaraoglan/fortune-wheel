using UnityEngine;

namespace FortuneWheel.Scripts.Stat
{
    public class AOEHealEffect : StatEffect
    {
        public float radius;

        public AOEHealEffect()
        {
            EffectType = "AOE Heal";
        }

        public override void Apply(GameObject target)
        {
        }
    }
}
using UnityEngine;
using UnityEngine.Serialization;

namespace FortuneWheel.Scripts.Stat
{
    [System.Serializable]
    public abstract class StatEffect
    {
        [field: SerializeField, HideInInspector] public string EffectType { get; protected set; }

        [field: SerializeField] public float Amount { get; private set; }

        [field: SerializeField] public EffectApplicationType ApplicationType { get; private set; }
        [field: SerializeField] public EffectValueType ValueType { get; private set; }
        [field: SerializeField] public StackingLogic StackingLogic { get; private set; }

        public abstract void Apply(GameObject target); // Cooldown and duration ?
    }
}
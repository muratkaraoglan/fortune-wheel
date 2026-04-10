using UnityEngine;

namespace FortuneWheel.Scripts.Stat
{
    [System.Serializable]
    public abstract class StatEffect
    {
        [field: SerializeField,HideInInspector] public string EffectType { get; protected set; }
     
        public float amount;
                   
        public EffectApplicationType applicationType;
        public EffectValueType valueType;
        public StackingLogic stackingLogic;
        
        public abstract void Apply(GameObject target);// Cooldown and duration ?
    }
}
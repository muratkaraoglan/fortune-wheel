using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public abstract class AbstractWheelConfigData : ScriptableObject
    {
        [field: SerializeField] public bool IsSafeZone { get; private set; }
    }
}
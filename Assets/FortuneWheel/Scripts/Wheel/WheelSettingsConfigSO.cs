using UnityEngine;
using UnityEngine.Serialization;

namespace FortuneWheel.Scripts.Wheel
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Wheel/Wheel Settings Config")]
    public class WheelSettingsConfigSO : ScriptableObject
    {
        [field: SerializeField] public float SpinDuration { get; private set; }
        [field: SerializeField] public int MinRotation { get; private set; }
        [field: SerializeField] public int MaxRotation { get; private set; }

        [field: SerializeField] public bool ClockwiseRotation { get; private set; }
        [field: SerializeField] public AnimationCurve SpinCurve { get; private set; }

        [field: SerializeField] public int SilverSafeZoneFrequency { get; private set; } = 5;
        [field: SerializeField] public int GoldSafeZoneFrequency { get; private set; } = 30;

        [SerializeField] private WheelZoneConfigSO bronzeWheelZoneConfigSo;
        [SerializeField] private WheelZoneConfigSO silverWheelZoneConfigSo;
        [SerializeField] private WheelZoneConfigSO goldWheelZoneConfigSo;

        public WheelZoneConfigSO GetCurrentZone(int spinCount)
        {
            if (spinCount == 1) return silverWheelZoneConfigSo;
            if (spinCount % GoldSafeZoneFrequency == 0) return goldWheelZoneConfigSo;
            if (spinCount % SilverSafeZoneFrequency == 0) return silverWheelZoneConfigSo;
            
            return bronzeWheelZoneConfigSo;
        }
    }
}
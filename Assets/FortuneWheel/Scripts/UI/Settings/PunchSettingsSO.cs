using UnityEngine;

namespace FortuneWheel.Scripts.UI.Settings
{
    [CreateAssetMenu( menuName = "Settings/Punch Animation", order = 1)]
    public class PunchSettingsSO : ScriptableObject
    {
        [Header("Scale Punch")]
        public Vector3 punchAmount = Vector3.one * 0.22f;
        public float duration = 0.28f;
        public int vibrato = 4;
        public float elasticity = 0.5f;

        [Header("Color Flash")]
        public Color flashColor = new(1f, 0.85f, 0.2f, 0.55f);
        public Color normalColor = Color.white;
        public float flashDuration = 0.07f;
        public int flashLoops = 2;
    }
}
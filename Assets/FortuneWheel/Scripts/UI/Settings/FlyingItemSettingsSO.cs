using DG.Tweening;
using UnityEngine;

namespace FortuneWheel.Scripts.UI.Settings
{
    [CreateAssetMenu( menuName = "Settings/Flying Item Animation", order = 1)]
    public class FlyingItemSettingsSO : ScriptableObject
    {
        [Header("Fade & Scale Settings")]
        public float fadeDuration = 0.1f;
        public float scaleInDuration = 0.15f;
        public float initialScaleFactor = 0.2f;

        [Header("Path & Spread Settings")]
        public float spawnSpreadRadius = 100f;
        public float minArcHeight = 80f;
        public float maxArcHeight = 140f;
        public float midPointLerpFactor = 0.5f;

        [Header("Easings")]
        public Ease scaleEase = Ease.OutBack;
        public Ease pathEase = Ease.InQuad;
    }
}
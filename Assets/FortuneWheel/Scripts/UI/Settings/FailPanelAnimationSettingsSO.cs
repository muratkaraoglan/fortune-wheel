using DG.Tweening;
using UnityEngine;

namespace FortuneWheel.Scripts.UI.Settings
{
    [CreateAssetMenu( menuName = "Settings/Fail Panel Animation", order = 1)]
    public class FailPanelAnimationSettingsSO : ScriptableObject
    {
        [field: SerializeField] public float FadeInDuration { get; private set; } = 0.4f;
        
        [field:Header("Fail Image Settings")]
        [field: SerializeField] public float FailImageScaleDuration { get; private set; } = 0.5f;
        [field: SerializeField] public Ease FailImageScaleEase { get; private set; } = Ease.OutBack;
        
        [field:Header("Background Color Settings")]
        [field: SerializeField] public Color BgColorA { get; private set; } = Color.white;
        [field: SerializeField] public Color BgColorB { get; private set; } = Color.red;
        [field: SerializeField] public float BgColorDuration { get; private set; } = 0.7f;
        [field: SerializeField] public int FailPanelBgColorLoops { get; private set; } = -1;
        [field: SerializeField] public LoopType FailPanelBgColorLoopType { get; private set; } = LoopType.Yoyo;
        [field: SerializeField] public Ease FailPanelBgColorEase { get; private set; } = Ease.InOutSine;
        
        [field:Header("Heartbeat Settings")]
        [field: SerializeField] public Vector3 FailImageHeartBeatScale { get; private set; } = new Vector3(1.15f, 1.15f, 1);
        [field: SerializeField] public float FailImageHeartBeatDuration { get; private set; } = 0.7f;
        [field:SerializeField] public int HeartBeatTweenScaleLoops { get; private set; } = -1;
        [field:SerializeField] public LoopType HeartBeatTweenScaleLoopType { get; private set; } = LoopType.Yoyo;
        [field: SerializeField] public Ease HeartbeatScaleEase { get; private set; } = Ease.InOutQuad;
       
        [field:Header("Button Settings")]
        [field: SerializeField] public float ButtonsAppearDelay { get; private set; } = 0.7f;
        [field: SerializeField] public float ButtonScaleDuration { get; private set; } = 0.3f;
        [field: SerializeField] public Ease ButtonScaleEase { get; private set; } = Ease.OutBack;
    }
}
using DG.Tweening;
using FortuneWheel.Scripts.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.UI.Visual
{
    public class PunchFeedback : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private Image background;
        [SerializeField] private PunchSettingsSO  settings;
        
        private Sequence _seq;

        public void Punch()
        {
            _seq?.Kill();
            rect.localScale = Vector3.one;
            _seq = DOTween.Sequence();

            _seq.Append(rect.DOPunchScale(settings.punchAmount, settings.duration, settings.vibrato, settings.elasticity));
            _seq.Join(
                background.DOColor(settings.flashColor, settings.flashDuration)
                    .SetLoops(settings.flashLoops, LoopType.Yoyo)
                    .OnComplete(() => background.color = settings.normalColor)
            );
        }

        private void OnDestroy() => _seq?.Kill();
    }
}
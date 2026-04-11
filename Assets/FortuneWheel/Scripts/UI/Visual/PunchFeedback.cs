using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.UI.Visual
{
    public class PunchFeedback : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private Image background;
        [SerializeField] private Color flashColor = new(1f, 0.85f, 0.2f, 0.55f);
        [SerializeField] private Color normalColor = new(1f, 1f, 1f, 1f);
        
        private Sequence _seq;

        public void Punch()
        {
            _seq?.Kill();
            rect.localScale = Vector3.one;
            _seq = DOTween.Sequence();

            _seq.Append(rect.DOPunchScale(Vector3.one * 0.22f, 0.28f, 4, 0.5f));
            _seq.Join(
                background.DOColor(flashColor, 0.07f)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => background.color = normalColor)
            );
        }

        private void OnDestroy() => _seq?.Kill();
    }
}
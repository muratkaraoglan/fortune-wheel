using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace FortuneWheel.Scripts.UI.Visual
{
    public class AnimatedCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private float punchStrength = 0.4f;

        private Sequence _seq;
        private int _current;

        public void SetImmediate(int value)
        {
            _seq?.Kill();
            _current = value;
            countText.SetText(value.ToString());
            transform.localScale = Vector3.one;
        }

        public void BumpBy(int amount, float duration = 0.3f)
        {
            AnimateTo(_current + amount, duration);
        }

        public void AnimateTo(int target, float duration = 0.4f)
        {
            _seq?.Kill();

            var from = _current;
            _current = target;

            _seq = DOTween.Sequence();

            _seq.Append(DOTween.To(
                    () => from,
                    x =>
                    {
                        from = x;
                        countText.SetText(target.ToString());
                    },
                    target,
                    duration
                ).SetEase(Ease.OutQuart)
            );

            // _seq.Join(
            //     transform.DOPunchScale(Vector3.one * punchStrength, duration, 5, 0.5f));
        }

        private void OnDestroy()
        {
            _seq?.Kill();
        }
    }
}
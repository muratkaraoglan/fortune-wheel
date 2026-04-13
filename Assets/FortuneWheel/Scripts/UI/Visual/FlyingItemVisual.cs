using System;
using DG.Tweening;
using FortuneWheel.Scripts.UI.Settings;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FortuneWheel.Scripts.UI.Visual
{
    public sealed class FlyingItemVisualData
    {
        public Sprite icon;
        public RectTransform spawnPoint;
        public RectTransform targetPoint;
        public float duration;
        public float delay;
        public System.Action onArrived;
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class FlyingItemVisual : BaseVisualController<FlyingItemVisualData>
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private FlyingItemSettingsSO settings;

        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private Sequence _seq;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void OnInitialize(FlyingItemVisualData data)
        {
            iconImage.sprite = data.icon;
            _rect.position = data.spawnPoint.position;
            _rect.localScale = Vector3.one * settings.initialScaleFactor;
            _canvasGroup.alpha = 0f;

            var spread = Random.insideUnitCircle * settings.spawnSpreadRadius;
            _rect.anchoredPosition += spread;

            var start = _rect.position;
            var end = data.targetPoint.position;
            var mid = Vector3.Lerp(start, end, settings.midPointLerpFactor)
                      + Vector3.up * Random.Range(settings.minArcHeight, settings.maxArcHeight);

            _seq = DOTween.Sequence().SetDelay(data.delay);

            _seq.Append(_canvasGroup.DOFade(1f, settings.fadeDuration));
            _seq.Join(_rect.DOScale(1f, settings.scaleInDuration).SetEase(settings.scaleEase));

            _seq.Append(
                _rect.DOPath(
                    new[] { start, mid, end },
                    data.duration,
                    PathType.CatmullRom
                ).SetEase(settings.pathEase)
            );
            _seq.OnComplete(() =>
            {
                data.onArrived?.Invoke();
                Destroy(gameObject);
            });

            _seq.Play();
        }

        protected override void OnClear()
        {
            _seq?.Kill();
        }

        private void OnDestroy() => _seq?.Kill();
    }
}
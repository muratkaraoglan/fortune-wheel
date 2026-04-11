using System;
using DG.Tweening;
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
            _rect.localScale = Vector3.one * 0.2f;
            _canvasGroup.alpha = 0f;
            
            var spread = Random.insideUnitCircle * 100f;
            _rect.anchoredPosition += spread;
            
            var start  = _rect.position;
            var end    = data.targetPoint.position;
            var mid    = Vector3.Lerp(start, end, 0.5f)
                             + Vector3.up * Random.Range(80f, 140f);
            
            _seq = DOTween.Sequence().SetDelay(data.delay);
            
            _seq.Append(_canvasGroup.DOFade(1f, 0.1f));
            _seq.Join(_rect.DOScale(1f, 0.15f).SetEase(Ease.OutBack));
            
            _seq.Append(
                _rect.DOPath(
                    new[] { start, mid, end },
                    data.duration,
                    PathType.CatmullRom
                ).SetEase(Ease.InQuad)
            );
            
            //float fadeSt = data.duration * 0.7f;
            // _seq.Insert(
            //     0.1f + fadeSt,
            //     _rect.DOScale(0.35f, data.duration * 0.3f).SetEase(Ease.InBack)
            // );
            // _seq.Insert(
            //     0.1f + fadeSt,
            //     _canvasGroup.DOFade(0f, data.duration * 0.25f)
            // );
            
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
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.UI.Visual
{
    public class TextScroller : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentText;
        [SerializeField] private TextMeshProUGUI nextText;
        [SerializeField] private float scrollDuration = 0.3f;
        [SerializeField] private Image background;
        
        private RectTransform _currentRect;
        private RectTransform _nextRect;
        private float _containerHeight;
        
        private void Awake()
        {
            _currentRect = currentText.rectTransform;
            _nextRect = nextText.rectTransform;
            _containerHeight = GetComponent<RectTransform>().rect.height;
          
            _currentRect.anchoredPosition = Vector2.zero;
            _nextRect.anchoredPosition = new Vector2(0, -_containerHeight); 
        }

        public void ScrollToValue(string newValue, Color backgroundColor)
        {
            nextText.text = newValue;
            _nextRect.anchoredPosition = new Vector2(0, -_containerHeight);

            var seq = DOTween.Sequence();

            seq.Join(_currentRect.DOAnchorPosY(_containerHeight, scrollDuration).SetEase(Ease.OutBack));
            seq.Join(background.DOColor(backgroundColor, 0.3f).SetEase(Ease.OutBack));
            seq.Join(_nextRect.DOAnchorPosY(0, scrollDuration).SetEase(Ease.OutBack));

            seq.OnComplete(() =>
            {
                var tempRect = _currentRect;
                var tempText = currentText;

                _currentRect = _nextRect;
                currentText = nextText;

                _nextRect = tempRect;
                nextText = tempText;
                
                _nextRect.anchoredPosition = new Vector2(0, -_containerHeight);
              
            });
        }

        public void ScrollToValue(string newValue)
        {
            ScrollToValue(newValue, Color.white);
        }

        public void ResetText(string newValue)
        {
            currentText.text = newValue;
            background.color = Color.white;
        }
    }
}
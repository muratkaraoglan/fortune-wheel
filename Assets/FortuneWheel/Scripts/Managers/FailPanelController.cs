using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.Managers
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public class FailPanelController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image failPanelBgImage;
        [SerializeField] private Image failImage;
        [SerializeField] private RectTransform failImageRect;

        [Header("Buttons")] [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButtonGold;
        [SerializeField] private Button reviveButtonAds;

        [Header("Animation Settings")] [SerializeField]
        private float fadeInDuration = 0.4f;

        [Header("BG Color Settings")] [SerializeField]
        private Color bgColorA = Color.white;

        [SerializeField] private Color bgColorB = Color.red;
        [SerializeField] private float bgColorDuration = 0.5f;

        [Header("Fail Image Settings")] [SerializeField]
        private float failImageScaleDuration = 0.5f;

        [SerializeField] private Vector3 failImageHeartbeatScale = new Vector3(1.15f, 1.15f, 1f);
        [SerializeField] private float failImageHeartbeatDuration = 0.12f;

        [Header("Button Settings")] [SerializeField]
        private float buttonAppearDelay = 0.5f;

        [SerializeField] private float buttonScaleDuration = 0.3f;

        private Sequence _sequence;
        private Tween _bgColorTween;
        private Tween _heartbeatTween;
        private Action<FailPanelResult> _onMakeChoice;

        private void OnValidate()
        {
            var btnGiveUp = transform.Find("GiveUpButton").GetComponent<Button>();
            if (btnGiveUp != null) giveUpButton = btnGiveUp;
            var btnRevive = transform.Find("ReviveButtonGold").GetComponent<Button>();
            if (btnRevive != null) reviveButtonGold = btnRevive;
            var btnAds = transform.Find("ReviveButtonAds").GetComponent<Button>();
            if (btnAds != null) reviveButtonAds = btnAds;
        }

        private void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            failPanelBgImage.color = bgColorB;
            giveUpButton.onClick.AddListener(OnGiveUpButtonClicked);
            reviveButtonGold.onClick.AddListener(OnReviveButtonGoldClicked);
            reviveButtonAds.onClick.AddListener(OnReviveButtonAdsClicked);
        }

        public void OpenFailPanel(Sprite failSprite, Action<FailPanelResult> onMakeChoice)
        {
            failImage.sprite = failSprite;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            _onMakeChoice = onMakeChoice;
            
            _bgColorTween?.Kill();
            _heartbeatTween?.Kill();
            _sequence?.Kill();
            
            _sequence = DOTween.Sequence();

            canvasGroup.alpha = 0;
            failImageRect.localScale = Vector3.zero;
            giveUpButton.transform.localScale = Vector3.zero;
            reviveButtonGold.transform.localScale = Vector3.zero;
            reviveButtonAds.transform.localScale = Vector3.zero;

            _sequence.Append(canvasGroup.DOFade(1f, fadeInDuration));

            _sequence.Append(failImageRect.DOScale(Vector3.one, failImageScaleDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _bgColorTween = failPanelBgImage.DOColor(bgColorA, bgColorDuration)
                        .From(bgColorB)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);

                    _heartbeatTween = failImageRect.DOScale(failImageHeartbeatScale, failImageHeartbeatDuration)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutQuad);
                }));

            _sequence.AppendInterval(buttonAppearDelay);

            _sequence.Append(giveUpButton.transform.DOScale(Vector3.one, buttonScaleDuration).SetEase(Ease.OutBack));
            _sequence.Append(reviveButtonGold.transform.DOScale(Vector3.one, buttonScaleDuration)
                .SetEase(Ease.OutBack));
            _sequence.Append(reviveButtonAds.transform.DOScale(Vector3.one, buttonScaleDuration).SetEase(Ease.OutBack));
        }

        private void CloseFailPanel()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            _bgColorTween?.Kill();
            _heartbeatTween?.Kill();
            _sequence?.Kill();
        }

        private void OnGiveUpButtonClicked()
        {
            CloseFailPanel();
            _onMakeChoice?.Invoke(FailPanelResult.GiveUp);
        }

        private void OnReviveButtonGoldClicked()
        {
            //TODO:check gold?
            CloseFailPanel();
            _onMakeChoice?.Invoke(FailPanelResult.Continue);
        }

        private void OnReviveButtonAdsClicked()
        {
            CloseFailPanel();
            _onMakeChoice?.Invoke(FailPanelResult.Continue);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
            giveUpButton.onClick.RemoveListener(OnGiveUpButtonClicked);
            reviveButtonGold.onClick.RemoveListener(OnReviveButtonGoldClicked);
            reviveButtonAds.onClick.RemoveListener(OnReviveButtonAdsClicked);
        }
    }
}
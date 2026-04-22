using System;
using DG.Tweening;
using FortuneWheel.Scripts.UI.Settings;
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

        [Header("Buttons")] 
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButtonGold;
        [SerializeField] private Button reviveButtonAds;
        
        [SerializeField] private FailPanelAnimationSettingsSO failPanelAnimationSettings;

        private Sequence _sequence;
        private Tween _bgColorTween;
        private Tween _heartbeatTween;
        private Action<FailPanelResult> _onMakeChoice;

        private void OnValidate()
        {
            var btnGiveUp = transform.Find("GiveUpButton").GetComponent<Button>();
            if (btnGiveUp != null && giveUpButton!= null) giveUpButton = btnGiveUp;
            var btnRevive = transform.Find("ReviveButtonGold").GetComponent<Button>();
            if (btnRevive != null && reviveButtonGold!= null) reviveButtonGold = btnRevive;
            var btnAds = transform.Find("ReviveButtonAds").GetComponent<Button>();
            if (btnAds != null && reviveButtonAds!= null) reviveButtonAds = btnAds;
        }

        private void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            failPanelBgImage.color = failPanelAnimationSettings.BgColorB;
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

            _sequence.Append(canvasGroup.DOFade(1f, failPanelAnimationSettings.FadeInDuration));

            _sequence.Append(failImageRect.DOScale(Vector3.one, failPanelAnimationSettings.FailImageScaleDuration)
                .SetEase(failPanelAnimationSettings.FailImageScaleEase)
                .OnComplete(() =>
                {
                    _bgColorTween = failPanelBgImage.DOColor(failPanelAnimationSettings.BgColorA,
                            failPanelAnimationSettings.BgColorDuration)
                        .From(failPanelAnimationSettings.BgColorB)
                        .SetLoops(failPanelAnimationSettings.FailPanelBgColorLoops, 
                            failPanelAnimationSettings.FailPanelBgColorLoopType)
                        .SetEase(failPanelAnimationSettings.FailPanelBgColorEase);

                    _heartbeatTween = failImageRect.DOScale(failPanelAnimationSettings.FailImageHeartBeatScale,
                            failPanelAnimationSettings.FailImageHeartBeatDuration)
                        .SetLoops(failPanelAnimationSettings.HeartBeatTweenScaleLoops,
                            failPanelAnimationSettings.HeartBeatTweenScaleLoopType)
                        .SetEase(failPanelAnimationSettings.HeartbeatScaleEase);
                }));

            _sequence.AppendInterval(failPanelAnimationSettings.ButtonsAppearDelay);

            _sequence.Append(giveUpButton.transform.DOScale(Vector3.one, failPanelAnimationSettings.ButtonScaleDuration)
                .SetEase(failPanelAnimationSettings.ButtonScaleEase));
            _sequence.Append(reviveButtonGold.transform.DOScale(Vector3.one, failPanelAnimationSettings.ButtonScaleDuration)
                .SetEase(failPanelAnimationSettings.ButtonScaleEase));
            _sequence.Append(reviveButtonAds.transform.DOScale(Vector3.one, failPanelAnimationSettings.ButtonScaleDuration)
                .SetEase(failPanelAnimationSettings.ButtonScaleEase));
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
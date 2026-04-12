using System;
using System.Collections.Generic;
using DG.Tweening;
using FortuneWheel.Scripts.UI.Visual;
using FortuneWheel.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.Managers
{
    public class RewardPanelManager : Singleton<RewardPanelManager>
    {
        [SerializeField] private RewardVisualController rewardVisualControllerPrefab;
        [SerializeField] private Transform rewardVisualContainer;
        [SerializeField] private Button claimButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private readonly List<RewardVisualController> _rewardVisualPool = new();
        
        private void OnValidate()
        {
            var btn = transform.Find("ClaimButton").GetComponent<Button>();
            if(btn != null) claimButton = btn;
            else Debug.LogWarning("Claim button not found");
        }

        private void Start()
        {
            claimButton.onClick.AddListener(OnClaimButtonClick);
        }

        private void OnDestroy()
        {
            claimButton.onClick.RemoveListener(OnClaimButtonClick);
        }

        public void ShowRewardPanel(List<RewardVisualData> rewards)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            
            claimButton.transform.localScale = Vector3.zero;
            
            foreach (var visual in _rewardVisualPool)
                visual.gameObject.SetActive(false);
            
            Debug.Log(rewards.Count);

            for (var i = 0; i < rewards.Count; i++)
            {
                RewardVisualController visual;
                
                if (i < _rewardVisualPool.Count)
                {
                    visual = _rewardVisualPool[i];
                    visual.gameObject.SetActive(true);
                }
                else
                {
                    visual = Instantiate(rewardVisualControllerPrefab, rewardVisualContainer);
                    _rewardVisualPool.Add(visual);
                }

                visual.Initialize(rewards[i]);
                
                visual.transform.localScale = Vector3.zero;
                var delay = i * 0.1f;
                visual.transform.DOScale(Vector3.one, 0.4f)
                    .SetDelay(delay)
                    .SetEase(Ease.OutBack);
            }
            
            claimButton.transform.DOScale(Vector3.one, 0.4f)
                .SetDelay(rewards.Count * 0.1f)
                .SetEase(Ease.OutBack);
        }

        private void OnClaimButtonClick()
        {
            foreach (var visual in _rewardVisualPool)
            {
                visual.Clear();
            }
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
using DG.Tweening;
using FortuneWheel.Scripts.Item;
using FortuneWheel.Scripts.Item.Enums;
using FortuneWheel.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel.Scripts.UI.Visual
{
    public sealed class RewardVisualData
    {
        public string itemId;
        public string itemName;
        public Sprite icon;
        public int quantity;
        public ItemRaritySO rarity;
    }

    public class RewardVisualController : BaseVisualController<RewardVisualData>
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TextMeshProUGUI itemNameText;
        
        private Tween _backgroundImageTween;

        protected override void OnInitialize(RewardVisualData data)
        {
            iconImage.sprite = data.icon;
            itemNameText.text = data.itemName;
            quantityText.text = data.quantity.ToString();
            backgroundImage.color = data.rarity.RarityColor;
            _backgroundImageTween = backgroundImage.transform
                .DORotate(Vector3.forward * -360, 7f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);;
        }

        protected override void OnClear()
        {
            _backgroundImageTween.Kill();
            backgroundImage.transform.localRotation = Quaternion.Euler(0, 0, 0);
            gameObject.SetActive(false);
        }
    }
}
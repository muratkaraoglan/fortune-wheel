using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Cash", fileName = "Cash SO")]
    public class CashSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Cash;
    }
}
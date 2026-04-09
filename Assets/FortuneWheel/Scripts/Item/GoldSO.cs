using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Gold", fileName = "Gold SO ")]
    public class GoldSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Gold;
    }
}
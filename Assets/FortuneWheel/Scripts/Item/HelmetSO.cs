using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Helmet", fileName = "Helmet SO ")]
    public class HelmetSO : EquippableItemBaseSO
    {
        public override ItemType Type => ItemType.Helmet;
        //Helmet stats
    }
}
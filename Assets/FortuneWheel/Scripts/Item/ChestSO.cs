using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Chest", fileName = "Chest SO ")]
    public class ChestSO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Chest;
        
        //Possible item list
    }
}
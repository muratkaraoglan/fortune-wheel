using FortuneWheel.Scripts.Item.Enums;
using UnityEngine;

namespace FortuneWheel.Scripts.Item
{
    [CreateAssetMenu(menuName = "Fortune Wheel/Item/New Currency", fileName = "Currency SO")]
    public class CurrencySO : ItemBaseSO
    {
        public override ItemType Type => ItemType.Currency;
        [field:SerializeField]public CurrencyType Currency{get;private set;}
    }
}
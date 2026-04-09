using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    [CreateAssetMenu( menuName = "Fortune Wheel/Wheel Item Data" )]
    public class WheelItemData : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public Sprite ItemSprite { get; private set; }
        [field: SerializeField, Range(1, 100)] public float DropChance { get; private set; }
        [field: SerializeField] public int DropCount { get; private set; }
    }
}
using FortuneWheel.Scripts.Item;
using UnityEngine;

namespace FortuneWheel.Scripts
{
    public abstract class SkinBaseSO : ItemBaseSO
    {
        public abstract ItemBaseSO TargetItem { get; }
    }
}
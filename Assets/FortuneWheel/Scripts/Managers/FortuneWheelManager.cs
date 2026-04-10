using System.Collections.Generic;
using FortuneWheel.Scripts.Wheel;
using UnityEngine;

namespace FortuneWheel.Scripts.Managers
{
    public class FortuneWheelManager : MonoBehaviour
    {
        [SerializeField] private WheelItemConfigSO bronzeWheelItemConfigSo;
        [SerializeField] private List<WheelSliceVisualController> sliceVisualControllers;

        private const int SliceItemCount = 8;
        private readonly List<WheelSliceItemData> _cachedSliceItems = new(8);

        private void Start()
        {
            bronzeWheelItemConfigSo.PopulateSliceItems(_cachedSliceItems, SliceItemCount);
            for (int i = 0; i < SliceItemCount; i++)
            {
                sliceVisualControllers[i].Initialize(_cachedSliceItems[i]);
            }
        }
    }
}
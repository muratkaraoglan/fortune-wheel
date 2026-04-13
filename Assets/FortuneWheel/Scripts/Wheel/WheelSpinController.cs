using DG.Tweening;
using UnityEngine;

namespace FortuneWheel.Scripts.Wheel
{
    public class WheelSpinController
    {
        private const float AnglePerSlice = 45f;
        private const float FullRotationDegrees = 360f;
 
        private readonly Transform _wheelTransform;
        private readonly WheelSettingsConfigSO _settings;
 
        public WheelSpinController(Transform wheelTransform, WheelSettingsConfigSO settings)
        {
            _wheelTransform = wheelTransform;
            _settings = settings;
        }
 
        public void Spin(int selectedSliceIndex, TweenCallback onComplete)
        {
            var rotations = Random.Range(_settings.MinRotation, _settings.MaxRotation);
            var targetRotation = CalculateTargetRotation(selectedSliceIndex, rotations);
 
            _wheelTransform
                .DORotate(new Vector3(0, 0, targetRotation), _settings.SpinDuration, RotateMode.FastBeyond360)
                .SetEase(_settings.SpinCurve)
                .OnComplete(onComplete);
        }
 
        public void ResetRotation()
        {
            _wheelTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
 
        private float CalculateTargetRotation(int sliceIndex, int rotations)
        {
            var baseAngle = sliceIndex * AnglePerSlice;
 
            return _settings.ClockwiseRotation
                ? -(FullRotationDegrees * rotations + (FullRotationDegrees - baseAngle))
                : FullRotationDegrees * rotations + baseAngle;
        }
    }
}
namespace FortuneWheel.Scripts.Stat
{
    public enum StackingLogic
    {
        Override,       // The new effect overwrites the old one and resets the duration
        Stack,          // Two effects run simultaneously (Two needles = Double the speed)
        RefreshDuration // The previous effect remains, but its duration resets to the beginning
    }
}
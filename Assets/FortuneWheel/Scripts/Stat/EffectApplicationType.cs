namespace FortuneWheel.Scripts.Stat
{
    public enum EffectApplicationType
    {
        Instant,        // Apply immediately and finish (e.g., Health Pack)
        OverTime,       // Over a specific period of time, in “tick” increments (e.g., 5 HP per second)
        Duration,       // Remains active for the duration; the effect is removed once the duration ends (e.g., +20% speed for 10 seconds)
        Persistent      // Until the player dies or the round ends (e.g., permanent armor boost)
    }

}
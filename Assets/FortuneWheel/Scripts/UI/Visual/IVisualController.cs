namespace FortuneWheel.Scripts.UI.Visual
{
    public interface IVisualController<T>
    {
        void Initialize(T data);
        void Clear();
    }
}
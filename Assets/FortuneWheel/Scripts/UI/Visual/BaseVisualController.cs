using UnityEngine;

namespace FortuneWheel.Scripts.UI.Visual
{
    public abstract class BaseVisualController<T> : MonoBehaviour , IVisualController<T>
    {
        public void Initialize(T data)
        {
            OnInitialize(data);
        }

        public void Clear()
        {
            OnClear();
        }
        
        protected abstract void OnInitialize(T data);

        protected abstract void OnClear();
    }
}
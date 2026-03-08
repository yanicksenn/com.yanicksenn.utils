using System;

namespace YanickSenn.Utils.Animations {
    public abstract class Animation<TConfig> where TConfig : IAnimationConfig {

        public Action OnComplete;

        public abstract bool IsPlaying { get; }
        public abstract void Play(TConfig config);
        public abstract void Stop();

        protected void Complete(bool aborted) {
            OnComplete?.Invoke();
        }
    }
    public interface IAnimationConfig {

    }
}

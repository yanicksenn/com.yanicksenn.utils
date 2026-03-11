using System;
using UnityEngine;

namespace YanickSenn.Utils.Animations {
    public enum AnimationCompletionState {
        COMPLETED,
        ABORTED
    }

    public abstract class Animation<TConfig> where TConfig : IAnimationConfig {

        public Action OnComplete;
        private AwaitableCompletionSource<AnimationCompletionState> _tcs;

        public abstract bool IsPlaying { get; }

        public Awaitable<AnimationCompletionState> Play(TConfig config) {
            if (IsPlaying) {
                return _tcs?.Awaitable;
            }

            _tcs = new AwaitableCompletionSource<AnimationCompletionState>();
            DoPlay(config);
            return _tcs.Awaitable;
        }

        protected abstract void DoPlay(TConfig config);

        public abstract void Stop();

        protected void Complete(AnimationCompletionState state) {
            if (_tcs != null) {
                var tcs = _tcs;
                _tcs = null;
                tcs.SetResult(state);
            }
            OnComplete?.Invoke();
        }
    }
    public interface IAnimationConfig {

    }
}

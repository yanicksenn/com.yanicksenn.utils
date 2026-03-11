using System;
using PrimeTween;

namespace YanickSenn.Utils.Animations {
    /// <summary>
    /// Base class for animations that use <see cref="PrimeTween"/>.
    /// </summary>
    public abstract class TweenAnimation<TConfig> : Animation<TConfig> where TConfig : ITweenConfig {
        private IState _currentState = new IdlingState();

        public override bool IsPlaying => _currentState is PlayingState;

        protected override void DoPlay(TConfig config) {
            if (_currentState is PlayingState) {
                return;
            }

            _currentState = new PlayingState(CreateTween(config).OnComplete(() => {
                _currentState = new IdlingState();
                Complete(AnimationCompletionState.COMPLETED);
            }));
        }

        public override void Stop() {
            if (_currentState is not PlayingState playingState) {
                return;
            }

            if (playingState.CurrentTween.isAlive) {
                playingState.CurrentTween.Stop();
            }

            _currentState = new IdlingState();
            Complete(AnimationCompletionState.ABORTED);
        }

        /// <summary>
        /// Creates the tween to be played.
        /// </summary>
        /// <param name="config">The config for this animation.</param>
        /// <returns>The created tween.</returns>
        protected abstract Tween CreateTween(TConfig config);

        private interface IState { }

        private class PlayingState : IState {
            public readonly Tween CurrentTween;

            public PlayingState(Tween currentTween) {
                CurrentTween = currentTween;
            }
        }

        private class IdlingState : IState { }
    }

    public interface ITweenConfig : IAnimationConfig {}
}

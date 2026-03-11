using System;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using YanickSenn.Utils.Variables;

namespace YanickSenn.Utils.Animations {
    public class SplineAnimation : Animation<ISplineConfig> {
        private readonly Transform _transform;
        private readonly Spline _spline;

        private IState _currentState;

        public override bool IsPlaying => _currentState is PlayingState;

        public SplineAnimation(Transform transform, IValue<Spline> spline) : this(transform, spline.Value) { }

        public SplineAnimation(Transform transform, Spline spline) {
            _transform = transform;
            _spline = spline;
            _currentState = new IdlingState();
        }

        protected override void DoPlay(ISplineConfig config) {
            if (_currentState is PlayingState) {
                return;
            }

            float duration = CalculateDuration(config);
            _currentState = new PlayingState(
                _transform.position,
                _transform.rotation,
                Tween.Custom(
                        this,
                        0f,
                        1f,
                        duration,
                        (target, t) => target.UpdateTransform(t))
                        .OnComplete(() => {
                        _currentState = new IdlingState();
                        Complete(AnimationCompletionState.COMPLETED);
                        }));        }

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

        private void UpdateTransform(float t) {
            if (_currentState is not PlayingState playingState) {
                return;
            }

            float3 localPos = _spline.EvaluatePosition(t);
            float3 tangent = _spline.EvaluateTangent(t);

            Vector3 localPosV3 = new Vector3(localPos.x, localPos.y, localPos.z);
            Vector3 worldPos = playingState.StartPos + playingState.StartRot * localPosV3;
            _transform.position = worldPos;

            Vector3 tangentV3 = new Vector3(tangent.x, tangent.y, tangent.z);
            if (!(tangentV3.sqrMagnitude > 0.001f)) {
                return;
            }

            Quaternion worldRot = playingState.StartRot * Quaternion.LookRotation(tangentV3.normalized, Vector3.up);
            _transform.rotation = worldRot;
        }

        private float CalculateDuration(ISplineConfig config) {
            return config switch {
                WithDuration duration => duration.Duration,
                WithSpeed speed => _spline.CalculateLength(_transform.localToWorldMatrix) / speed.Speed,
                _ => throw new ArgumentException($"Unknown animation config: {config}")
            };
        }

        private interface IState { }

        private class PlayingState : IState {
            public readonly Vector3 StartPos;
            public readonly Quaternion StartRot;
            public readonly Tween CurrentTween;

            public PlayingState(Vector3 startPos, Quaternion startRot, Tween currentTween) {
                StartPos = startPos;
                StartRot = startRot;
                CurrentTween = currentTween;
            }
        }

        private class IdlingState : IState { }
    }

    public interface ISplineConfig : IAnimationConfig {}

    public class WithDuration : ISplineConfig {
        public readonly float Duration;

        public WithDuration(float duration) {
            Duration = duration;
        }
    }

    public class WithSpeed : ISplineConfig {
        public readonly float Speed;

        public WithSpeed(float speed) {
            Speed = speed;
        }
    }
}

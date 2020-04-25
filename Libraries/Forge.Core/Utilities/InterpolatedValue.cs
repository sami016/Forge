using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Utilities
{
    public struct InterpolatedValue
    {
        public float _currentValue;
        public float _targetValue;
        public float _startValue;
        private float? _targetTimeSeconds;
        private float? _transitionStartTimeSeconds;
        private bool _active;
        private readonly Mode _mode;

        public float Rate { get; set; }
        public float Epsilon { get; set; }
        public TimeSpan TransitionDuration { get; set; }

        public enum Mode
        {
            Cosine,
            CosineFast,
        }

        public InterpolatedValue(Mode mode, float initialValue = 0f, float rate = 1f)
        {
            _active = false;
            _mode = mode;
            _startValue = initialValue;
            _currentValue = initialValue;
            _targetValue = initialValue;
            Rate = rate;
            Epsilon = 0.05f;
            _targetTimeSeconds = null;
            _transitionStartTimeSeconds = null;
            TransitionDuration = TimeSpan.FromMilliseconds(500);
        }

        public void Tick(TickContext tickContext)
        {
            if (_active)
            {
                var relativeProgress = (tickContext.TimeSeconds - _transitionStartTimeSeconds.Value) / (_targetTimeSeconds.Value - _transitionStartTimeSeconds.Value);
                switch (_mode)
                {
                    case Mode.Cosine:
                        {
                            var weight = Math.Cos(relativeProgress * Math.PI / 2);
                            _currentValue = (float)(
                                (weight * _startValue)
                                + ((1 - weight) * _targetValue)
                            );
                        }
                        break;
                    case Mode.CosineFast:
                        {
                            var weight = Math.Cos(Math.PI / 4 + relativeProgress * Math.PI / 4);
                            _currentValue = (float)(
                                (weight * _startValue)
                                + ((1 - weight) * _targetValue)
                            );
                        }
                        break;
                        //case Mode.Proportionate:
                        //    _currentValue += (_targetValue - _currentValue) * Rate * tickContext.DeltaTimeSeconds;
                        //    break;
                }
                if (_targetTimeSeconds <= tickContext.TimeSeconds)
                {
                    _currentValue = _targetValue;
                    _active = false;
                    _targetTimeSeconds = null;
                    _transitionStartTimeSeconds = null;
                }
            }
        }

        public float Value
        {
            get => _currentValue;
        }

        public float TargetValue
        {
            get => _targetValue;
        }

        public void SetTargetValie(float value, TickContext tickContext)
        {
            if (_targetValue != value)
            {
                _transitionStartTimeSeconds = (float)(tickContext.TimeSeconds);
                _targetTimeSeconds = (float)(tickContext.TimeSeconds + TransitionDuration.TotalSeconds);
                _startValue = _currentValue;
                _targetValue = value;
                _active = true;
            }
        }
    }
}

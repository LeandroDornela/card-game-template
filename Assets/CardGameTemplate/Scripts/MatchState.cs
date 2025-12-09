using UnityEngine;

namespace CardGameTemplate
{
    public class MatchState
    {
        private float _matchTimer;

        private float _timerUpdateSignalInterval = 1;
        private float _lastTimerVal = 0;

        public float MatchTimer => _matchTimer;

        public void TickTimer()
        {
            _matchTimer += Time.deltaTime;

            if(_matchTimer - _lastTimerVal >= _timerUpdateSignalInterval)
            {
                _lastTimerVal = _matchTimer;
                GameEvents.OnMatchTimerUpdate.Trigger(_matchTimer);
            }
        }
    }
}

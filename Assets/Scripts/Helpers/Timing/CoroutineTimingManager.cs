using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.Timing
{
    public class CoroutineTimingManager : MonoBehaviour, ITimingManager
    {
        private readonly IList<Coroutine> _activeCoroutines = new List<Coroutine>();
        
        public Coroutine SetInterval(float interval,int loops,Action onFinished)
        {
            var intervalCoroutine = StartCoroutine(LoopIntervalCoroutine(interval,loops,onFinished));
            _activeCoroutines.Add(intervalCoroutine);
            return intervalCoroutine;
        }

        public Coroutine SetInterval(float interval,Action onFinished)
        {
            var intervalCoroutine = StartCoroutine(IntervalCoroutine(interval, onFinished));
            _activeCoroutines.Add(intervalCoroutine);
            return intervalCoroutine;
        }

        public void Stop(Coroutine activeCoroutine)
        {
            StopCoroutine(activeCoroutine);
        }

        private void OnDestroy()
        {
            foreach (var activeCoroutine in _activeCoroutines)
            {
                StopCoroutine(activeCoroutine);
            }
            _activeCoroutines.Clear();
        }

        private IEnumerator IntervalCoroutine(float interval, Action onFinished)
        {
            yield return new WaitForSeconds(interval);
            onFinished?.Invoke();
        }

        private IEnumerator LoopIntervalCoroutine(float interval,int loopCount, Action onLoop)
        {
            var isLoopingAlways = loopCount == -1;
            var counter = 0;
            
            while (isLoopingAlways || counter < loopCount)
            {
                yield return IntervalCoroutine(interval, onLoop);
                counter++;
            }
        }
    }
}
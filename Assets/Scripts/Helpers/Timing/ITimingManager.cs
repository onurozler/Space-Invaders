using System;
using UnityEngine;

namespace Helpers.Timing
{
    public interface ITimingManager
    {
        Coroutine SetInterval(float interval, Action onFinished);
        Coroutine SetInterval(float interval, int loops, Action onFinished);
        void Stop(Coroutine activeCoroutine);
    }
}
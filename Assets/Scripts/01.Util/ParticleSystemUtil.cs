using System;
using UnityEngine;

namespace Util
{
    public class ParticleSystemUtil : MonoBehaviour
    {
        public Action onParticleStopEvent;

        public void OnParticleSystemStopped()
        {
            onParticleStopEvent?.Invoke();
        }
    }
}
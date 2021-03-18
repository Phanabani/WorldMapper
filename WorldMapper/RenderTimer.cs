using System;

namespace WorldMapper
{
    /// <summary>
    /// A class that aids framerate-independent rendering. Use SampleTime
    /// once each frame to calculate the delta time in seconds from the last
    /// frame. You can then multiply transformations by this value to define
    /// movement per second rather than per frame.
    /// </summary>
    public class RenderTimer
    {
        public float DeltaTime { get; private set; }

        private long _lastTime;

        /// <summary>
        /// Calculate the time in seconds since the last sampling (0 if first
        /// sample).
        /// </summary>
        public void SampleTime()
        {
            var now = DateTime.UtcNow;
            var nowTicks = now.Ticks;
            if (_lastTime != 0)
                // Ticks equal 100ns AKA 10,000,000ths of a second
                DeltaTime = (nowTicks - _lastTime) / 10_000_000f;
            _lastTime = nowTicks;
        }
    }
}

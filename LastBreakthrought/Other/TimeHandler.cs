using UnityEngine;

namespace LastBreakthrought.Other
{
    public class TimeHandler
    {
        public void StopTime() => Time.timeScale = 0;
        public void ResetTime() => Time.timeScale = 1;
    }
}

using TMPro;
using UnityEngine;

namespace LastBreakthrought.UI.Timer
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dayCounterText;
        [SerializeField] private TextMeshProUGUI _clockText;

        public void UpdateDay(int daysPassed) =>
            _dayCounterText.text = $"{daysPassed.ToString()} Day";

        public void UpdateClock(int minutes, int seconds)
        {
            string minutesString = minutes < 10 ? $"0{minutes}" : minutes.ToString();
            string secondsString = seconds < 10 ? $"0{seconds}" : seconds.ToString();
            _clockText.text = $"{minutesString} : {secondsString}";
        }
    }
}

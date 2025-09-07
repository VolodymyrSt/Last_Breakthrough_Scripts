using LastBreakthrought.UI.ToolTip;
using TMPro;
using UnityEngine;

namespace LastBreakthrought.UI.Home
{
    public class HomeDistanceView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _distanceText;

        public void SetHomeDistance(int distance) =>
            _distanceText.text = distance.ToString() + "m";
    }
}

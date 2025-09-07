using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace LastBreakthrought.UI.ToolTip
{
    public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string _toolTipHeaderText;
        private string _toolTipContentText;

        private ToolTipHandler _toolTip;
        private ToolTipPosition _toolTipPosition;

        [Inject]
        private void Construct(ToolTipHandler toolTip) =>
            _toolTip = toolTip;

        public void ConfigureToolTip(string header, string content, ToolTipPosition toolTipPosition)
        {
            _toolTipHeaderText = header;
            _toolTipContentText = content;
            _toolTipPosition = toolTipPosition;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(1.1f, 0.2f).SetEase(Ease.Linear).Play();
            _toolTip.Activate(_toolTipHeaderText, _toolTipContentText, _toolTipPosition);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, 0.2f).SetEase(Ease.Linear).Play();
            _toolTip.Disactivate();
        }
    }
}

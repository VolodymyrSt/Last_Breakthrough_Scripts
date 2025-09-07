using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.Windows.OxygenSupplier
{
    public class OxygenSupplierWindowView : WindowView<OxygenSupplierWindowHandler>
    {
        [SerializeField] private Button _rechargeButton;
        [SerializeField] private RectTransform _proccesingImage;

        public Tween ProccesingAnimation { get; private set; }

        public override void Initialize()
        {
            DOTween.Init();

            ProccesingAnimation = _proccesingImage.DORotate(new Vector3(0f, 0f, 360f), 5f, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);

            _rechargeButton.onClick.AddListener(() =>
            {
                Handler.UseDevice();
                ProccesingAnimation.Restart();
            });
        }

        public override void Dispose() =>
            _rechargeButton.onClick.RemoveListener(() => Handler.UseDevice());
    }
}

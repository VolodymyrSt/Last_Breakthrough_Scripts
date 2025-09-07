using System;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.CraftingMachine.Crafts
{
    public class MechanismCraftView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform _mechanismContainer;
        [SerializeField] private RectTransform _detailsContainer;
        [SerializeField] private Button _craftButton;

        public void Init(Action craftAction) => 
            _craftButton.onClick.AddListener(() => craftAction?.Invoke());

        public RectTransform GetContainer() => _detailsContainer;
        public RectTransform GetMechanismContainer() => _mechanismContainer;
    }
}

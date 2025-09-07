using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Logic.Camera;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI
{
    public abstract class BasePanelView : MonoBehaviour 
    {
        protected IEventBus EventBus;
        protected IAudioService AudioService;
        protected FollowCamera FollowCamera;

        protected bool IsMenuOpen = false;
        protected bool IsTutorialEnded = false;

        [Inject]
        private void Construct(IEventBus eventBus, IAudioService audioService, FollowCamera followCamera)
        {
            EventBus = eventBus;
            AudioService = audioService;
            FollowCamera = followCamera;
        }

        public abstract void Init();
        public abstract void Close();
        public abstract void Open();

        public void OnNewItemAdded(Transform newItem) =>
            newItem.localScale = IsMenuOpen ? Vector3.one : Vector3.zero;

        protected void PerformOpenAndClose()
        {
            if (!IsTutorialEnded)
                return;
            else
            {
                if (IsMenuOpen)
                    Close();
                else
                    Open();
            }
        }
    }
}

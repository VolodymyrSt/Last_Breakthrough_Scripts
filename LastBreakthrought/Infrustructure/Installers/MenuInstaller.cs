using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Other;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure.Installers
{
    public class MenuInstaller : MonoInstaller 
    {
        [SerializeField] private SoundHolder _soundHolder;

        public override void InstallBindings()
        {
            BindSoundHolder();
            BindEventBus();
        }

        private void BindSoundHolder() =>
            Container.Bind<SoundHolder>().FromInstance(_soundHolder).AsSingle();
        
        private void BindEventBus() =>
            Container.Bind<IEventBus>().To<EventBus>().AsSingle();
    }
}


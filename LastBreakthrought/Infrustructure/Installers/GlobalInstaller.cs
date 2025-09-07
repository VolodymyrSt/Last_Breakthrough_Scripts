using LastBreakthrought.Configs.Dialogue;
using LastBreakthrought.Configs.Enemy;
using LastBreakthrought.Configs.Player;
using LastBreakthrought.Configs.Robot;
using LastBreakthrought.Configs.Sound;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using LastBreakthrought.Infrustructure.Services.Input;
using LastBreakthrought.Logic;
using LastBreakthrought.Util;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _curtainPrefab;
        [SerializeField] private CoroutineRunner _coroutineRunner;

        [Header("Configs")]
        [SerializeField] private PlayerConfigSO _playerConfigSO;
        [SerializeField] private EnemyConfigHolderSO _enemyConfigHolderSO;
        [SerializeField] private RobotConfigHolderSO _robotConfigHolderSO;
        [SerializeField] private DialogueConfigSO _dialogueConfigSO;
        [SerializeField] private SoundConfigSO _soundConfigSO;

        public override void InstallBindings()
        {
            BindLoadingCurtain();
            BindCoroutineRunner();

            BindInput();

            BindConfigs();
            BindConfigProviderService();

            BindSceneLoader();
            BindAudioService();

            BindGame();
        }

        private void BindGame() => 
            Container.Bind<Game>().AsSingle().NonLazy();

        private void BindConfigProviderService() =>
            Container.Bind<IConfigProviderService>().To<ConfigProviderService>().AsSingle();

        private void BindConfigs()
        {
            Container.Bind<PlayerConfigSO>().FromInstance(_playerConfigSO).AsSingle();
            Container.Bind<EnemyConfigHolderSO>().FromInstance(_enemyConfigHolderSO).AsSingle();
            Container.Bind<RobotConfigHolderSO>().FromInstance(_robotConfigHolderSO).AsSingle();
            Container.Bind<DialogueConfigSO>().FromInstance(_dialogueConfigSO).AsSingle();
            Container.Bind<SoundConfigSO>().FromInstance(_soundConfigSO).AsSingle();
        }

        private void BindSceneLoader() => 
            Container.Bind<SceneLoader>().AsSingle();

        private void BindCoroutineRunner()
        {
            var coroutineRunner = Container.InstantiatePrefabForComponent<CoroutineRunner>(_coroutineRunner);
            Container.Bind<ICoroutineRunner>().To<CoroutineRunner>().FromInstance(coroutineRunner);
        }

        private void BindLoadingCurtain()
        {
            var curtain = Container.InstantiatePrefabForComponent<LoadingCurtain>(_curtainPrefab);
            Container.Bind<LoadingCurtain>().FromInstance(curtain);
        }

        private void BindInput()
        {
            //if (SystemInfo.deviceType == DeviceType.Handheld)
            //    Container.Bind<IInputService>().To<MobileInput>().AsSingle();
            //else
            //    Container.Bind<IInputService>().To<ComputerInput>().AsSingle();

            Container.Bind<IInputService>().To<StandeloneInput>().AsSingle();
        }

        private void BindAudioService() =>
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
    }
}


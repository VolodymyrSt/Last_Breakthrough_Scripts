using LastBreakthrought.Configs.Dialogue;
using LastBreakthrought.Configs.Enemy;
using LastBreakthrought.Configs.Game;
using LastBreakthrought.Configs.Player;
using LastBreakthrought.Configs.Robot;
using LastBreakthrought.Infrustructure.Services.EventBus;

namespace LastBreakthrought.Infrustructure.Services.ConfigProvider
{
    public class ConfigProviderService : IConfigProviderService
    {
        public PlayerConfigSO PlayerConfigSO { get; private set; }
        public GameConfigSO GameConfigSO { get; set; }
        public EnemyConfigHolderSO EnemyConfigHolderSO { get; private set; }
        public RobotConfigHolderSO RobotConfigHolderSO { get; private set; }
        public DialogueConfigSO DialogueConfigSO { get; private set; }

        public ConfigProviderService(PlayerConfigSO playerConfigSO, EnemyConfigHolderSO enemyConfigHolderSO
            , RobotConfigHolderSO robotConfigHolderSO, DialogueConfigSO dialogueConfigSO)
        {
            PlayerConfigSO = playerConfigSO;
            EnemyConfigHolderSO = enemyConfigHolderSO;
            RobotConfigHolderSO = robotConfigHolderSO;
            DialogueConfigSO = dialogueConfigSO;
        }
    }
}

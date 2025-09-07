using LastBreakthrought.Configs.Dialogue;
using LastBreakthrought.Configs.Enemy;
using LastBreakthrought.Configs.Game;
using LastBreakthrought.Configs.Player;
using LastBreakthrought.Configs.Robot;

namespace LastBreakthrought.Infrustructure.Services.ConfigProvider
{
    public interface IConfigProviderService
    {
        PlayerConfigSO PlayerConfigSO { get; }
        GameConfigSO GameConfigSO { get; set; }
        EnemyConfigHolderSO EnemyConfigHolderSO { get; }
        RobotConfigHolderSO RobotConfigHolderSO { get; }
        DialogueConfigSO DialogueConfigSO { get; }
    }
}
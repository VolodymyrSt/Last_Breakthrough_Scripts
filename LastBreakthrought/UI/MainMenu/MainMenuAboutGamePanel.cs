using LastBreakthrought.Infrustructure.Services.EventBus.Signals;

namespace LastBreakthrought.Infrustructure.UI
{
    public class MainMenuAboutGamePanel : MainMenuPanel
    {
        public override void Init() => 
            EventBus.SubscribeEvent((OnMainMenuSettingPanelOpenedSignal signal) => Hide());

        public override void OnPanelOpened() => 
            EventBus.Invoke(new OnMainMenuAboutGamePanelOpenedSignal());
    }
}

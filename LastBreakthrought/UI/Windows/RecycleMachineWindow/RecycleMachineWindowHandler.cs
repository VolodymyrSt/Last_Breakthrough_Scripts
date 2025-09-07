namespace LastBreakthrought.UI.Windows.RecycleMachineWindow
{
    public class RecycleMachineWindowHandler : WindowHandler<RecycleMachineWindowView>
    {
        public override void ActivateWindow() => 
            View.ShowView();

        public override void DeactivateWindow() => 
            View.HideView();
    }
}


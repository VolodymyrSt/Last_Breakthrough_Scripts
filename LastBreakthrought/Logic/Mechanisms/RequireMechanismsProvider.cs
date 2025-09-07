namespace LastBreakthrought.Logic.Mechanisms
{
    public class RequireMechanismsProvider
    {
        public RequireMechanismHolderSO Holder { get; private set; }

        public RequireMechanismsProvider(RequireMechanismHolderSO requireMechanismHolderSO) =>
            Holder = requireMechanismHolderSO;
    }
}

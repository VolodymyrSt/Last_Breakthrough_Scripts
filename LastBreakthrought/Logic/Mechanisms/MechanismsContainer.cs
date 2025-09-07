using System.Collections.Generic;
using Zenject;

namespace LastBreakthrought.Logic.Mechanisms
{
    public class MechanismsContainer
    {
        public List<MechanismEntity> Mechanisms { get; set; }

        public MechanismsContainer() =>
            Mechanisms = new List<MechanismEntity>();

        public bool IsSearchedMechanismsAllFound(List<MechanismEntity> searchedMechanisms)
        {
            var searchedMechanismsCount = searchedMechanisms.Count;
            var currentSearchedMechanismsCount = 0;

            if (searchedMechanismsCount > Mechanisms.Count)
                return false;

            foreach (var searchedMechanism in searchedMechanisms)
            {
                foreach (var existedDetail in Mechanisms)
                {
                    if (searchedMechanism.Data.Id == existedDetail.Data.Id)
                    {
                        if (searchedMechanism.Quantity <= existedDetail.Quantity)
                            currentSearchedMechanismsCount++;
                        else
                            return false;
                    }
                    else
                        continue;
                }
            }

            if (currentSearchedMechanismsCount == searchedMechanismsCount)
                return true;
            else
                return false;
        }

        public void GiveMechanisms(List<MechanismEntity> requiredMechanisms)
        {
            foreach (var requiredMechanism in requiredMechanisms)
            {
                for (int i = 0; i < Mechanisms.Count; i++)
                {
                    var existedMechanism = Mechanisms[i];

                    if (existedMechanism != null)
                    {
                        if (requiredMechanism.Data.Id == existedMechanism.Data.Id)
                        {
                            existedMechanism.Quantity -= requiredMechanism.Quantity;

                            if (existedMechanism.Quantity <= 0)
                                Mechanisms.Remove(existedMechanism);
                        }
                        else
                            continue;
                    }
                }
            }
        }
    }
}

using System.Collections.Generic;

namespace LastBreakthrought.Logic.ShipDetail
{
    public class DetailsContainer
    {
        public List<ShipDetailEntity> Details { get;  set; }

        public DetailsContainer() => 
            Details = new List<ShipDetailEntity>();

        public bool IsSearchedDetailsAllFound(List<ShipDetailEntity> searchedDetails)
        {
            var searchedDetailsCount = searchedDetails.Count;
            var currentSearchedDetailsCount = 0;

            if (searchedDetailsCount > Details.Count)
                return false;

            foreach (var searchedDetail in searchedDetails)
            {
                foreach (var existedDetail in Details)
                {
                    if (searchedDetail.Data.Id == existedDetail.Data.Id)
                    {
                        if (searchedDetail.Quantity <= existedDetail.Quantity)
                            currentSearchedDetailsCount++;
                        else
                            return false;
                    }
                    else
                        continue;
                }
            }

            if (currentSearchedDetailsCount == searchedDetailsCount)
                return true;
            else
                return false;
        }

        public void GiveDetails(List<ShipDetailEntity> neededDetails)
        {
            foreach (var neededDetail in neededDetails)
            {
                for (int i = 0; i < Details.Count; i++)
                {
                    var existedDetail = Details[i];

                    if (existedDetail != null)
                    {
                        if (neededDetail.Data.Id == existedDetail.Data.Id)
                        {
                            existedDetail.Quantity -= neededDetail.Quantity;

                            if (existedDetail.Quantity <= 0)
                                Details.Remove(existedDetail);
                        }
                        else
                            continue;
                    }
                }
            }
        }
    }
}

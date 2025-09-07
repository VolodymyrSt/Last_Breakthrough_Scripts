using UnityEngine;

namespace LastBreakthrought.NPC.Robot
{
    public class RobotBattary
    {
        private const float INCREASE_CAPACITY_DELTA = 2f;
        private const float DECREASE_CAPACITY_DELTA = 1f;

        private float _capacityLimit;

        private float _maxCapacity;
        private float _currentCapacity;
        public float Capacity 
        { 
            get => _currentCapacity; 
            private set 
            { 
                _currentCapacity = value;
                
                if (_currentCapacity > _maxCapacity)
                    _currentCapacity = _maxCapacity;

                if (_currentCapacity < 0)
                    _currentCapacity = 0;
            } 
        }
        public bool NeedToBeRecharged { get; set; }

        public RobotBattary(float maxCapacity, float capacityLimit)
        {
            _maxCapacity = maxCapacity;
            _capacityLimit = capacityLimit;
            _currentCapacity = _maxCapacity;
            NeedToBeRecharged = false;
        }

        public void CheckIfCapacityIsRechedLimit()
        {
            if (Capacity <= _capacityLimit)
                NeedToBeRecharged = true;
        }
        public bool IsCapacityFull()
        {
            if (Capacity == _maxCapacity)
                return true;
            return false;
        }

        public void DecreaseCapacity() => 
            Capacity -= DECREASE_CAPACITY_DELTA * Time.deltaTime;

        public void IncreaseCapacity() => 
            Capacity += INCREASE_CAPACITY_DELTA * Time.deltaTime;
    }
}

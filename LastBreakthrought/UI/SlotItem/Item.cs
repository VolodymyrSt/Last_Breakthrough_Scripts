using UnityEngine;

namespace LastBreakthrought.UI.SlotItem
{
    public abstract class Item : MonoBehaviour, IItem
    {
        protected bool IsItemSelected = false;

        public abstract void Select();

        public abstract void UnSelect();
    }
}

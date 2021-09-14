using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniTech.Components.States
{
    public class CartState
    {
        public event Action OnChange;

        public void CartIconUpdate()
        {
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}

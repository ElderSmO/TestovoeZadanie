using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testovoe.Services
{
    public static class EventManager
    {
        public delegate void MaxProgressValue(int count);
        public static event MaxProgressValue maxProgresHandler;

        public delegate void UpdateProgressValue();
        public static event UpdateProgressValue updateProgressHandler;

        public delegate void StateOperation(bool state);
        public static event StateOperation stateOperationHandler;

        public static void OnGetMaxProgressValue(int count)
        {
            maxProgresHandler?.Invoke(count);
        }

        public static void OnUpdateProgress()
        {
            updateProgressHandler?.Invoke();
        }
        public static void OnGetStateOperation(bool state)
        {
            stateOperationHandler?.Invoke(state);
        }

    }
}

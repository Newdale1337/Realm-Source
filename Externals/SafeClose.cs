using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Externals
{
    public class SafeClose
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
        private delegate bool HandlerRoutine(CtrlTypes CtrlType);
        private HandlerRoutine ConsoleCtrlCheckRoutine { get; set; }

        private Action OnClose { get; set; }
        private ManualResetEvent WaitEvent { get; set; }

        public SafeClose(Action a, ManualResetEvent waitEvent)
        {
            ConsoleCtrlCheckRoutine = ConsoleCtrlCheck;
            SetConsoleCtrlHandler(ConsoleCtrlCheckRoutine, true);
            WaitEvent = waitEvent;
            OnClose = a;
        }

        private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    OnClose();
                    WaitEvent.WaitOne(1000);
                    break;
            }
            return true;
        }
    }

    public enum CtrlTypes
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }
}
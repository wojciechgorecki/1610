using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmallStacker.ViewModel.LogViewModel;

namespace SmallStacker.Utills
{
    public class LogMessage
    {
        private string msg;
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        private LogType logtype;
        public LogType Log
        {
            get { return logtype; }
            set { logtype = value; }
        }

        public LogMessage(string _msg,
                         LogType _log)
        {
            Log = _log;
            Msg = _msg;
        }
    }
}

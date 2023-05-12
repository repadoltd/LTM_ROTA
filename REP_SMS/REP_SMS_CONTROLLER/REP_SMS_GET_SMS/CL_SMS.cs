using System.ComponentModel;
using System.Data;
using System.Diagnostics;




namespace REP_SMS_GET_SMS
{
    public class CL_SMS
    {
        public string _message;
        public static CL_MSG _obj_cl_msg;

        protected static string _user;
        protected static string _password;
        protected static string _python_exe_path;
        protected static string _python_project_path_send;
        protected static string _python_project_path_get;

        public System.Timers.Timer _getSMSTimer = new System.Timers.Timer();

        
        

        public CL_SMS(CL_MSG obj_cl_msg, string user, string password, string python_exe_path, string python_project_path_send, string python_project_path_get)
        {
            _user = user;
            _password = password;
            _obj_cl_msg = obj_cl_msg;
            _python_exe_path = python_exe_path;
            _python_project_path_send = python_project_path_send;
            _python_project_path_get = python_project_path_get;



            SetupTimer();
        }

        public void SendSMS(string phonenumber, string messagetext)
        {
            ProcessStartInfo start = new();
            start.FileName = _python_exe_path;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            string function = "SendSMS";
            string argument = string.Format(_python_project_path_send, _user, _password, function, phonenumber, messagetext ); // @"path\to\application.py {0} {1} {2} {3} {4}"
            start.Arguments = argument;

            using Process process = Process.Start(start);
            //using (StreamReader reader = process.StandardOutput)
            //{
              //  string result = reader.ReadToEnd();
            //}
        }
        public void GetSMS()
        {
            ProcessStartInfo start = new();
            start.FileName = _python_exe_path;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            string function = "GetSMS";
            string argument = string.Format(_python_project_path_get, _user, _password, function); // @"path\to\application.py {0} {1} {2}"
            start.Arguments = argument;

            using Process process = Process.Start(start);
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();

                if (result != null)
                {
                    
                    _obj_cl_msg.PopulateDB(result);

                }
                
                
            }

            
        }
        private void SetupTimer()
        {
            _getSMSTimer = new System.Timers.Timer();
            _getSMSTimer.Interval = 1000;
            _getSMSTimer.Elapsed += new System.Timers.ElapsedEventHandler(getSMSTimerElapsed);
            _getSMSTimer.Enabled = true;
            
            _getSMSTimer.Start();
        }

        void getSMSTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _getSMSTimer.Stop();
            try
            {
                GetSMS();
            }
            catch { }
            _getSMSTimer.Start();
        }

        

        
    }

    public class CL_MSG
    {
        public CL_EVENT _obj_event;
        



        DataTable tab_message = new();
        public CL_MSG()
        {
            CreateDB();
            _obj_event = new CL_EVENT();
        }

        private void CreateDB()
        {
            tab_message.Columns.Add("Time", typeof(string));
            tab_message.Columns.Add("Content", typeof(string));
        }

        public string GetTime(string messages)
        {
            Console.WriteLine(messages);
            int from = messages.IndexOf(", 'Date': '");
            int to = messages.LastIndexOf("', 'Sca':");

            string time = messages.Substring(from, to - from).Replace(", 'Date': '", "").Trim();
            return time;
        }

        public string GetContent(string message)
        {
            int from = message.IndexOf(", 'Content': '");
            int to = message.LastIndexOf("', 'Date': '");

            string content = message.Substring(from, to - from).Replace(", 'Content': '", "").Trim();
            return content;
        }

        public void PopulateDB(string message)
        {
            string time = GetTime(message);
            string content = GetContent(message);

            DataRow[] row = tab_message.Select(String.Format("Time = '{0}'", time));
            
            
            tab_message.Rows.Add(time, content);
            ParseMessage(content);
            
            
        }

        public void ParseMessage(string content)
        {
            string events = content.Substring(0, 2);

            string parameter1 = content.Substring(2, 4);

            string parameter2 = content.Substring(6);

            switch (events)
            {
                case "00":
                    _obj_event.EventTaskStarted(parameter1, parameter2);
                    break;
                case "01":
                    _obj_event.EventTaskCompleted(parameter1, parameter2);
                    break;
                case "02":
                    _obj_event.EventRemainingTaskTime(parameter1, parameter2);
                    break;
                case "03":
                    _obj_event.EventRemainingSubstepTime(parameter1, parameter2);
                    break;
                case "04":
                    _obj_event.EventSetCurrentStep(parameter1, parameter2);
                    break;
                case "05":
                    _obj_event.EventDisplayIssue(parameter1, parameter2);
                    break;
                case "06":
                    _obj_event.EventDismissIssue(parameter1, parameter2);
                    break;
                    default: throw new Exception("False Format in Messagetext");
            }
        }
    }

    public class CL_EVENT
    {
        public event TaskStartedEventHandler TaskStarted;
        public delegate void TaskStartedEventHandler(string parameter1, string parameter2);

        public event TaskCompletedEventHandler TaskCompleted;
        public delegate void TaskCompletedEventHandler(string parameter1, string parameter2);

        public event RemainingTaskTimeEventHandler RemainingTaskTime;
        public delegate void RemainingTaskTimeEventHandler(string parameter1, string parameter2);

        public event RemainingSubstepTimeEventHandler RemainingSubstepTime;
        public delegate void RemainingSubstepTimeEventHandler(string parameter1, string parameter2);

        public event SetCurrentStepEventHandler SetCurrentStep;
        public delegate void SetCurrentStepEventHandler(string parameter1, string parameter2);

        public event DisplayIssueReceivedEventHandler DisplayIssueReceived;
        public delegate void DisplayIssueReceivedEventHandler(string parameter1, string parameter2);

        public event DismissIssueEventHandler DismissIssue;
        public delegate void DismissIssueEventHandler(string parameter1, string parameter2);



        
        
        
        
        


        public void EventTaskStarted(string parameter1, string parameter2)
        {
            
            if (TaskStarted != null)
            {
                TaskStarted.Invoke(parameter1, parameter2);
            }
            
        }

        public void EventTaskCompleted(string parameter1, string parameter2)
        {
            
            if (TaskCompleted != null)
            {
                TaskCompleted.Invoke(parameter1, parameter2);
            }
            
        }

        public void EventRemainingTaskTime(string parameter1, string parameter2)
        {
            
            if (RemainingTaskTime != null)
            {
                RemainingTaskTime.Invoke(parameter1, parameter2);
            }
            
        }

        public void EventRemainingSubstepTime(string parameter1, string parameter2)
        {
            
            if (RemainingSubstepTime != null)
            {
                RemainingSubstepTime.Invoke(parameter1, parameter2);
            }
            
        }

        public void EventSetCurrentStep(string parameter1, string parameter2)
        {
            
            if (SetCurrentStep != null)
            {
                SetCurrentStep.Invoke(parameter1, parameter2);
            }
            
        }

        public void EventDisplayIssue(string parameter1, string parameter2)
        {
            
            if (DisplayIssueReceived != null)
            {
                DisplayIssueReceived.Invoke(parameter1, parameter2);
            }
            
        }

        public void EventDismissIssue(string parameter1, string parameter2)
        {
            
            if (DismissIssue != null)
            {
                DismissIssue.Invoke(parameter1, parameter2);
            }
            
        }
    }
}
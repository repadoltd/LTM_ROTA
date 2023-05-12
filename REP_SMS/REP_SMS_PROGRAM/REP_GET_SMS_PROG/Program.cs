using REP_SMS_GET_SMS;


namespace REP_GET_SMS_PROG
{
    internal static class Program
    {
        public static CL_SMS _obj_sms;

        static private System.Configuration.Configuration _configuration;
        static private System.Configuration.KeyValueConfigurationCollection _appSettings = null;
        static private string _PythonExe = "";
        static private string _PythonPathSendSMS = "";
        static private string _PythonPathGetSMS = "";
        static private string _user = "";
        static private string _password = "";
        [STAThread]
        static void Main()
        {
            _configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            _appSettings = _configuration.AppSettings.Settings;

            CL_MSG event_control = new CL_MSG();


            

            event_control._obj_event.TaskStarted += TaskStarted;
            event_control._obj_event.TaskCompleted += TaskCompleted;
            event_control._obj_event.RemainingTaskTime += RemainingTaskTime;
            event_control._obj_event.RemainingSubstepTime += RemainingSubstepTime;
            event_control._obj_event.SetCurrentStep += SetCurrentStep;
            event_control._obj_event.DisplayIssueReceived += DisplayIssue;
            event_control._obj_event.DismissIssue += DismissIssue;

            ApplicationConfiguration.Initialize();

            try
            {
                string tmp = _appSettings["PathToPythonExe"].Value;
                if (tmp == "")
                    tmp = @"C:\Users\eblock\AppData\Local\Programs\Python\Python36\python.exe";
                                                
                _PythonExe = tmp;
            }
            catch
            {
                if (_appSettings["PathToPythonExe"] == null)
                    _appSettings.Add("PathToPythonExe", "C:\\Users\\eblock\\AppData\\Local\\Programs\\Python\\Python36\\python.exe");
                _PythonExe = @"C:\Users\eblock\AppData\Local\Programs\Python\Python36\python.exe";
            }

            try
            {
                string tmp2 = _appSettings["PathToPythonProgSendSMS"].Value;
                if (tmp2 == "")
                    tmp2 = @"C:\Python36\Test_3_6.py {0} {1} {2} {3} {4}";

                _PythonPathSendSMS = tmp2;
            }
            catch
            {
                if (_appSettings["PathToPythonProgSendSMS"] == null)
                    _appSettings.Add("PathToPythonProgSendSMS", "C:\\Python36\\Test_3_6.py {0} {1} {2} {3} {4}");
                _PythonPathSendSMS = @"C:\Python36\Test_3_6.py {0} {1} {2} {3} {4}";
            }

            try
            {
                string tmp3 = _appSettings["PathToPythonProgGetSMS"].Value;
                if (tmp3 == "")
                    tmp3 = @"C:\Python36\Test_3_6.py {0} {1} {2}";

                _PythonPathGetSMS = tmp3;
            }
            catch
            {
                if (_appSettings["PathToPythonProgGetSMS"] == null)
                    _appSettings.Add("PathToPythonProgGetSMS", "C:\\Python36\\Test_3_6.py {0} {1} {2}");
                _PythonPathGetSMS = @"C:\Python36\Test_3_6.py {0} {1} {2}";
            }

            try
            {
                string tmp4 = _appSettings["User"].Value;
                if (tmp4 == "")
                    tmp4 = "admin";

                _user = tmp4;
            }
            catch
            {
                if (_appSettings["User"] == null)
                    _appSettings.Add("User", "admin");
                _user = "admin";
            }

            try
            {
                string tmp5 = _appSettings["Password"].Value;
                if (tmp5 == "")
                    tmp5 = "repadofree!";

                _password = tmp5;
            }
            catch
            {
                if (_appSettings["Password"] == null)
                    _appSettings.Add("Password", "repadofree!");
                _password = "repadofree!";
            }

            _obj_sms = new(event_control, _user, _password, _PythonExe, _PythonPathSendSMS, _PythonPathGetSMS);

            
                
            
            


        }

        private static void DismissIssue(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }

        private static void DisplayIssue(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }

        private static void SetCurrentStep(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }

        private static void RemainingTaskTime(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }

        private static void RemainingSubstepTime(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }

        private static void TaskCompleted(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }

        private static void TaskStarted(string parameter1, string parameter2)
        {
            Application.Run(new Form1(parameter1, parameter2));
        }
    }
}
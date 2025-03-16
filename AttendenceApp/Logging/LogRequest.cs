namespace AttendenceApp.Logging
{
    public sealed class LoggingClass
    {
        private LoggingClass() { }
        public static LoggingClass Instance = null;
        public static LoggingClass GetInstance
        {
            get
            {
                if(Instance==null)
                {
                    Instance = new LoggingClass();
                }
                return Instance;
            }
        }
      public string fileName = "C:\\LEARN EPAM Exercise\\AttendenceApp-EPAM\\AttendenceApp\\Logging\\log.txt";
        public bool WriteLog(string message)
        {
         using(FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {

                byte[] data = System.Text.Encoding.UTF8.GetBytes(message+"\n");
                fs.Write(data, 0, data.Length);
                return true;
            }
        }
    }
}

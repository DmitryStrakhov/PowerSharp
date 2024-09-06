using System.IO;

namespace PowerSharp.Diagnostics {
    class Logger {
        readonly StreamWriter writer;

        private Logger() {
            writer = new StreamWriter(new FileStream(@"C:\Temp\logs\log.log", FileMode.Create));
        }
        public static readonly Logger Instance = new Logger();

        public void Write(string s) {
            writer.WriteLine(s);
            writer.Flush();
        }
    }
}

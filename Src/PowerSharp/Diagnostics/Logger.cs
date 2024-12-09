using System;
using System.IO;

namespace PowerSharp.Diagnostics {
    class Logger {
        readonly StreamWriter writer;

        private Logger() {
            writer = new StreamWriter(new FileStream(@"C:\Temp\powersharp.log", FileMode.Create));
        }
        public static readonly Logger Instance = new Logger();

        public void Write(string message) {
            writer.WriteLine(message);
            writer.Flush();
        }

        public void WriteValue<T>(string message, T value) {
            Write(message + ": " + (value?.ToString() ?? "null"));
        }
    }
}

using System;

namespace PluginTests {
    class MyClass {
        public int Property {
            {caret}get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

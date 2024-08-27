using System;

namespace PluginTests {
    class MyClass {
        public int Property {
            get { throw new {caret}NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

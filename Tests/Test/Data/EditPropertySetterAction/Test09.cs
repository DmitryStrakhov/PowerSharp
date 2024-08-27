using System;

namespace PluginTests {
    class MyClass {
        public int Property {
            get { throw new NotImplementedException(); }
            set{caret} { throw new NotImplementedException(); }
        }
    }
}

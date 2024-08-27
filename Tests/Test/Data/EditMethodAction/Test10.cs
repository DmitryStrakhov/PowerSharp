using System;

namespace PluginTests {
    class MyClass {
        public void Method(int x, int y, int z) {
            throw new Not{caret}ImplementedException();
        }
    }
}

using System;

namespace PluginTests {
    class MyClass {
        int x;
        int y;
        int z;

        public MyClass{caret}(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Method() {
            throw new NotImplementedException();
        }
    }
}

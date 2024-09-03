using System;

namespace PluginTests {
    class MyClass {
        int x;
        int y;
        int z;

        public MyClass() {
            
        }
        public int Property1 {
            g{caret}et { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public int Property2 { get; set; }
    }
}

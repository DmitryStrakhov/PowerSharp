using System;

namespace PluginTests {
    public interface I {
        void Method1();
        void Method2(int x, int y);
        string Method3(object value);

        int Property1 { get; }
        int Property2 { get; set; }
        int this[object value] { get; }
    }

    class MyClass {
        public void Method1() {
        }
        public string Method2(int x, int y, int z) {
            return ToString(x) + ToString(y) + ToString(z);

            string ToString(int x) {
                return x.ToString();
            }
        }

        public int Property1 {
            get { {caret}throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public int this[string value] {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

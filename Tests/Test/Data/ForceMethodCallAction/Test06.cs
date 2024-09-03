﻿using System;

namespace PluginTests {
    public interface I {
        void Method1();
        void Method2();
        int Property1 { get; set; }
        int Property2 { get; set; }
    }

    class MyClass {
        public void Method1() {
        }
        public void Method2() { }
        public void Method3() => throw new NotImplementedException();

        public int Property1 {
            get { throw new NotImplementedException(); }
            set { f{caret} }
        }
        public int Property2 { get; set; }
    }
}

﻿using System;

namespace PluginTests {
    public interface I {
        void {caret}Method1();
        void Method2();
        void Method3();

        int Property1 { get; set; }
        int Property2 { get; set; }
        int Property3 { get; set; }
        int Property4 { get; }
    }

    abstract class MyClass : I {
        public void Method1() {
            throw new NotImplementedException();
        }
        public void Method2() { }

        public void Method3() => throw new NotImplementedException();

        public int Property1 {
            get {

                throw new NotImplementedException();
                throw new NotImplementedException();
                throw new NotImplementedException();

            }
            set { throw new NotImplementedException(); }
        }
        public int Property2 { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        public int Property3 { get; set; }
        public int Property4 { get { throw new NotImplementedException(); } }

        void X() {
        }
        void Y() {
        }
        public abstract void Z();
    }
}

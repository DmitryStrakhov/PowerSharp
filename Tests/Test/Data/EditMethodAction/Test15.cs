﻿using System;

namespace PluginTests {
    class MyClass {
        int x;

        public MyClass(int x) {
            this.x = x;
        }

        public int X {
            get { return x;} }
            set {
                if(X == value) return;
                x = value;{caret}
                OnXChanged();
            }
        }
    }
}

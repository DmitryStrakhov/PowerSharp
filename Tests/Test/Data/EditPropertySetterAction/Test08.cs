﻿using System;

namespace PluginTests {
    class MyClass {
        public int Property {
            get { throw{caret} new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

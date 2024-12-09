﻿using System;

namespace PluginTests {
    readonly struct MyStruct {
        public Configuration(Set iSet, Set jSet, long fairCut) {
            ISet = iSet;
            JSet = jSet;
            FairCut = fairCut;
        }
        public readonly Set ISet;
        public readonly Set JSet;
        public readonly long {caret}FairCut;
    } 
}

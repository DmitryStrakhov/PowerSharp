using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Core;
using JetBrains.Diagnostics;
using JetBrains.Rider.Model.UIAutomation;

namespace PowerSharp.Extensions {
    public static class BeControlExtensions {
        [NotNull]
        [Pure]
        public static T GetBeControlByIdBfs<T>(this BeControl @this, [NotNull] string id, int? maxLevelOfDepth = null)
            where T : BeControl {
            int maxDepth = maxLevelOfDepth.GetValueOrDefault(int.MaxValue);
            int level = 0;

            Queue<BeControl> queue = new Queue<BeControl>();
            queue.Enqueue(@this);
            queue.Enqueue(null);

            while(queue.Count != 0) {
                BeControl control = queue.Dequeue();
                if(control == null) {
                    if(++level == maxDepth) {
                        throw new Assertion.AssertionException("Control is not found in the depth " + maxDepth.ToString());
                    }
                    queue.Enqueue(null);
                    continue;
                }

                Maybe<string> controlId = control.ControlId.Maybe;
                if(controlId.HasValue && controlId.Value == id) {
                    return (T)control;
                }
                foreach(BeControl c in ChildrenOf(control)) {
                    queue.Enqueue(c);
                }
            }
            throw new Assertion.AssertionException("Control is not found");
        }
        static IEnumerable<BeControl> ChildrenOf([NotNull] BeControl owner) {
            switch(owner) {
                case BeGrid grid:
                    if(grid.Items.Maybe.ValueOrDefault == null)
                        yield break;
                    else {
                        foreach(BeGridElement gridElement in grid.Items.Value) {
                            yield return gridElement.Content;
                        }
                        break;
                    }
                case BeGroupBox groupBox:
                    yield return groupBox.Content;
                    break;
            }
        }
    }
}
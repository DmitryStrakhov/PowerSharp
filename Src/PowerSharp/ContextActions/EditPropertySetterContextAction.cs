using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace PowerSharp.ContextActions {
    [ContextAction(Name = "Edit Property Setter", Description = "Start editing a property setter", Group = "C#")]
    public sealed class EditPropertySetterContextAction : EditPropertyContextActionBase {
        public EditPropertySetterContextAction([NotNull] ICSharpContextActionDataProvider dataProvider)
            : base(dataProvider, CSharpAccessorKind.SETTER) {
        }
        public override string Text { get { return "Edit property setter"; } }
    }
}

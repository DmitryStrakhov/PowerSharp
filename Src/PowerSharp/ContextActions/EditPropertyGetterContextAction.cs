using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace PowerSharp.ContextActions {
    [ContextAction(Name = "Edit Property Getter", Description = "Start editing a property getter", Group = "C#")]
    public sealed class EditPropertyGetterContextAction : EditPropertyContextActionBase {
        public EditPropertyGetterContextAction([NotNull] ICSharpContextActionDataProvider dataProvider)
            : base(dataProvider, CSharpAccessorKind.GETTER) {
        }
        public override string Text { get { return "Edit property getter"; } }
    }
}

using System;
using JetBrains.Annotations;
using PowerSharp.Utils;
using JetBrains.Diagnostics;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Actions {
    public abstract class EditPropertyActionBase : EditActionBase {
        readonly CSharpAccessorKind accessorKind;

        protected EditPropertyActionBase(CSharpAccessorKind accessorKind) {
            this.accessorKind = accessorKind;
        }

        protected sealed override bool IsAvailable(IDataContext context) {
            IPropertyDeclaration property = context.TryGetPropertyDeclaration();

            if(IntentionUtils.IsValid(property)) {
                IAccessorDeclaration acc = GetTargetAccessor(property);
                return acc != null && acc.HasCodeBody();
            }
            return false;
        }
        protected sealed override ICSharpFunctionDeclaration GetTargetFunction(IDataContext context) {
            IPropertyDeclaration property = context.TryGetPropertyDeclaration();
            property.NotNull("property != null");
            return GetTargetAccessor(property);
        }

        [CanBeNull]
        private IAccessorDeclaration GetTargetAccessor([NotNull] IPropertyDeclaration property) {
            foreach(IAccessorDeclaration accessor in property.AccessorDeclarationsEnumerable) {
                if(accessor.CSharpKind == accessorKind) return accessor;
            }
            return null;
        }
    }
}

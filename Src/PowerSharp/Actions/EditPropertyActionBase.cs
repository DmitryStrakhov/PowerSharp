using JetBrains.Annotations;
using PowerSharp.Utils;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace PowerSharp.Actions {
    public abstract class EditPropertyActionBase : EditFunctionActionBase {
        readonly CSharpAccessorKind accessorKind;

        protected EditPropertyActionBase(CSharpAccessorKind accessorKind) {
            this.accessorKind = accessorKind;
        }

        protected sealed override ICSharpFunctionDeclaration GetTargetFunction(IDataContext context) {
            IPropertyDeclaration property = context.TryGetPropertyDeclaration();
            if(!IntentionUtils.IsValid(property))
                return null;

            IAccessorDeclaration accessor = context.TryGetAccessorDeclaration();
            if(accessor != null)
                return AcceptAccessor(accessor) ? accessor : null;

            foreach(IAccessorDeclaration acc in property.AccessorDeclarationsEnumerable) {
                if(AcceptAccessor(acc)) return acc;
            }
            return null;
        }
        private bool AcceptAccessor([NotNull] IAccessorDeclaration accessor) {
            return accessor.CSharpKind == accessorKind && accessor.HasCodeBody();
        }
    }
}

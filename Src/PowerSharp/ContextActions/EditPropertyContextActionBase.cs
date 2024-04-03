using System;
using JetBrains.Annotations;
using JetBrains.Util;
using PowerSharp.Utils;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.CSharp.ContextActions;

namespace PowerSharp.ContextActions {
    public abstract class EditPropertyContextActionBase : EditContextActionBase {
        [NotNull] readonly ICSharpContextActionDataProvider dataProvider;
        readonly CSharpAccessorKind accessorKind;

        protected EditPropertyContextActionBase([NotNull] ICSharpContextActionDataProvider dataProvider, CSharpAccessorKind accessorKind) {
            this.dataProvider = dataProvider;
            this.accessorKind = accessorKind;
        }

        public sealed override bool IsAvailable(IUserDataHolder cache) {
            IPropertyDeclaration property = dataProvider.GetPropertyDeclaration();

            if(IntentionUtils.IsValid(property)) {
                IAccessorDeclaration acc = GetTargetAccessor(property);
                return acc != null && acc.HasCodeBody();
            }
            return false;
        }
        protected sealed override ICSharpFunctionDeclaration GetTargetFunction() {
            return GetTargetAccessor(dataProvider.GetPropertyDeclaration());
        }

        [CanBeNull]
        private IAccessorDeclaration GetTargetAccessor(IPropertyDeclaration property) {
            foreach(IAccessorDeclaration accessor in property.AccessorDeclarationsEnumerable) {
                if(accessor.CSharpKind == accessorKind) return accessor;
            }
            return null;
        }
    }
}

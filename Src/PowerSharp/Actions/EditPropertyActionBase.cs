using System;
using JetBrains.Annotations;
using PowerSharp.Utils;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace PowerSharp.Actions {
    public abstract class EditPropertyActionBase(CSharpAccessorKind accessorKind) : EditFunctionActionBase {
        readonly CSharpAccessorKind accessorKind = accessorKind;

        protected sealed override IEditFunctionTarget GetTargetFunction(IDataContext context) {
            IAccessorDeclaration accessor = FindAccessor(context);
            if(accessor == null) return null;
            return new PropertyAccessorTarget(accessor);
        }
        IAccessorDeclaration FindAccessor(IDataContext context) {
            IPropertyDeclaration property = null;
            IAccessorDeclaration accessor = null;

            foreach(TreeElement element in context.EnumerateSelectedTreeElementAndAncestors()) {
                switch(element) {
                    case IPropertyDeclaration p:
                        property = p;
                        break;
                    case IAccessorDeclaration a:
                        accessor = a;
                        break;
                }
                if(property != null && accessor != null)
                    break;
            }

            if(!IntentionUtils.IsValid(property))
                return null;

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

        class PropertyAccessorTarget(IAccessorDeclaration accessor) : IEditFunctionTarget {
            readonly IAccessorDeclaration accessor = accessor;

            public IBlock Body => accessor.Body;
            public ITreeNode Node => accessor;
            public IArrowExpressionClause ArrowClause => accessor.ArrowClause;
        }
    }
}

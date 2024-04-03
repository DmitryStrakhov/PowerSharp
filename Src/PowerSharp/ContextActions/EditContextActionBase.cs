using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Feature.Services.ContextActions;

namespace PowerSharp.ContextActions {
    public abstract class EditContextActionBase : ContextActionBase {
        protected EditContextActionBase() {
        }

        protected sealed override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress) {
            return editor => {
                ICSharpFunctionDeclaration function = GetTargetFunction();
                if(function != null) {
                    DocumentRange range = CalculateSelectionRange(function);
                    
                    if(range.IsValid())
                        editor.Selection.SetRange(range);
                }
            };
        }
        [CanBeNull]
        protected abstract ICSharpFunctionDeclaration GetTargetFunction();

        private DocumentRange CalculateSelectionRange(ICSharpFunctionDeclaration function) {
            if(function.Body != null) {
                return new DocumentRange(
                    function.Body.LBrace.GetNextNonWhitespaceSibling().GetDocumentStartOffset(),
                    function.Body.RBrace.GetPreviousNonWhitespaceSibling().GetDocumentEndOffset()
                );
            }

            if(function.ArrowClause != null && function.LastChild != null) {
                return new DocumentRange(
                    function.ArrowClause.Expression.GetDocumentStartOffset(),
                    function.LastChild.GetDocumentEndOffset()
                );
            }
            return DocumentRange.InvalidRange;
        }
    }
}

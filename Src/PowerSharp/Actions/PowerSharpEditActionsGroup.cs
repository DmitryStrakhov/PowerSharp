using JetBrains.ReSharper.Feature.Services.Menu;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;

namespace PowerSharp.Actions {
    [ActionGroup(ActionGroupInsertStyles.Separated)]
    public class PowerSharpEditActionsGroup : IAction, IInsertLast<EditTextualGroup> {
        public PowerSharpEditActionsGroup(
            EditMethodAction editMethodAction,
            EditPropertyGetterAction editPropertyGetterAction,
            EditPropertySetterAction editPropertySetterAction,
            EditReturnValueAction editReturnValueAction,
            EditParametersAction editParametersAction,
            NewMemberLineAction newMemberLineAction,
            ForceMethodCallAction forceMethodCallAction
        ) {
        }
    }
}

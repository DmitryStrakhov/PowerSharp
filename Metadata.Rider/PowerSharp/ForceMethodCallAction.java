package PowerSharp;

import com.intellij.openapi.actionSystem.AnActionEvent;
import com.jetbrains.rider.actions.base.RiderAnAction;
import com.intellij.ide.IdeEventQueue;
import org.jetbrains.annotations.NotNull;

public class ForceMethodCallAction extends RiderAnAction {
    public ForceMethodCallAction() {
        super("ForceMethodCall", "Force Method Call", null, null, null);
    }
    @Override
    public void actionPerformed(@NotNull AnActionEvent e) {
        super.actionPerformed(e);
        IdeEventQueue.getInstance().getPopupManager().closeAllPopups();
    }
}

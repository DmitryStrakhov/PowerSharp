using System;
using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;
using JetBrains.IDE.UI.Extensions;
using JetBrains.Rider.Model.UIAutomation;

namespace PowerSharp.Builders {
    /// <summary>
    /// 
    /// Helps building UIs. Mostly used in refactorings.
    /// API is designed in a fluent style
    /// 
    /// </summary>
    public class PageBuilder {
        readonly Lifetime lifetime;
        [NotNull] readonly BeGrid content;
        [CanBeNull] PageBuilder owner;

        public PageBuilder(Lifetime lifetime) {
            this.lifetime = lifetime;
            this.content = BeControls.GetGrid();
        }
        private PageBuilder(Lifetime lifetime, PageBuilder owner)
            : this(lifetime) {
            this.owner = owner;
        }
        [Pure]
        [NotNull]
        public PageBuilder TextBox([NotNull] IProperty<string> property, [NotNull] string description, [CanBeNull] Action<BeTextBox> initializer = null) {
            Guard.IsNotNull(property, nameof(property));
            Guard.IsNotEmpty(description, nameof(description));

            BeTextBox textBox = property.GetBeTextBox(lifetime);
            initializer?.Invoke(textBox);
            content.AddElement(textBox.WithDescription(description, lifetime));
            return this;
        }
        [Pure]
        [NotNull]
        public PageBuilder CheckBox([NotNull] IProperty<bool> property, [NotNull] string description, bool? isEnabled = null, [CanBeNull] Action<BeCheckbox> initializer = null) {
            Guard.IsNotNull(property, nameof(property));
            Guard.IsNotEmpty(description, nameof(description));

            BeCheckbox checkBox = property.GetBeCheckBox(lifetime, description, isEnabled.GetValueOrDefault(true));
            initializer?.Invoke(checkBox);
            content.AddElement(checkBox);
            return this;
        }
        [Pure]
        [NotNull]
        public PageBuilder StartGroupBox() {
            return new PageBuilder(lifetime, this);
        }
        [NotNull]
        public PageBuilder EndGroupBox([NotNull] string title) {
            Guard.IsNotEmpty(title, nameof(title));

            if(owner == null)
                throw new InvalidOperationException(nameof(owner));
            owner.content.AddElement(content.InGroupBox(title));
            return owner;
        }
        [Pure]
        [NotNull]
        public BeGrid Content() {
            return content;
        }
    }
}
using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public class RedirectNodeView : BaseNodeView<RedirectNode>
    {
        protected override void OnModelChanged() =>
            SetPosition(Model.Position);
    }
}
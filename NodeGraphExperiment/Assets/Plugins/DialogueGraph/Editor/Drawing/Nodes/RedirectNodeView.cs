using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class RedirectNodeView : BaseNodeView<RedirectNode>
    {
        protected override void OnModelChanged() =>
            SetPosition(Model.Position);
    }
}
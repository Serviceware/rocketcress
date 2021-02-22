using System.Collections.Generic;

namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestMenuControl : IUITestControl
    {
        IEnumerable<IUITestControl> Items { get; }
    }
}

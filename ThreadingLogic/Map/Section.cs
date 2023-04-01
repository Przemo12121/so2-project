using System.Drawing;

namespace ThreadingLogic.Map;

public record Section(int Index)
{
    public virtual IOccupant? Occupant { get; set; } = default;
}
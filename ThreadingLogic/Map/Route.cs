using System.Collections.Generic;
using System.Linq;

namespace ThreadingLogic.Map;

public class Route
{
    private IReadOnlyList<Section> _map;
    
    public Route(int length)
    {
        _map = new List<Section>(
            Enumerable.Range(0, length)
                .Select(index => new Section(index))
        );
    }

    public Section Next(Section section)
        => _map[(section.Index +1) % _map.Count];

    public Section Enter()
        => _map[0];
};
﻿namespace ThreadingLogic.Map;

public class Route : IRoute
{
    public IReadOnlyList<Section> Map { get; private init; }
    
    public Route(int length)
    {
        Map = new List<Section>(
            Enumerable.Range(0, length)
                .Select(index => new Section(index))
        );
    }

    public Section Next(Section section)
        => Map[(section.Index + 1) % Map.Count];

    public Section Enter()
        => Map[0];

    public bool IsFinished(Section position)
        => position.Index.Equals(Map.Count - 1);
};
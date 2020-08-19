using System.Collections;
using System.Collections.Generic;
using AntDiary;
using UnityEngine;

public abstract class NestPathEdge : IPathEdge
{
    public abstract NestPathNode A { get; }
    IPathNode IPathEdge.B => B;

    public float Cost => (A.WorldPosition - B.WorldPosition).magnitude;

    IPathNode IPathEdge.A => A;

    public abstract NestPathNode B { get; }

    protected void RegisterToNode()
    {
        A.RegisterEdge(this);
        B.RegisterEdge(this);
    }

    protected void UnregisterFromNode()
    {
        A.UnregisterEdge(this);
        B.UnregisterEdge(this);
    }
    


}

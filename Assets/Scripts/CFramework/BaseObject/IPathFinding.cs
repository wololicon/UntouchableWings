using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public interface IPathFinding
{
    void GetPath();
    void StartMove(Path P_path);
}

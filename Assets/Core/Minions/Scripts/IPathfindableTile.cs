using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfindableTile
{


    public void CalculateFCost();


    public void SetIsWalkable(bool isWalkable);




}

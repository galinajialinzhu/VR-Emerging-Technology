using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

public class PuzzlePieceTag : MonoBehaviour, IGameObjectFilter
{
    public enum PieceType
    {
        Drums,Bass,Melody,Rhythm
    }

    public PieceType piece;

    public bool Filter(GameObject gameObject)
    {
        bool hasPuzzlePieceTag = gameObject.GetComponent<PuzzlePieceTag>()!=null;
        bool isSame = false;
        if (hasPuzzlePieceTag)
        {
            bool hasSamePieceAsThis=gameObject.GetComponent<PuzzlePieceTag>().piece == piece;

            isSame = hasSamePieceAsThis;
        }
        
        return isSame;
     
    }
    
}

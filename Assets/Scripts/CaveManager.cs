using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CaveManager : MonoBehaviour
{
    [SerializeField] private List<Transform> allCover;

    [SerializeField] private List<VisionCheck> notBrokenCover;

    private string name = "HidingSpotVision";

    public VisionCheck[] GetAllAvalibleHidingSpots()
    {
        cleanupCover();
        notBrokenCover.Clear();
        foreach (Transform cover in allCover)
        {
            foreach (Transform child in cover.transform)
            {
                if (child.name.Contains(name))
                {
                    notBrokenCover.Add(child.GetComponent<VisionCheck>());
                }
            }
        }

        return notBrokenCover.ToArray();
    }

    private void cleanupCover()
    {
        for(int i = 0; i < allCover.Count; i++)
        {
            if (allCover[i] == null) { 
                allCover.RemoveAt(i);
            }
        }
    }
}

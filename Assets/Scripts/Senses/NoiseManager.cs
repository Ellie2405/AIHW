using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages noise calls and triggers the relevant listeners
public class NoiseManager : MonoBehaviour
{
    static public NoiseManager Instance;
    List<Hearing> allHearing = new List<Hearing>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddHearer(Hearing hearing)
    {
        allHearing.Add(hearing);
    }

    //called by sources of noise to alert relevant listeners
    public void EmitNoise(Aspect.Affiliation sourceAffiliation, Vector3 position, float noiseLevel)
    {
        foreach (var hearing in allHearing)
        {
            if (sourceAffiliation == hearing.targetAffiliation)
            {
                if (Vector3.Distance(hearing.transform.position, position) < noiseLevel * hearing.HearingPower)
                {
                    hearing.Hear(position);
                }
            }
        }
    }
}

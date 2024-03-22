using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeilController : MonoBehaviour
{
    public CharacterAdventureController characterAdventureController;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material normalMaterial;
    public Material deadMaterial;
    private bool Deaddone;
    private bool NormalDone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(characterAdventureController.doll.status == Status.Dead && !Deaddone)
        {
            Material[] mats = new Material[1];
            mats[0] = deadMaterial;
            skinnedMeshRenderer.materials = mats;
            Deaddone = true;
            NormalDone = false;
        }
        else if(characterAdventureController.doll.status == Status.Normal && !NormalDone)
        {
            Material[] mats = new Material[1];
            mats[0] = normalMaterial;
            skinnedMeshRenderer.materials = mats;
            NormalDone = true;
            Deaddone = false;
        }
    }
}

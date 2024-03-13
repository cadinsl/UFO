using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    private enum CURRENT_TERRAIN { GRASS = 0, GRAVEL = 1, WOOD_FLOOR = 2, WATER = 3};

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    [SerializeField]
    private AK.Wwise.Switch[] terrainSwitch;

    [SerializeField]
    private AK.Wwise.Event footstepsEvent;

    public void PlayFootsteps()
    {
        terrainSwitch[(int)currentTerrain].SetValue(this.gameObject);
        AkSoundEngine.PostEvent(footstepsEvent.Id, this.gameObject);    
    }

    private void CheckTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 1.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Gravel"))
            {
                currentTerrain = CURRENT_TERRAIN.GRAVEL;
                break;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD_FLOOR;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentTerrain = CURRENT_TERRAIN.GRASS;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                currentTerrain = CURRENT_TERRAIN.WATER;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckTerrain();
    }
}

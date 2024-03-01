using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPointer : MonoBehaviour
{
    [SerializeField]
    private GameObject pointer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Equals(EventSystem.current.currentSelectedGameObject, this.gameObject))
        {
            pointer.SetActive(true);
        }
        else
        {
            pointer.SetActive(false);
        }
    }
}

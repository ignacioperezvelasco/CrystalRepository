using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivate : MonoBehaviour
{
    #region VARIABLES
    public enum Type
    {
        NONE,
        ACTIVATE,
        DEACTIVATE
    };

    public Type type;

    public GameObject[] gameObjects;
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case Type.NONE:
                    {
                        Debug.Log("No has seleccinado tipo");
                        break;
                    }
                case Type.ACTIVATE:
                    {
                        foreach (GameObject go in gameObjects)
                        {
                            go.SetActive(true);
                        }
                        break;
                    }
                case Type.DEACTIVATE:
                    {
                        foreach (GameObject go in gameObjects)
                        {
                            go.SetActive(false);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
    #endregion
}

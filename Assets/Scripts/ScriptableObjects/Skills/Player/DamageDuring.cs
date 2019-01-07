using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDuring : MonoBehaviour, ISkill<GameObject> {

    public void Use(GameObject target)
    {
        if (target.tag == "Player")
        {
            
        }
    }
}

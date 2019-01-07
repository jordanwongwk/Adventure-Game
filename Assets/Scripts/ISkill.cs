using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill<T>
{
    void Use(T targetObject);
}

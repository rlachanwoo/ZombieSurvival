using UnityEngine;
using UnityEngine.Events;
public interface IItem
{
    public UnityEvent OnUse{get; set;}
    void Use(GameObject target);    
} 


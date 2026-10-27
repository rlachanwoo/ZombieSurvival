using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour, IItem
{
    private Gamemanager gamemanager;
    int score = 100;
    public UnityEvent OnUse { get; set; } = new UnityEvent();

    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameController");
        gamemanager = findGo.GetComponent<Gamemanager>();
    }

    public void Use(GameObject target)
    {
        gamemanager.AddScore(score);
        
        gameObject.SetActive(false);
        OnUse.Invoke();
    }
}
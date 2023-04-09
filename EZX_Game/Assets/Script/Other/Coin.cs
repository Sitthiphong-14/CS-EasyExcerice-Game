using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Interactable, Moveable, Damageable
{
    public float currentMovementSpeed = 2f;
    public float destroyTime = 5f;
    public float point = 1;
    private GameObject coin;
    public float speed = 100f;

    private void Awake()
    {
        coin = this.gameObject;
        Destroy(gameObject, destroyTime);
    }
    private void Update()
    {
        Moveforward();
        transform.Rotate(0f, speed*Time.deltaTime, 0f);
    }
    public void Interact()
    {
        Debug.Log("interact");
    }

    public void Movebackward()
    {
        // movebackward
    }

    public void Moveforward()
    {
        coin.transform.position = coin.transform.position + new Vector3(0, 0, -1 * currentMovementSpeed * Time.deltaTime);
    }

    public void Moveleft()
    {
        //moveleft
    }

    public void Moveright()
    {
        //moveright
    }

    public float damage()
    {
        AudioManager.Instance.PlaySFX("Item");
        return point * -1;
    }
}

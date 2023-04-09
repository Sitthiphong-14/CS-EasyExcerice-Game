using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class PlayerRunController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject player;
    private Animator playerAnimator;
    public float point = 0;
    public float MaxSpeed = 4f;
    public float MinSpeed = -1f;
    private float currentMovementSpeed = 1f;
    private float currentTime, nextHitTime;
    public float delayHitTime = 3;
    public GameOverScreen GameOverScreen;
    public RunController RunController;
    private bool reset = true;
    private bool isSendRecord = true;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerAnimator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (PoseTransform.Instance.isLegUp() && reset)
        {
            IncreaseMoveSpeed();
            reset = false;
        }
        else
        {
            DecreaseMoveSpeed();
        }
        if (PoseTransform.Instance.isLegDown())
        {
            reset = true;
        }
        Move();
        currentTime += Time.deltaTime;
        if (currentHealth <= 0)
        {
            Time.timeScale = 0f;
            if(isSendRecord)
            {
                GameOver();
                isSendRecord = false;
            }
        }
    }
    private void IncreaseMoveSpeed()
    {
        if (currentMovementSpeed < MaxSpeed)
            currentMovementSpeed += 3f;
    }

    private void DecreaseMoveSpeed()
    {
        if (currentMovementSpeed > MinSpeed)
            currentMovementSpeed -= 0.05f;
    }

    private void Move()
    {
        player.transform.position = player.transform.position + new Vector3(1 * currentMovementSpeed * Time.deltaTime, 0, 0);
        playerAnimator.SetFloat("Blend", 0.6f);
    }

    public void OnCollisionEnter(Collision other)
    {
        try
        {
            if (other.gameObject.tag == "Wall")
            {
                TakeDamage(20);
                return;
            }
            Damageable damageObject = other.gameObject.GetComponent<Damageable>();
            point = point - damageObject.damage();
            if (other.gameObject.tag == "Enemy")
            {
                TakeDamage(20);
            }
            Destroy(other.gameObject);
            if (point <= 0) point = 0;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return;
        }
    }

    void TakeDamage(int damage)
    {
        if (currentTime >= nextHitTime)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            nextHitTime = currentTime + delayHitTime;
            AudioManager.Instance.PlaySFX("Damage");
        }
    }

    public void GameOver()
    {
        GameOverScreen.Setup(RunController.CalculateKcal(), (int)currentTime, "RUN");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    [SerializeField]private float _superAttackCooldown;
    

    private float lastAttackTime = 0;
    private bool isDead = false;
    public Animator AnimatorController;
    private bool _isSuperAttackOnCooldown=false;

    [SerializeField] private Button _superAttackButton;
    [SerializeField] private TMP_Text _cooldownText;
   


    private void Start()
    {
        _superAttackButton.onClick.AddListener(PerformSuperAttack);
    }
    private void Update()
    {
        Enemie closestEnemy = FindClosestEnemy();

        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!isDead)
            {
                if (closestEnemy != null)
                {
                    Attack(closestEnemy);
                }
            }
        }
        
    }

    private void Die()
    {
        _superAttackButton.interactable = false;
        isDead = true;
        AnimatorController.SetTrigger("Die");
        GetComponent<Movement>().SetPlayerAlive(false);
        SceneManager.Instance.GameOver();
    }

    private Enemie FindClosestEnemy()
    {
        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemy = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            if (enemy == null)
            {
                continue;
            }

            if (closestEnemy == null)
            {
                closestEnemy = enemy;
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

  
    private void Attack(Enemie targetEnemy)
    {
        var distanceToAttack = Vector3.Distance(transform.position, targetEnemy.transform.position);

        if (Time.time - lastAttackTime > AtackSpeed)
        {
            //transform.LookAt(targetEnemy.transform);
            transform.rotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);

            lastAttackTime = Time.time;
            AnimatorController.SetTrigger("Attack");

            if (distanceToAttack <= AttackRange)
            {
                targetEnemy.Hp -= Damage;
            }
        }
    }

    private void SuperAttack(Enemie targetEnemy)
    {
        if (_isSuperAttackOnCooldown) 
        {
            return;
        }
        var distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
        if (distance <= AttackRange)
        {
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                //transform.LookAt(closestEnemie.transform);
                transform.transform.rotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);

                lastAttackTime = Time.time;
                targetEnemy.Hp -= Damage*2;
                AnimatorController.SetTrigger("SuperAttack");
                StartCoroutine(SuperAttackCooldown());
            }
        }
    }
    
    private IEnumerator SuperAttackCooldown()
    {
        _isSuperAttackOnCooldown = true;
        _superAttackButton.interactable = false;
        float cooldownTime = _superAttackCooldown;

        while (cooldownTime > 0)
        {
            _cooldownText.text = cooldownTime.ToString("F1");
            yield return new WaitForSeconds(1f);
            cooldownTime -= 1f;
        }
        _isSuperAttackOnCooldown = false;
        _superAttackButton.interactable = true;
        _cooldownText.text = "Super Attack";
    }

    private void PerformSuperAttack()
    {
        Enemie closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            SuperAttack(closestEnemy);
        }
    }

    public void KillHealing()
    {
        
        Hp += 2;
        
    }
}

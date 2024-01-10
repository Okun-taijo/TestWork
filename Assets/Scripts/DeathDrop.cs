using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : Enemie
{
    [SerializeField]private GameObject _spawlings;
    
    private void SpawnWeakerEnemies()
    {
        
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            GameObject newEnemy = Instantiate(_spawlings, spawnPosition, Quaternion.identity);
            Enemie newEnemyScript = newEnemy.GetComponent<Enemie>();
        }
    }
    private void Update()
    {
        
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            
            Die();
            Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.Hp -= Damage;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.speed);
        Debug.Log(Agent.speed);

    }
    protected void Die()
    {
        if (_player != null)
        {
            _player.KillHealing();
        }
        SceneManager.Instance.RemoveEnemie(this);
        isDead = true;
        AnimatorController.SetTrigger("Die");
        SpawnWeakerEnemies();
    }

}

using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObjects {

    public int playerDamage;
    public AudioClip enemyAtackSound1;
    public AudioClip enemyAtackSound2;

    private Animator animator;
    private Transform target;
    private bool skipMove;

	
	protected override void Start ()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAtack");
        SoundManager.instance.RandomizeSfx(enemyAtackSound1, enemyAtackSound2);
        hitPlayer.LoseFood(playerDamage);
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }
}

using UnityEngine;

public class RandomAnimMenu : MonoBehaviour
{
    private Animator animator;
    private float timer = 0f;
    private float idleDuration = 12f;
    private int? oldInt = null;
    private int repeatZeroCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            timer = 0f;
            PlayRandomIdle();
        }
    }

    void PlayRandomIdle()
    {
        int randomIndex = Random.Range(0, 3);
        if (randomIndex == 0 && oldInt == 0)
        {
            repeatZeroCount++;
        }
        else
        {
            repeatZeroCount = 0;
        }

        while ((randomIndex == oldInt && randomIndex != 0) || (randomIndex == 0 && repeatZeroCount >= 2))
        {
            randomIndex = Random.Range(0, 3);
            if (randomIndex == 0 && oldInt == 0)
                repeatZeroCount++;
            else
                repeatZeroCount = 0;
        }

        oldInt = randomIndex;
        animator.SetInteger("RandomAnim", randomIndex);
        idleDuration = 24f;
        //Debug.Log(randomIndex);
    }
}

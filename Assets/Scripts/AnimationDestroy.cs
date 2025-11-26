using UnityEngine;

public class AnimationDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool ableToDestroy = false;

    void Update()
    {
        if (ableToDestroy) destroyThis();
    }

    public void destroyThis()
    {
        Destroy(gameObject);
    }
}

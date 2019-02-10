using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Paddle")
        {
            this.ApplyEffect();
        }

        if (collision.tag == "Paddle" || collision.tag == "DeathWall")
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void ApplyEffect();
}

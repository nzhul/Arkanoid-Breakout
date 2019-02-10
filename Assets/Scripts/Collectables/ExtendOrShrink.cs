public class ExtendOrShrink : Collectable
{
    public float NewWidth = 2.5f;

    protected override void ApplyEffect()
    {
        if (Paddle.Instance != null && !Paddle.Instance.PaddleIsTransforming)
        {
            Paddle.Instance.StartWidthAnimation(NewWidth);
        }
    }
}

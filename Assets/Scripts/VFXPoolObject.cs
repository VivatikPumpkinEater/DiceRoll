using CartoonFX;

public class VFXPoolObject : CFXR_Effect //CFXR_Effect сторонний класс в рамках тестового я особо в него не вникал
{
    protected override void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
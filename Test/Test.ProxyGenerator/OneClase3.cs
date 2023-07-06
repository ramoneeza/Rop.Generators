namespace Test.ProxyGenerator;

public abstract class OneClase3
{
    public virtual int MyMethod(int a, int b)
    {
        return a + b;
    }
    public virtual int MyMethod2(int a, int b)
    {
        return a - b;
    }
    protected abstract int MyMethod3(int a, int b);
    protected abstract int MyMethod4(int a, int b);
}
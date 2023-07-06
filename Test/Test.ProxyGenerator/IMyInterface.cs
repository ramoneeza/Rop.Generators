using Rop.ProxyGenerator.Annotations;

namespace Test.ProxyGenerator;


public interface ISub
{
    [Explicit] 
    object SubProperty { get; set; }

    [Explicit]
    object SubMethod(string hello);
    [Exclude]
    object SubExcluido { get; }
}

public interface IMyInterface<T>:ISub
{
    event EventHandler ValueChanged;
    event EventHandler<T> TValueChanged;
    void OnValueChanged();
    void OnIntValueChanged();
    string Name { get; set; }
    string Description { get; set; }
    DateTime Fecha { get; }
    string SacaAlgo(T valor, string otroparametro);
}
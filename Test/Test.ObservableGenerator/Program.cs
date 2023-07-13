using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rop.ObservableGenerator.Annotations;

namespace Test.ObservableGenerator;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }

}

public partial class Example1 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [AutoNotify]
        private string _myproperty = string.Empty;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    public partial class Example2
    {

        [AutoObservable]
        private string _myPropertyOne = string.Empty;

        [AutoObservable]
        private string _myPropertyTwo = string.Empty;
        private void _onMyPropertyTwoChanged()
        {
            Invalidate();
        }

        [AutoObservable(true)]
        private string _myPropertyThree = string.Empty;

        public event EventHandler? MyPropertyThreeChanged;
        public virtual void OnMyPropertyThreeChanged()
        {
            Invalidate();
            MyPropertyThreeChanged?.Invoke(this, EventArgs.Empty);
        }


        public void Invalidate()
        {
            // Refresh UI
        }
    }



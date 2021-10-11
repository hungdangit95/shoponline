namespace SharedKernel.Linq
{
    internal interface IValueComparer<in T>
    {
        bool Compare(T x, T y);
    }
}
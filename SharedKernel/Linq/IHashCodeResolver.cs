namespace SharedKernel.Linq
{
    internal interface IHashCodeResolver<in T>
    {
        int GetHashCodeFor(T obj);
    }
}
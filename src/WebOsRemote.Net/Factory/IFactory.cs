namespace WebOsRemote.Net.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}

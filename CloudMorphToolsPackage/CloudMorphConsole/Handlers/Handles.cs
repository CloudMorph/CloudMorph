namespace CloudMorphConsole.Handlers
{
    public interface Handles<T>
    {
        void Handle(T message);
    }
}
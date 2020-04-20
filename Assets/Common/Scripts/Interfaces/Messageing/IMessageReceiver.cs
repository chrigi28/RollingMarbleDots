namespace Assets.Common.Scripts.Components
{
    public interface IMessageReceiver<T> where T : IMessageBase
    {
        void ExecuteMessage(T m);
    }
}
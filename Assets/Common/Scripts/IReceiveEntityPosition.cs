using Unity.Transforms;

namespace Assets.Common.Scripts
{
    public interface IReceiveEntityPosition
    {
        void SendPosition(Translation pos);
    }
}
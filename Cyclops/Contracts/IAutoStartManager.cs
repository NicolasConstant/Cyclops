namespace Cyclops.Contracts
{
    public interface IAutoStartManager
    {
        bool IsAutoStartSet();
        void SetAutoStart(bool enabled);
    }
}
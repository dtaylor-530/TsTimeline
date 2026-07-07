namespace Common
{
    public interface IUpdater
    {
        bool CanUpdate(object control);
        void Update(object context);
    }
}
namespace CardGameTemplate
{
    public interface IBehaviourTargetWrapper
    {
        // "where T : class" is just to ensure it is a ref type. A option would be to make all,
        // possible targets holders inherit from a interface.
        public T GetSubTargetsHolder<T>() where T : class;
    }
}
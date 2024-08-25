namespace Bento
{
    public abstract partial class CanGetModel
    {
        private static IOCContainer m_Container = new();

        public static T GetModel<T>() where T : ModelBase, new()
        {
            var model = m_Container.Get<T>();
            if (model == null)
            {
                model = new T();
                model.Init();
                m_Container.Set(model);
            }
            return model;
        }
    }
}
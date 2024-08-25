namespace Bento
{
    public abstract class ModelBase
    {
        public void Init() => OnInit();

        protected abstract void OnInit();

        public abstract void Clear();

    }
}
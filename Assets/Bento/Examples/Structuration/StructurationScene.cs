using Bento;
using UnityEngine;

public class StructurationScene : MonoBehaviour, IView
{
    #region Models
    private class TestModel : ModelBase
    {
        public int Count { get; set; }

        protected override void OnInit()
        {
            Debug.Log("TestModel#OnInit");
        }

        public override void Clear()
        {
            Debug.Log("TestModel#Clear");
        }
    }
    #endregion

    #region Commands
    private class TestSetCommand : CommandBase
    {
        protected override void OnExecute()
        {
            var model = GetModel<TestModel>();
            model.Count++;
            Debug.Log("TestSetCommand#OnExecute");
        }
    }

    private class TestGetCommand : CommandBase<int>
    {
        protected override int OnExecute()
        {
            Debug.Log("TestGetCommand#OnExecute");
            return GetModel<TestModel>().Count;
        }
    }
    #endregion

    private void Start()
    {
        this.Send<TestSetCommand>();
        Debug.Log(this.Send<TestGetCommand, int>());
        this.Send<TestSetCommand>();
        Debug.Log(this.Send<TestGetCommand, int>());
    }

}

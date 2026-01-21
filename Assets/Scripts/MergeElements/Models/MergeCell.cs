using Core.MergeElements.Views;

namespace Core.MergeElements.Models
{
    class MergeCell
    {
        public MergeElement Model
        {
            get 
            {
                return _model;
            }
            set
            {
                _model = value;
                if(_model == null)
                {
                    View.gameObject.SetActive(false);
                }
                else
                {
                    View.SetVisualParams(_model.Icon);
                    View.gameObject.SetActive(true);
                }
            }
        }

        public MergeElementUI View { get; private set; }


        private MergeElement _model;

        public MergeCell(MergeElementUI mergeElementUI)
        {
            View = mergeElementUI;
            View.Init();
            View.gameObject.SetActive(false);

            _model = null;
        }

    }
}

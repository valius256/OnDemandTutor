namespace OnDemandTutor.DataAccess.Models.Exception
{
    public class ModelException : System.Exception
    {
        public string Field { get; set; }
        public string[] Paras { get; set; }

        public ModelException(string field, string errorCode, params string[] paras)
            : base(errorCode)
        {
            Field = field;
            Paras = paras;
        }
    }
}

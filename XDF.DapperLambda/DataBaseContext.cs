using XDF.DapperLambda.Model;

namespace XDF.DapperLambda
{
    public class DataBaseContext<T>
    {
        public CommandSet<T> CommandSet { get; set; }
         public QuerySet<T> QuerySet { get; set; }
        internal EOperateType OperateType { get; set; }
    }
}
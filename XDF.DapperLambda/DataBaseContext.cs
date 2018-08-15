using XDF.DapperLambda.Interface;
using XDF.DapperLambda.Model;

namespace XDF.DapperLambda
{
    public class DataBaseContext<T>
    {
        public Command<T> CommandSet => (Command<T>)Set;

        public ISet<T> Set { get; internal set; }

        internal EOperateType OperateType { get; set; }
    }
}
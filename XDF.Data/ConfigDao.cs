using XDF.Core.Entity;

namespace XDF.Data
{  
    /// <summary>
    /// Dao
    /// </summary>
    public partial class ConfigDao : BaseDao<ConfigEntity>
    {  
        public ConfigDao() : base("XDF")
        {
            base._Field = " [sName],[sValue],[sDescription] ";
            base._Primary = "id";
            base._FieldAt = " @sName,@sValue,@sDescription ";
            base._Table = " bs_Config ";
            base._FieldUp = " [sName]=@sName,[sValue]=@sValue,[sDescription]=@sDescription ";
            base._IsIdentity = true;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}


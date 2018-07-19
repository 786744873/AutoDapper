

using XDF.Core.Entity;

namespace XDF.Data
{
    /// <summary>
    /// Dao
    /// </summary>
    public partial class MenuDao : BaseDao<MenuEntity>
    {
        public MenuDao() : base("YSKJ")
        {
            base._Field = " C_ID AS 'ID',C_Code AS 'Code',C_Name AS 'Name',C_Index AS 'Index',C_FuncID AS 'FuncID',C_urlAddr AS 'urlAddr',C_imgAddr AS 'imgAddr',C_Level AS 'Level',C_ParentSeq AS 'ParentSeq',C_IsTheSamePage AS 'IsTheSamePage',C_Creator AS 'Creator',C_CreateTime AS 'CreateTime',C_Modifier AS 'Modifier',C_ModifyTime AS 'ModifyTime',C_CompanyID AS 'CompanyID',C_IsDelete AS 'IsDelete',C_SystemID AS 'SystemID' ";
            base._Primary = "C_ID";
            base._FieldAt = " @Code,@Name,@Index,@FuncID,@urlAddr,@imgAddr,@Level,@ParentSeq,@IsTheSamePage,@Creator,@CreateTime,@Modifier,@ModifyTime,@CompanyID,@IsDelete,@SystemID ";
            base._Table = " Base_Menu ";
            base._FieldUp = " C_Code=@Code,C_Name=@Name,C_Index=@Index,C_FuncID=@FuncID,C_urlAddr=@urlAddr,C_imgAddr=@imgAddr,C_Level=@Level,C_ParentSeq=@ParentSeq,C_IsTheSamePage=@IsTheSamePage,C_Creator=@Creator,C_CreateTime=@CreateTime,C_Modifier=@Modifier,C_ModifyTime=@ModifyTime,C_CompanyID=@CompanyID,C_IsDelete=@IsDelete,C_SystemID=@SystemID ";
            base._IsIdentity = false;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}


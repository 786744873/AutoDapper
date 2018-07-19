using System;
using System.Collections.Generic;
using System.Text;
using XDF.Core.Entity;

namespace XDF.Data
{
    /// <summary>
    /// FunctionDao
    /// </summary>
    public partial class FunctionDao : BaseDao<FunctionEntity>
    {
        public FunctionDao() : base("YSKJ")
        {
            base._Field = " Id,PId,Name,Des,ApiPath,Type,UrlPath,ComponentPath,Icon,Level,Sort,IsNavigation,IsButton,IsShortcut ";
            base._Primary = "Id";
            base._FieldAt = " @PId,@Name,@Des,@ApiPath,@Type,@UrlPath,@ComponentPath,@Icon,@Level,@Sort,@IsNavigation,@IsButton,@IsShortcut ";
            base._Table = " Function ";
            base._FieldUp = " PId=@PId,Name=@Name,Des=@Des,ApiPath=@ApiPath,Type=@Type,UrlPath=@UrlPath,ComponentPath=@ComponentPath,Icon=@Icon,Level=@Level,Sort=@Sort,IsNavigation=@IsNavigation,IsButton=@IsButton,IsShortcut=@IsShortcut ";
            base._IsIdentity = true;//主建是否自增，如果不自增插入的时候要赋值
        }
    }
}

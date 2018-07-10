using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Dto
{
    public class LoginSuccessDto
    {
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}

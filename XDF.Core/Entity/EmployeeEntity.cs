using System;
using System.Collections.Generic;
using System.Text;

namespace XDF.Core.Entity
{
  public  class EmployeeEntity
    {
        public int Id { get; set; }
        public int NSchoolId { get; set; }
        public string SName { get; set; }
        public string SCode { get; set; }
        public string SNameSpell { get; set; }
        public int NGender { get; set; }
        public string SPhone { get; set; }
        public string SFax { get; set; }
        public string SEmail { get; set; }
        public string SDeptCode { get; set; }
        public string SAddress { get; set; }
        public string SPostCode { get; set; }
        public string SMemo { get; set; }
        public bool BValid { get; set; }
        public int NInUsed { get; set; }
        public string SLoginName { get; set; }
        public byte[] SPassword { get; set; }
        public DateTime PwdChangeDate { get; set; }
    }
}

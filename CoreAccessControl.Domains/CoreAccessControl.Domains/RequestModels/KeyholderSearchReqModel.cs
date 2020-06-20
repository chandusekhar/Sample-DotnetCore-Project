using System;
using System.Collections.Generic;
using System.Text;

namespace CoreAccessControl.Domain.ResponseModels
{
    public class KeyholderSearchReqModel
    {
        public string KeySerialNumber { get; set; }
        public string Pin { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long? State { get; set; }
        public long? StatusId { get; set; }
        public int Skips { get; set; }
        public int Takes { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }
}

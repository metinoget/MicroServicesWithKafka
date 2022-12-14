using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ContactMicroService.Api.Helpers
{
    public abstract class BaseResponse
    {
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = null;
        public string FriendlyMessage { get; set; }
        public List<string> Errors { get; set; } = null;
        public int ErrorCode { get; set; } = 0;
        public int? RecordsTotal { get; set; } = null;
        public int? RecordsFiltered { get; set; } = null;
        
    }
}

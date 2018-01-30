using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MacaMvc.IHttpHelpers
{
    public interface IHttpPayloadClient
    {
        Task<HttpClient> GetClient();
    }
}

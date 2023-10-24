using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStoreAPI.Base.Token
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
    }
}
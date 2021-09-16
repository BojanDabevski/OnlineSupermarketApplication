using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSupermarket.Domain.Identity
{
    public class EShopApplicationUser
    {
        public string NormalizedEmail { get; set; }
        public string Email { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserName { get; set; }
    }
}

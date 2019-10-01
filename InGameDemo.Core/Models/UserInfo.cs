using System;

namespace InGameDemo.Core.Models
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string[] Roles { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}

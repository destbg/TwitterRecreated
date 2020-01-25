using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Posts = new HashSet<Post>();
        }

        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Verified { get; set; }
        public DateTime JoinedOn { get; set; }
        public DateTime LastLogin { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}

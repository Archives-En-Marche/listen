using System;
using Realms;

namespace Listen.Models.RealmObjects
{
    public class User : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString(); 

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset LastAccess { get; set; }
    }
}

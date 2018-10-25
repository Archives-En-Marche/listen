using System;
using Realms;

namespace Listen.Models.RealmObjects
{
    public class User : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Uuid { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Mail { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset LastAccess { get; set; }

        public User Clone()
        {
            return new User()
            {
                Uuid = this.Uuid,
                LastName = this.LastName,
                FirstName = this.FirstName,
                Mail = this.Mail,
                Country = this.Country,
                City = this.City,
                ZipCode = this.ZipCode,
                Token = this.Token,
                RefreshToken = this.RefreshToken,
                LastAccess = this.LastAccess
            };
        }
    }
}

using System;
using Realms;
using System.Collections.Generic;

namespace Listen.Models.RealmObjects
{
    public class Survey : RealmObject, ICloneable
    {
        public string Uuid { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Questions { get; set; }

        public object Clone()
        {
            return new Survey()
            {
                Uuid = this.Uuid,
                Name = this.Name,
                Type = this.Type,
                Questions = this.Questions
            };
        }
    }
}

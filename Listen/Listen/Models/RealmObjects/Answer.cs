using System;
using Realms;

namespace Listen.Models.RealmObjects
{
    public class Answer : RealmObject
    {
        public string Uuid { get; set; }

        public string Content { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }

        public Answer()
        {

        }
    }
}

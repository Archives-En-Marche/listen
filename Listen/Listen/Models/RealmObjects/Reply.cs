using System;
using Realms;

namespace Listen.Models.RealmObjects
{
    public class Reply : RealmObject, ICloneable
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string SurveyId { get; set; }

        public string Answer { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool Uploading { get; set; }

        public object Clone()
        {
            return new Reply()
            {
                Id = this.Id,
                SurveyId = this.SurveyId,
                Answer = this.Answer,
                Date = this.Date,
                Uploading = this.Uploading
            };
        }
    }
}

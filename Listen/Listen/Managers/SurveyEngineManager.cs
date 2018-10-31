using System;
using System.Collections.Generic;
using Listen.Models.RealmObjects;
using Listen.Models.WebServices;
using Newtonsoft.Json;
using Xamarin.Forms;
using Reply = Listen.Models.WebServices.Reply;
using Survey = Listen.Models.RealmObjects.Survey;

namespace Listen.Managers
{
    public class SurveyEngineManager
    {
        private static readonly Lazy<SurveyEngineManager> lazy = new Lazy<SurveyEngineManager>(() => new SurveyEngineManager());

        public SurveyEngineManager()
        {
        }

        public static SurveyEngineManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        int _pos = 0;

        public Survey CurrentSurvey { get; set; }

        public Reply CurrentReply { get; set; }

        //public Question CurrentQuestion { get; set; }

        public IList<Question> Questions { get; set; }

        public int Count { get; set; }

        public int QuestionNumber { get { return _pos + 1; } }

        public void Init(Survey survey)
        {
            _pos = -1;

            CurrentSurvey = survey;

            Questions = JsonConvert.DeserializeObject<List<Question>>(survey.Questions);

            Count = Questions.Count;

            CurrentReply = new Reply();
            CurrentReply.Survey = CurrentSurvey.Uuid;
            CurrentReply.Answers = new List<Answer>();
        }

        public void InitCurrentSurvey()
        {
            _pos = -1;
            CurrentReply = new Reply();
            CurrentReply.Survey = CurrentSurvey.Uuid;
            CurrentReply.Answers = new List<Answer>();
        }

        public Question GetNextQuestion()
        {
            _pos++;
            if (_pos >= 0 && _pos < Count)
            {
                return Questions[_pos];
            }
            else
            {
                return null;
            }
        }

        public bool IsLastQuestion { get => _pos == Count - 1; }
    }
}


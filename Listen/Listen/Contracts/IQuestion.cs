using System;
using Listen.Models.WebServices;

namespace Listen.Contracts
{
    public interface IResetQuestion
    {
        void Reset();
        bool HasValidated();
        void UpdateAnswers(Question question);
    }
}

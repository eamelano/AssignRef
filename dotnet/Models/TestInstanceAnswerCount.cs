using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.TestInstances
{
    public class TestInstanceAnswerCount : BaseTestInstance
    {
        public int QuestionCount { get; set; }
        public int CorrectAnswers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.TestInstances
{
    public class TestQuestionDetailed
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string Question { get; set; }
        public string HelpText { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultipleAllowed { get; set; }
        public int TestId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int SortOrder { get; set; }
        public List<InstanceAnswerOption> AnswerOptions { get; set; }
        public List<InstanceAnswer> Answer { get; set; }

    }
}

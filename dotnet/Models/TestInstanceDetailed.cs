using Sabio.Models.Domain.Tests;
using System;
using System.Collections.Generic;

namespace Sabio.Models.Domain.TestInstances
{
    public class TestInstanceDetailed
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser User { get; set; }
        public LookUp Status { get; set; }
        public TestV1 Test { get; set; }
        public List<TestQuestionDetailed> Questions { get; set; }

    }
}

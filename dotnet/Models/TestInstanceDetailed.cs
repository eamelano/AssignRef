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

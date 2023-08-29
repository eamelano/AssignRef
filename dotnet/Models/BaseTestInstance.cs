public class BaseTestInstance
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public LookUp Status { get; set; }
        public BaseUser User { get; set; }
        public LookUp Test { get; set; }
        public LookUp TestType { get; set; }
    }

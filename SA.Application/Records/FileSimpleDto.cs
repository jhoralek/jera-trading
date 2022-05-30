using System;

namespace SA.Application.Records
{
    public class FileSimpleDto
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }
        public virtual int UserId { get; set; }
        public virtual int RecordId { get; set; }
        public virtual DateTime Created { get; set; }
    }
}

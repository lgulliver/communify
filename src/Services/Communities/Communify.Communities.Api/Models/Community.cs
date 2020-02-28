using System;

namespace Communify.Communities.Api.Models
{
    public class Community
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

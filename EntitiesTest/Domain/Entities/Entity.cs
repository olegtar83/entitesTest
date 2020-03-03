using System.Collections.Generic;

namespace EntitiesTest.Domain.Entities
{
    public class Entity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<double> Parameters { get; set; }
    }
}

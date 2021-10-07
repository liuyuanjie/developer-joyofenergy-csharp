using System;

namespace JOIEnergy.Domain.Entity
{
    public class Account: EntityBase
    {
        public string Name { get; set; }

        public static Account Create(string name)
        {
            return new Account()
            {
                Name = name
            };
        }
    }
}

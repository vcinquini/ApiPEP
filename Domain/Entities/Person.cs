using System;

namespace Domain.Entities
{
    public class Person
    {
        public Person(long cpf, DateTime creation, DateTime change, int pep, int pepType)
        {
            if (cpf < 1)
            {
                throw new ArgumentException("Invalid CPF (must be greater than zero.");
            }

            this.CPF = cpf;
            this.Creation = creation;
            this.Change = change;
            this.PEP = pep;
            this.PEPType = pepType;
        }

        public long CPF { get; }
        public DateTime Creation { get; }
        public DateTime Change { get; }
        public int PEP { get; }
        public int PEPType { get; }

    }
}

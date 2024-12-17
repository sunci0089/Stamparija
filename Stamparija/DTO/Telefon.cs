using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Telefon
    {
        public string brTel { get; set; }
        public Saradnik saradnik { get; set; }

        public Telefon(string brTel, Saradnik saradnik)
        {
            this.brTel = brTel;
            this.saradnik = saradnik;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Telefon other = (Telefon)obj;
            return brTel == other.brTel;
        }

        public override int GetHashCode()
        {
            return brTel?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return brTel; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

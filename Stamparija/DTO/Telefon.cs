using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Telefon
    {
        public string BrTel { get; set; }
        public Saradnik Saradnik { get; set; }

        public Telefon()
        {
            Saradnik = new Saradnik();
        }
        public Telefon(string brTel, Saradnik saradnik)
        {
            this.BrTel = brTel;
            this.Saradnik = saradnik;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Telefon other = (Telefon)obj;
            return BrTel == other.BrTel;
        }

        public override int GetHashCode()
        {
            return BrTel?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return BrTel;
        }
    }
}

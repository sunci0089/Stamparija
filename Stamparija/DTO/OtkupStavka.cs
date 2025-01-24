using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class OtkupStavka
    {
        public Otkup otkup { get; set; }
        public Artikal artikal { get; set; }

        public double kolicina { get; set; }

        public OtkupStavka()
        {
            otkup = new Otkup();
            artikal = new Artikal();
        }

        public OtkupStavka(Otkup otkup, Artikal artikal, double kolicina)
        {
            this.otkup = otkup;
            this.artikal = artikal;
            this.kolicina = kolicina;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            OtkupStavka other = (OtkupStavka)obj;
            return otkup == other.otkup && artikal==other.artikal;
        }

        public override int GetHashCode()
        {
            return otkup?.GetHashCode() ?? 0 + artikal?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return otkup + " - " + artikal; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

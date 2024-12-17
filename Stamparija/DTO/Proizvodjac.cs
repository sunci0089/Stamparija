using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Proizvodjac
    {
        public string sifra { get; set; }
        public string ime { get; set; }
        public string drzavaPorijekla { get; set; }
        public Proizvodjac() { }

        public Proizvodjac(string sifra, string ime, string drzavaPorijekla)
        {
            this.sifra = sifra;
            this.ime = ime;
            this.drzavaPorijekla = drzavaPorijekla;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Proizvodjac other = (Proizvodjac)obj;
            return sifra == other.sifra;
        }

        public override int GetHashCode()
        {
            return sifra?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return sifra; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

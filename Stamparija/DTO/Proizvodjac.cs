using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Proizvodjac
    {
        public string Sifra { get; set; }
        public string Ime { get; set; }
        public string DrzavaPorijekla { get; set; }
        public Proizvodjac() { }

        public Proizvodjac(string sifra, string ime, string drzavaPorijekla)
        {
            this.Sifra = sifra;
            this.Ime = ime;
            this.DrzavaPorijekla = drzavaPorijekla;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Proizvodjac other = (Proizvodjac)obj;
            return Sifra == other.Sifra;
        }

        public override int GetHashCode()
        {
            return Sifra?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Sifra; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

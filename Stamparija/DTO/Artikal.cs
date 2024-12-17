using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    [Serializable]
    public class Artikal
    {
        public string sifra { get; set; }
        public string naziv { get; set; }
        public double kolicina { get; set; }
        public double cijenaBezMarze { get; set; }
        public string kategorija { get; set; }
        public double marza { get; set; }

        public double proizvodjac_sifra { get; set; }

        public Artikal() { }

        public Artikal(string sifra, string naziv, double kolicina,
            double cijenaBezMarze, string kategorija, double marza, double proizvodjac_sifra)
        {
            this.sifra = sifra;
            this.naziv = naziv;
            this.kolicina = kolicina;
            this.cijenaBezMarze = cijenaBezMarze;
            this.kategorija = kategorija;
            this.marza = marza;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Artikal other = (Artikal)obj;
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

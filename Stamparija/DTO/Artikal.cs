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
        public string Sifra { get; set; }
        public string Naziv { get; set; }
        public double Kolicina { get; set; }
        public double CijenaBezMarze { get; set; }
        public string Kategorija { get; set; }
        public double Marza { get; set; }
        public Proizvodjac Proizvodjac { get; set; }

        public Artikal() {
            Proizvodjac = new Proizvodjac();
        }

        public Artikal(string sifra, string naziv, double kolicina,
            double cijenaBezMarze, string kategorija, double marza, Proizvodjac proizvodjac)
        {
            this.Sifra = sifra;
            this.Naziv = naziv;
            this.Kolicina = kolicina;
            this.CijenaBezMarze = cijenaBezMarze;
            this.Kategorija = kategorija;
            this.Marza = marza;
            this.Proizvodjac = proizvodjac;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Artikal other = (Artikal)obj;
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

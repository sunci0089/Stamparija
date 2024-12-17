using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    [Serializable]
    public class Saradnik
    {
        public string sifra { get; set; }
        public Mjesto mjesto { get; set; }
        public string naziv { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string jib { get; set; }
        public string jmb { get; set; }
        public string vrsta { get; set; }
        public Saradnik() { }

        public Saradnik(string sifra, string ime, Mjesto mjesto,
            string naziv, string prezime, string jib, string jmb,
            string vrsta)
        {
            this.sifra = sifra;
            this.ime = ime;
            this.mjesto = mjesto;
            this.naziv = naziv;
            this.prezime = prezime;
            this.jib = jib;
            this.jmb = jmb;
            this.vrsta = vrsta;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Saradnik other = (Saradnik)obj;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Otkup
    {
        public string sifra { get; set; }
        public Faktura faktura { get; set; }
        public Saradnik saradnik { get; set; }
        public Otkup() { }

        public Otkup(string sifra, Faktura faktura, Saradnik saradnik, string v)
        {
            this.sifra = sifra;
            this.faktura = faktura;
            this.saradnik = saradnik;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Otkup other = (Otkup)obj;
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

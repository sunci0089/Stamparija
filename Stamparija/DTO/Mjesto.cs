using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Mjesto
    {
        public string PostanskiBroj { get; set; }
        public string Naziv { get; set; }
        public Mjesto() { }

        public Mjesto(string postanskiBroj, string naziv)
        {
            this.PostanskiBroj = postanskiBroj;
            this.Naziv = naziv;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Mjesto other = (Mjesto)obj;
            return PostanskiBroj == other.PostanskiBroj;
        }

        public override int GetHashCode()
        {
            return PostanskiBroj?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return PostanskiBroj; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

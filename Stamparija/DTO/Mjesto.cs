using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Mjesto
    {
        public string postanskiBroj { get; set; }
        public string naziv { get; set; }
        public Mjesto() { }

        public Mjesto(string postanskiBroj, string naziv)
        {
            this.postanskiBroj = postanskiBroj;
            this.naziv = naziv;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Mjesto other = (Mjesto)obj;
            return postanskiBroj == other.postanskiBroj;
        }

        public override int GetHashCode()
        {
            return postanskiBroj?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return postanskiBroj; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

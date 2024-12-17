using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
        [Serializable]
        public class Faktura
        {
            public string Sifra { get; set; }
            public string DatumVrijeme { get; set; } // datetime as a string
            public string NacinPlacanja { get; set; }
            public string ZiroracunSaradnika { get; set; }
            public string VrstaUplate { get; set; }
            public double CijenaSaPDV { get; set; }

            public Faktura() { }

            public Faktura(string sifra, string datumVrijeme, string nacinPlacanja, string ziroracunSaradnika, string vrstaUplate, double cijenaSaPDV)
            {
                Sifra = sifra;
                DatumVrijeme = datumVrijeme;
                NacinPlacanja = nacinPlacanja;
                ZiroracunSaradnika = ziroracunSaradnika;
                VrstaUplate = vrstaUplate;
                CijenaSaPDV = cijenaSaPDV;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj)) return true;
                if (obj == null || GetType() != obj.GetType()) return false;

                Faktura other = (Faktura)obj;
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

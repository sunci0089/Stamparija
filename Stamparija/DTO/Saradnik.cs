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
        public string Sifra { get; set; }
        public Mjesto Mjesto { get; set; }
        public string Naziv { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JIB { get; set; }
        public string JMB { get; set; }
        public string Vrsta { get; set; }

        public Saradnik()
        {
            // Initialize Mjesto to avoid null reference issues
            this.Mjesto = new Mjesto();
        }
        public Saradnik(string sifra, string ime, string postanskiBroj, string nazivMjesta,
            string naziv, string prezime, string jib, string jmb,
            string vrsta)
        {
            this.Sifra = sifra;
            this.Ime = ime;
            this.Mjesto = new Mjesto
            {
                Naziv = nazivMjesta,
                PostanskiBroj = postanskiBroj
            };
            this.Naziv = naziv;
            this.Prezime = prezime;
            this.JIB = jib;
            this.JMB = jmb;
            this.Vrsta = vrsta;
        }

        public string isSaradnikDataValid()
        {
            string validationError = null;
            // Validate 'sifra'
            if (string.IsNullOrWhiteSpace(Sifra))
            {
                validationError = "Sifra is required.";
                return validationError;
            }

            // Validate 'postanskiBroj'
            if (string.IsNullOrWhiteSpace(Mjesto.PostanskiBroj) || !int.TryParse(Mjesto.PostanskiBroj, out _))
            {
                validationError = "Invalid postanski broj.";
                return validationError;
            }

            // Validate 'vrsta'
            if (string.IsNullOrWhiteSpace(Vrsta))
            {
                validationError = "Vrsta is required.";
                return validationError;
            }

            // Optional fields: Validate if provided
            if (!string.IsNullOrWhiteSpace(Ime) && Ime.Length > 50)
            {
                validationError = "Ime must be 50 characters or fewer.";
                return validationError;
            }

            /*if (!string.IsNullOrWhiteSpace(nazivMjesta) && nazivMjesta.Length > 100)
            {
                validationError = "Naziv mjesta must be 100 characters or fewer.";
                return false;
            }*/

            if (!string.IsNullOrWhiteSpace(Naziv) && Naziv.Length > 100)
            {
                validationError = "Naziv must be 100 characters or fewer.";
                return validationError;
            }

            if (!string.IsNullOrWhiteSpace(Prezime) && Prezime.Length > 50)
            {
                validationError = "Prezime must be 50 characters or fewer.";
                return validationError;
            }

            if (!string.IsNullOrWhiteSpace(JIB) && JIB.Length != 12)
            {
                validationError = "JIB must be exactly 12 characters.";
                return validationError;
            }

            if (!string.IsNullOrWhiteSpace(JMB) && JMB.Length != 13)
            {
                validationError = "JMB must be exactly 13 characters.";
                return validationError;
            }

            return validationError;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Saradnik other = (Saradnik)obj;
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

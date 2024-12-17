using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Zaposleni
    {
        public string id {  get; set; }
        public string ime { get; set; }

        public string prezime { get; set; }
        public string jmb { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
        public Zaposleni() { }
        public Zaposleni(string id, string ime, string prezime, string jmb, string username, string password, bool isAdmin)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.jmb = jmb;
            this.username = username;
            this.password = password;
            this.isAdmin = isAdmin;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Zaposleni other = (Zaposleni)obj;
            return id == other.id && username== other.username;
        }

        public override int GetHashCode()
        {
            return id?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return id; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

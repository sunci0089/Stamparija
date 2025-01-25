using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Zaposleni
    {
        public int id {  get; set; }
        public string ime { get; set; }

        public string prezime { get; set; }
        public string jmb { get; set; }
        public string username { get; set; }
        public string password { get; set; } //todo trebalo bi samo napraviti select na bazu u obliku passwordhash==passwordhash
        public byte isAdmin { get; set; }
        public Zaposleni() { }
        public Zaposleni(int id, string ime, string prezime, string jmb, string username, string password, byte isAdmin)
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

        public override string ToString()
        {
            return username; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

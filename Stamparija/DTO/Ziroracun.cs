using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stamparija.DTO
{
    public class Ziroracun
    {
        public string brojRacuna { get; set; }
        public Saradnik saradnik { get; set; }
        public string banka {  get; set; }

        public Ziroracun(string brojRacuna, Saradnik saradnik, string banka)
        {
            this.brojRacuna = brojRacuna;
            this.saradnik = saradnik;
            this.banka = banka;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Ziroracun other = (Ziroracun)obj;
            return brojRacuna == other.brojRacuna;
        }

        public override int GetHashCode()
        {
            return brojRacuna?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return brojRacuna; // or $"{DatumVrijeme} - {VrstaUplate}" if needed
        }
    }
}

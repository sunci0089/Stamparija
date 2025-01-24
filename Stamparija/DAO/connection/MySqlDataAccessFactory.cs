using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stamparija.DTO;
using Stamparija.DAO;
using System.Configuration;

namespace Stamparija.DAO.connection
{
    public class MySQLDataAccessFactory
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        public static SaradnikDAO GetSaradnikDataAccess() //1
        {
            return new SaradnikDAO(CONNECTION_STRING);
        }

        public static OtkupDAO GetOtkupDataAccess() //2
        {
            return new OtkupDAO(CONNECTION_STRING);
        }

        public static FakturaDAO GetFakturaDataAccess() //3
        {
            return new FakturaDAO(CONNECTION_STRING); 
        }

        public static ArtikalDAO GetArtikalDataAccess() //4
        {
            return new ArtikalDAO(CONNECTION_STRING);
        }

        public static OtkupStavkaDAO GetOtkupStavkaDataAccess() //5
        {
            return new OtkupStavkaDAO(CONNECTION_STRING);
        }

        public static ProizvodjacDAO GetProizvodjacDataAccess() //6
        {
            return new ProizvodjacDAO(CONNECTION_STRING);
        }

        public static ZaposleniDAO GetZaposleniDataAccess() //7
        {
            return new ZaposleniDAO(CONNECTION_STRING);
        }

        public static MjestoDAO GetMjestoDataAccess() //8
        {
            return new MjestoDAO(CONNECTION_STRING);
        }

        public static TelefonDAO GetTelefonDataAccess() //9
        {
            return new TelefonDAO(CONNECTION_STRING);
        }

        public static ZiroracunDAO GetZiroracunDataAccess() //10
        {
            return new ZiroracunDAO(CONNECTION_STRING);
        }
    }

}
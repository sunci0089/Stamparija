USE stamparija_hci;
INSERT INTO Mjesto (PostanskiBroj, Naziv) VALUES
('10000', 'Zagreb'),
('11000', 'Beograd'),
('78000', 'Banja Luka'),
('71000', 'Sarajevo');

INSERT INTO Saradnik (Sifra, Mjesto_PostanskiBroj, Naziv, Ime, Prezime, JIB, JMB, Vrsta) VALUES
('S001', '10000', 'A Firma',null,null,'123456789123',null,'Kupac'),
('S002', '11000', 'B Dobavljac',null,null,'876543210987',null,'Dobavljac'),
('S003', '78000', null, 'Marko', 'Markic', null , '0202993500025', 'Kupac');

INSERT INTO Telefon (BrTel, Saradnik_Sifra) VALUES
('123-456', 'S001'),
('234-567', 'S002'),
('223-456', 'S002'),
('345-678', 'S003');

INSERT INTO Proizvodjac (Sifra, Ime, DrzavaPorijekla) VALUES
('PR001', 'Proizvodjac 1', 'Hrvatska'),
('PR002', 'Proizvodjac 2', 'Srbija'),
('PR003', 'Proizvodjac 3', 'Kina');

INSERT INTO Artikal (Sifra, Naziv, Kolicina, CijenaBezMarze, Kategorija, Marza, Proizvodjac_sifra) VALUES
('R001', 'PAPIR 80GR 610X860', 500.00, 0.11, 'PAPIR', 20.00, 'PR001'),
('R002', 'LJEPENKA 1.5MM', 6.50, 1.50, 'LJEPENKA', 20.00, 'PR002'),
('R003', 'PAPIR 120GR 70X100', 300.00, 0.11, 'PAPIR', 20.00, 'PR001'),
('R004', 'PLATNO PLAVO', 2, 3.95, 'PLATNO', 20.00, 'PR002'),
('R005', 'SKOLSKI DNEVNIK', 10.00, '19.93', 'KNJIGA', 20.00, 'PR001');

INSERT INTO ZiroRacun (BrojRacuna, Banka, Saradnici_Sifra) VALUES
('1234123412341234', 'A Banka', 'S004'),
('1230123012301230', 'B Banka', 'S001');

INSERT INTO Faktura (Sifra, DatumVrijeme, NacinPlacanja, ZiroRacun_Saradnika, VrstaUplate, CijenaSaPDV) 
VALUES ('F001', '2023-01-01 10:00:00', 'ZIRALNO', '1234123412341234', 'KUPOVINA', 123.61);
-- ('F002', '2023-02-01 12:00:00', 'GOTOVINSKI', '987654321', 'Kartica', 200.00);

INSERT INTO OTKUP (BrojPotvrde, Faktura_sifra, Saradnici_Sifra) 
VALUES ('0001', 'F001', 'S004');

INSERT INTO OTKUP_STAVKA(Otkup_BrojPotvrde, Roba_Sifra, Kolicina) 
VALUES ('0001', 'R001', 10.00),
('0001', 'R005', 1.00);

INSERT INTO Zaposleni (Ime, Prezime, JMB, Username, Password, ID, isAdmin ) VALUES
('Petar', 'Petrovic', '1234123412345', 'admin', 'admin', '0', 1),
('Mira','Miric', '1231231231234','miramiric','miramiric', '1', 0);

/*CREATE TABLE `zaposleni` (
  `Ime` varchar(45) DEFAULT NULL,
  `Prezime` varchar(45) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `JMB` varchar(13) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `Username` varchar(45) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `Password` varchar(45) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `Id` varchar(45) COLLATE utf8mb3_unicode_ci NOT NULL,
  `isAdmin` tinyint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `JMB_UNIQUE` (`JMB`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_unicode_ci*/
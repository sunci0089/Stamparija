
use stamparija_hci;
-- racunaj cijenuSaPDV fakture na osnovu otkupa
delimiter $$

create procedure racunaj_cijenu_fakture_po_otkupu(in potvrda_ varchar(45))
BEGIN
	declare cijenaPDV_ decimal(10,2);
    declare sifra_fakture_ varchar(45);

	set @cijenaPDV_ = 0;
	if (select brojPotvrde from otkup
		where brojPotvrde=potvrda_) is null then
		SIGNAL SQLSTATE '45000' 
			SET MESSAGE_TEXT = 'Ne postoji otkup sa datom sifrom';
	End if;
    
	select sum((cijenaBezMarze+CijenaBezMarze*marza)*o.kolicina) as cijenaPDV 
    into @cijenaPDV_ from otkup_stavka o
    inner join artikal r on roba_sifra= r.sifra
    where otkup_BrojPotvrde = potvrda_
    group by Otkup_BrojPotvrde;
    
    -- dohvati sifru fakture iz otkupa
    SELECT faktura_sifra into @sifra_fakture_
    FROM otkup o
    WHERE o.BrojPotvrde = potvrda_;
    
    update faktura f
    set cijenaSaPDV=@cijenaPDV_
    where f.sifra=@sifra_fakture_;
    
END $$
delimiter ;

-- ----------------------------------------------------------------------------------
-- -- trigeri
-- -----------------------------------------------------------------------
-- da li je roba na stanju u poslovnici

DELIMITER $$

CREATE TRIGGER provjeri_zalihe_prije_prodaje_stavke_otkupa
BEFORE INSERT ON Otkup_stavka
FOR EACH ROW 
BEGIN
	DECLARE kupoprodaja_ VARCHAR(45);

    -- Dohvati vrstu uplate prema sifri otkupa
    SELECT f.vrstaUplate
    INTO @kupoprodaja_
    FROM Faktura f
    inner Join otkup o on o.faktura_sifra = f.sifra
    WHERE o.BrojPotvrde = NEW.Otkup_BrojPotvrde;
    
    If (@kupoprodaja_='PRODAJA') THEN
		IF (select kolicina from artikal r where r.sifra=new.roba_sifra)>=new.kolicina
		THEN
			update artikal
			set kolicina=kolicina-new.kolicina
			where sifra=new.Roba_sifra;
		ELSE SIGNAL SQLSTATE '45000' 
			SET MESSAGE_TEXT = 'Nema dovoljno robe na zalihama';
        END IF;
	END if;
END $$

DELIMITER ;
-- ---------------------------------------------------------------------------------------
-- ako dodamo stavku otkupa mijenja se cijena fakture za otkup
DELIMITER $$

create trigger promjeni_cijenu_nakon_umetanja_stavke_otkupa 
after insert on otkup_stavka
for each row
BEGIN
	call racunaj_cijenu_fakture_po_otkupu(new.otkup_brojPotvrde);
END$$

DELIMITER ;
-- ---------------------------------------------------------------------------------------
-- ako obrisemo stavku otkupa mijenja se cijena fakture za otkup

DELIMITER $$

create trigger promijeni_cijenu_i_kolicinu_nakon_brisanja_stavke_otkupa 
after delete on otkup_stavka
for each row
BEGIN
	DECLARE kupoprodaja_ VARCHAR(45);
	
    call racunaj_cijenu_fakture_po_otkupu(old.otkup_brojPotvrde);

    -- Dohvati vrstu uplate prema sifri otkupa i poslovnicu preko broja otkupa
	SELECT f.vrstaUplate
    INTO @kupoprodaja_
    FROM Faktura f
    inner Join otkup o on o.faktura_sifra = f.sifra
    WHERE o.BrojPotvrde = old.Otkup_BrojPotvrde;
    
    If (@kupoprodaja_='KUPOVINA') THEN
		IF (select kolicina from artikal r where r.sifra=old.roba_sifra)>=old.kolicina
		THEN
			update artikal r
			set r.kolicina=r.kolicina-old.kolicina
			where r.sifra=old.Roba_sifra;
        ELSE SIGNAL SQLSTATE '45000' 
			SET MESSAGE_TEXT = 'Artikal na zalihama <0';
        end if;
	ELSEIF (@kupoprodaja_='prodaja') THEN
			update Artikal r
			set r.kolicina=r.kolicina+old.kolicina
			where r.sifra=old.Roba_sifra;
	ELSE SIGNAL SQLSTATE '45000' 
			SET MESSAGE_TEXT = 'Pogresna vrsta uplate';
	END IF;
END$$

DELIMITER ;
-- ---------------------------------------------------------------------------------------
-- 
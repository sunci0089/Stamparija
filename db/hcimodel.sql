-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema stamparija_hci
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema stamparija_hci
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `stamparija_hci` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ;
USE `stamparija_hci` ;

-- -----------------------------------------------------
-- Table `stamparija_hci`.`Mjesto`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Mjesto` (
  `Naziv` VARCHAR(45) NOT NULL,
  `PostanskiBroj` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`PostanskiBroj`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Saradnik`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Saradnik` (
  `Sifra` VARCHAR(20) NOT NULL,
  `Mjesto_PostanskiBroj` VARCHAR(10) NOT NULL,
  `Naziv` VARCHAR(45) NULL,
  `Ime` VARCHAR(45) NULL,
  `Prezime` VARCHAR(45) NULL,
  `JIB` VARCHAR(12) NULL,
  `JMB` VARCHAR(13) NULL,
  `Vrsta` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Sifra`),
  INDEX `fk_Saradnici_Mjesto1_idx` (`Mjesto_PostanskiBroj` ASC) VISIBLE,
  CONSTRAINT `fk_Saradnici_Mjesto1`
    FOREIGN KEY (`Mjesto_PostanskiBroj`)
    REFERENCES `stamparija_hci`.`Mjesto` (`PostanskiBroj`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Telefon`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Telefon` (
  `BrTel` VARCHAR(15) NOT NULL,
  `Saradnik_Sifra` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`BrTel`, `Saradnik_Sifra`),
  INDEX `fk_Telefon_Saradnici1_idx` (`Saradnik_Sifra` ASC) VISIBLE,
  CONSTRAINT `fk_Telefon_Saradnici1`
    FOREIGN KEY (`Saradnik_Sifra`)
    REFERENCES `stamparija_hci`.`Saradnik` (`Sifra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Ziroracun`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Ziroracun` (
  `BrojRacuna` VARCHAR(16) NOT NULL,
  `Banka` VARCHAR(45) NOT NULL,
  `Saradnici_Sifra` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`BrojRacuna`),
  INDEX `fk_Ziroracun_Saradnici1_idx` (`Saradnici_Sifra` ASC) VISIBLE,
  CONSTRAINT `fk_Ziroracun_Saradnici1`
    FOREIGN KEY (`Saradnici_Sifra`)
    REFERENCES `stamparija_hci`.`Saradnik` (`Sifra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Faktura`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Faktura` (
  `Sifra` VARCHAR(45) NOT NULL,
  `DatumVrijeme` DATETIME(10) NOT NULL,
  `NacinPlacanja` VARCHAR(45) NOT NULL,
  `Ziroracun_Saradnika` VARCHAR(16) NULL,
  `VrstaUplate` VARCHAR(45) NOT NULL,
  `CijenaSaPDV` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`Sifra`),
  INDEX `fk_Isplata_Ziroracun1_idx` (`Ziroracun_Saradnika` ASC) VISIBLE,
  CONSTRAINT `fk_Isplata_Ziroracun1`
    FOREIGN KEY (`Ziroracun_Saradnika`)
    REFERENCES `stamparija_hci`.`Ziroracun` (`BrojRacuna`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Proizvodjac`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Proizvodjac` (
  `Sifra` VARCHAR(45) NOT NULL,
  `Ime` VARCHAR(45) NOT NULL,
  `DrzavaPorijekla` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Sifra`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Otkup`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Otkup` (
  `BrojPotvrde` VARCHAR(45) NOT NULL,
  `Faktura_sifra` VARCHAR(45) NOT NULL,
  `Saradnici_Sifra` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`BrojPotvrde`),
  INDEX `fk_Otkup_Racun1_idx` (`Faktura_sifra` ASC) VISIBLE,
  INDEX `fk_Otkup_Saradnici1_idx` (`Saradnici_Sifra` ASC) VISIBLE,
  CONSTRAINT `fk_Otkup_Racun1`
    FOREIGN KEY (`Faktura_sifra`)
    REFERENCES `stamparija_hci`.`Faktura` (`Sifra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Otkup_Saradnici1`
    FOREIGN KEY (`Saradnici_Sifra`)
    REFERENCES `stamparija_hci`.`Saradnik` (`Sifra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Artikal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Artikal` (
  `Sifra` VARCHAR(20) NOT NULL,
  `Naziv` VARCHAR(45) NOT NULL,
  `Kolicina` DECIMAL(10,2) NOT NULL,
  `CijenaBezMarze` DECIMAL(10,2) NOT NULL,
  `Kategorija` VARCHAR(45) NOT NULL,
  `Marza` DECIMAL(10,2) NOT NULL,
  `Proizvodjac_Sifra` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Sifra`),
  INDEX `fk_Artikal_Proizvodjac1_idx` (`Proizvodjac_Sifra` ASC) VISIBLE,
  CONSTRAINT `fk_Artikal_Proizvodjac1`
    FOREIGN KEY (`Proizvodjac_Sifra`)
    REFERENCES `stamparija_hci`.`Proizvodjac` (`Sifra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Otkup_stavka`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Otkup_stavka` (
  `Otkup_BrojPotvrde` VARCHAR(45) NOT NULL,
  `Kolicina` DECIMAL(10,2) NOT NULL,
  `Roba_Sifra` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`Otkup_BrojPotvrde`, `Roba_Sifra`),
  INDEX `fk_OtkupStavka_Otkup1_idx` (`Otkup_BrojPotvrde` ASC) VISIBLE,
  INDEX `fk_Otkup_stavka_Roba1_idx` (`Roba_Sifra` ASC) VISIBLE,
  CONSTRAINT `fk_OtkupStavka_Otkup1`
    FOREIGN KEY (`Otkup_BrojPotvrde`)
    REFERENCES `stamparija_hci`.`Otkup` (`BrojPotvrde`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Otkup_stavka_Roba1`
    FOREIGN KEY (`Roba_Sifra`)
    REFERENCES `stamparija_hci`.`Artikal` (`Sifra`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `stamparija_hci`.`Zaposleni`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stamparija_hci`.`Zaposleni` (
  `Ime` VARCHAR(45) NULL,
  `Prezime` VARCHAR(45) NULL,
  `JMB` VARCHAR(13) NULL,
  `Username` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(128) NOT NULL,
  `Id` INT NOT NULL AUTO_INCREMENT,
  `isAdmin` TINYINT NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `JMB_UNIQUE` (`JMB` ASC) VISIBLE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

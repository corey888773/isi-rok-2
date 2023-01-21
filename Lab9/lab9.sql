DROP SCHEMA kwiaciarnia CASCADE;
CREATE SCHEMA kwiaciarnia;

CREATE TABLE kwiaciarnia.klienci(
    idklienta VARCHAR(10),
    haslo VARCHAR(10) CHECK(LENGTH(haslo) >= 4) NOT NULL,
    nazwa VARCHAR(40) NOT NULL,
    miasto VARCHAR(40) NOT NULL,
    kod CHAR(6) NOT NULL,
    adres VARCHAR(40) NOT NULL,
    email VARCHAR(40),
    telefon VARCHAR(16) NOT NULL,
    fax VARCHAR(16),
    nip CHAR(13),
    region CHAR(9),
    PRIMARY KEY (idklienta)
);

CREATE TABLE kwiaciarnia.kompozycje(
    idkompozycji CHAR(5),
    nazwa VARCHAR(40) NOT NULL,
    opis VARCHAR(100),
    cena NUMERIC(10, 2) CHECK(cena >= 40),
    minimum INT,
    stan INT,
    PRIMARY KEY (idkompozycji)
);

CREATE TABLE kwiaciarnia.odbiorcy(
  idodbiorcy SERIAL,
  nazwa VARCHAR(40) NOT NULL,
  miasto VARCHAR(40) NOT NULL,
  kod CHAR(6) NOT NULL,
  adres VARCHAR(40) NOT NULL,
  PRIMARY KEY (idodbiorcy)
);

CREATE TABLE kwiaciarnia.zamowienia(
  idzamowienia INT,
  idklienta VARCHAR(10) NOT NULL,
  idodbiorcy INT NOT NULL,
  idkompozycji CHAR(5) NOT NULL,
  termin DATE NOT NULL,
  cena NUMERIC(10,2),
  zaplacone BOOLEAN,
  uwagi VARCHAR(200),
  PRIMARY KEY (idzamowienia)
);

CREATE TABLE kwiaciarnia.historia(
    idzamowienia INT,
    idklienta VARCHAR(10),
    idkompozycji CHAR(5),
    cena NUMERIC(10,2),
    termin DATE,
    PRIMARY KEY (idzamowienia)
);

CREATE TABLE kwiaciarnia.zapotrzebowanie(
  idkompozycji CHAR(5),
  data DATE,
  PRIMARY KEY (idkompozycji)
);

ALTER TABLE ONLY kwiaciarnia.zamowienia ADD CONSTRAINT fkey_klenci FOREIGN KEY (idklienta) REFERENCES kwiaciarnia.klienci;
ALTER TABLE ONLY kwiaciarnia.zamowienia ADD CONSTRAINT fkey_odbiorcy FOREIGN KEY (idodbiorcy) REFERENCES kwiaciarnia.odbiorcy;
ALTER TABLE ONLY kwiaciarnia.zamowienia ADD CONSTRAINT fkey_kompozycje FOREIGN KEY (idkompozycji) REFERENCES kwiaciarnia.kompozycje;

ALTER TABLE ONLY kwiaciarnia.zapotrzebowanie ADD CONSTRAINT fkey_kompozycje FOREIGN KEY (idkompozycji) REFERENCES kwiaciarnia.kompozycje;

select * from kwiaciarnia.kompozycje;
update kwiaciarnia.klienci set idklienta = 'msowins' where nazwa = 'Magdalena Sowinska';

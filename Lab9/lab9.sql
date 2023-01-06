-- create schema kwiaciarnia;

create table kwiaciarnia.klienci(
    idklienta varchar(10) NOT NULL,
    haslo varchar(10) NOT NULL,
    nazwa varchar(40) NOT NULL,
    miasto varchar(40) NOT NULL,
    kod char(6) NOT NULL,
    adres varchar(40) NOT NULL,
    email varchar(40),
    telefon varchar(16) NOT NULL,
    fax varchar(16),
    nip char(13),
    regon char(9),
    CONSTRAINT haslo_min CHECK (length((haslo)::text) >= 4)
);

create table kwiaciarnia.kompozycje(
    idkompozycji char(5) NOT NULL,
    nazwa varchar(40) NOT NULL,
    opis varchar(100),
    cena numeric(10,2),
    minimum int,
    stan int,
    CONSTRAINT min_cena CHECK( cena >= 40.00)
);

create table kwiaciarnia.odbiorcy(
    idodbiorcy serial NOT NULL,
    nazwa varchar(40) NOT NULL,
    miasto varchar(40) NOT NULL,
    kod char(6) NOT NULL,
    adres varchar(40) NOT NULL
);

create table kwiaciarnia.zamowienia(
    idzamowienia int NOT NULL,
    idklienta varchar(10) NOT NULL,
    idodbiorcy int NOT NULL,
    idkompozycji char(5) NOT NULL,
    termin date NOT NULL,
    cena numeric(10,2) NOT NULL,
    zaplacone boolean,
    uwagi varchar(200)
);

create table kwiaciarnia.historia(
    idzamowienia int NOT NULL,
    idklienta varchar(10),
    idkompozycji char(5),
    cena numeric(10,2),
    termin date
);

create table kwiaciarnia.zapotrzebowanie(
    idkompozycji char(5),
    data date
);

ALTER TABLE ONLY kwiaciarnia.klienci
    ADD CONSTRAINT klienci_pkey PRIMARY KEY (idklienta);

ALTER TABLE ONLY kwiaciarnia.kompozycje
    ADD CONSTRAINT kompozycje_pkey PRIMARY KEY (idkompozycji);

ALTER TABLE ONLY kwiaciarnia.odbiorcy
    ADD CONSTRAINT odbiorcy_pkey PRIMARY KEY (idodbiorcy);

ALTER TABLE ONLY kwiaciarnia.zamowienia
    ADD CONSTRAINT zamowienia_pkey PRIMARY KEY (idzamowienia);

ALTER TABLE ONLY kwiaciarnia.zapotrzebowanie
    ADD CONSTRAINT zapotrzebowanie_pkey PRIMARY KEY (idkompozycji);

ALTER TABLE ONLY kwiaciarnia.zamowienia
    ADD CONSTRAINT zamowienia_fkey_klienci FOREIGN KEY (idklienta) REFERENCES kwiaciarnia.klienci(idklienta);
ALTER TABLE ONLY kwiaciarnia.zamowienia
    ADD CONSTRAINT zamowienia_fkey_odbiorcy FOREIGN KEY (idkompozycji) REFERENCES kwiaciarnia.kompozycje;
ALTER TABLE ONLY kwiaciarnia.zamowienia
    ADD CONSTRAINT zamowienia_fkey_kompozycje FOREIGN KEY (idodbiorcy) REFERENCES kwiaciarnia.odbiorcy(idodbiorcy);

ALTER TABLE ONLY kwiaciarnia.zapotrzebowanie
    ADD CONSTRAINT zapotrzebowanie_fkey_kompozycje FOREIGN KEY (idkompozycji) REFERENCES kwiaciarnia.kompozycje(idkompozycji);

select * from kwiaciarnia.kompozycje;
update kwiaciarnia.klienci set idklienta = 'msowins' where nazwa = 'Magdalena Sowinska';

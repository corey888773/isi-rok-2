-- 1
insert into czekoladki
values ('W98', 'Biały kieł', 'biała',
        'laskowe', 'marcepan', 'Rozpływające się w rękach i kieszeniach', 0.45, 0.20);

insert into klienci values
                        (90,'Matusiak Edward','Kropiwnickiego 6/3', 'Leningrad', '31-471', '031 423 45 38'),
                        (91,'Matusiak Alina','Kropiwnickiego 6/3', 'Leningrad', '31-471', '031 423 45 38'),
                        (92,'Kimono Franek','Karateków 8', 'Mistrz', '30-029', '501 498 324');

insert into klienci values (93,
                            (select nazwa from klienci where idklienta = 90),
                            (select ulica from klienci where idklienta = 90),
                            (select miejscowosc from klienci where idklienta = 90),
                            (select kod from klienci where idklienta = 90),
                            (select telefon from klienci where idklienta = 90));

select * from klienci where idklienta >= 90;

-- 2
insert into czekoladki values ('X91', 'Nieznana Nieznajoma', null, null, null,'Niewidzialna czekoladka wspomagajaca odchudzanie.', 0.26, 0),
                              ('M98', 'Nieznana Nieznajoma', 'mleczna', null, null,'Aksamitna mleczna czekolada w ksztalcie butelki z mlekiem.', 0.26, 0);
select * from czekoladki where nazwa = 'Nieznana Nieznajoma';

-- 3
delete from czekoladki where nazwa = 'Nieznana Nieznajoma';
insert into czekoladki (idczekoladki, nazwa, czekolada, opis, koszt, masa ) values
                                ('X91', 'Nieznana Nieznajoma', null, 'Niewidzialna czekoladka wspomagajaca odchudzanie.', 0.26, 0),
                              ('M98', 'Nieznana Nieznajoma', 'mleczna','Aksamitna mleczna czekolada w ksztalcie butelki z mlekiem.', 0.26, 0);

-- 4
UPDATE klienci set nazwa = 'Czarnkowska Natalia' where nazwa = 'Czarnkowska Dalia';
select * from klienci;

UPDATE czekoladki set koszt = koszt * 1.1 where idczekoladki in ('W98', 'M98', 'X91');

UPDATE czekoladki set koszt = (select koszt from czekoladki where idczekoladki = 'W98' ) where nazwa = 'Nieznana Nieznajoma';
UPDATE czekoladki cz1 SET koszt = cz2.koszt
FROM czekoladki cz2
WHERE cz1.nazwa = 'Nieznana Nieznajoma' AND cz2.idczekoladki = 'W98';

UPDATE klienci set miejscowosc = 'Piotrogrod' where miejscowosc = 'Leningrad';

UPDATE czekoladki set koszt = koszt + 0.15 where substr(idczekoladki, 2, 2)::int = 10;
-- substr (stirng, od którego znaku, ile znaków)

-- 5
DELETE from klienci where nazwa LIKE '%Matusiak%';
DELETE from klienci where idklienta > 91;
DELETE from czekoladki where koszt > 0.45 or masa >= 36 or masa = 0;

-- 6

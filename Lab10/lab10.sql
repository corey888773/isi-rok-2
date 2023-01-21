set search_path to public;

--1
SELECT DISTINCT nazwa
FROM pudelka NATURAL JOIN zawartosc
WHERE idczekoladki
  IN (SELECT idczekoladki FROM czekoladki ORDER BY koszt LIMIT 3);

SELECT p.nazwa, idpudelka
FROM (SELECT idczekoladki FROM czekoladki ORDER BY koszt LIMIT 3)
  AS ulubioneczekoladki
NATURAL JOIN zawartosc
NATURAL JOIN pudelka p;

SELECT nazwa
FROM czekoladki
WHERE koszt = (SELECT MAX(koszt) FROM czekoladki);

SELECT nazwa, koszt, (SELECT MAX(koszt) FROM czekoladki) AS MAX FROM czekoladki;

 --2
 SELECT * FROM zamowienia WHERE idklienta IN
                                (SELECT idklienta FROM klienci WHERE nazwa LIKE '%ntoni%');

SELECT * FROM zamowienia WHERE idklienta IN (SELECT idklienta FROM klienci WHERE klienci.ulica LIKE '%/%');

-- 3
SELECT * FROM klienci WHERE idklienta IN (SELECT idklienta FROM zamowienia WHERE datarealizacji ='12.11.2013'::date);

SELECT * FROM klienci WHERE idklienta IN
    (SELECT idklienta FROM zamowienia WHERE idzamowienia IN
            (SELECT idzamowienia FROM artykuly WHERE idpudelka IN
                    (SELECT idpudelka FROM pudelka WHERE nazwa IN ('Kremowa fantazja','Kolekcja jesienna'))));

SELECT * FROM klienci WHERE idklienta IN
    (SELECT idklienta FROM zamowienia WHERE idzamowienia IN
            (SELECT idzamowienia FROM artykuly WHERE sztuk >= 2 AND idpudelka IN
                    (SELECT idpudelka FROM pudelka WHERE nazwa IN ('Kremowa fantazja','Kolekcja jesienna'))));
select * from klienci where idklienta in
    (select idklienta from zamowienia za join artykuly a on za.idzamowienia = a.idzamowienia join pudelka p on a.idpudelka = p.idpudelka
        where p.nazwa in ('Kremowa fantazja' , 'Kolekcja jesienna') and a.sztuk >= 2);

SELECT * FROM klienci WHERE idklienta NOT IN (SELECT idklienta FROM zamowienia);

-- 4
SELECT * FROM pudelka WHERE idpudelka IN
  (SELECT idpudelka FROM zawartosc WHERE idczekoladki NOT IN
     (SELECT idczekoladki FROM czekoladki WHERE orzechy IS NOT NULL));

SELECT * FROM pudelka WHERE idpudelka IN
  (SELECT idpudelka FROM zawartosc z WHERE EXISTS
     (SELECT idczekoladki FROM czekoladki c WHERE nadzienie IS NULL AND z.idczekoladki = c.idczekoladki));

--5
-- Kto (nazwa klienta) złożył zamówienia na takie same czekoladki (pudełka) jak zamawiała Gorka Alicja.
SELECT * FROM klienci WHERE idklienta IN
    (SELECT idklienta FROM zamowienia WHERE idzamowienia IN
        (SELECT idzamowienia FROM artykuly WHERE idpudelka IN
            (SELECT idpudelka FROM artykuly WHERE idzamowienia IN
                (SELECT idzamowienia FROM zamowienia WHERE idklienta =
                   (SELECT idklienta FROM klienci WHERE nazwa = 'Górka Alicja')))));

SELECT * FROM klienci WHERE idklienta IN
    (SELECT idklienta FROM zamowienia WHERE idzamowienia IN
        (SELECT idzamowienia FROM artykuly WHERE idpudelka IN
            (SELECT idpudelka FROM artykuly WHERE idzamowienia IN
                (SELECT idzamowienia FROM zamowienia WHERE idklienta IN
                   (SELECT idklienta FROM klienci WHERE miejscowosc ~* 'katowice')))));

-- 6
SELECT nazwa FROM pudelka WHERE idpudelka IN
    (SELECT idpudelka FROM
        (SELECT idpudelka, SUM(sztuk) as sztuk FROM zawartosc GROUP BY idpudelka) as id_sztuki
            WHERE id_sztuki.sztuk = (SELECT MAX(x) FROM (SELECT SUM(sztuk) as x FROM zawartosc GROUP BY idpudelka) AS sum));

SELECT nazwa FROM pudelka WHERE idpudelka IN
    (SELECT idpudelka FROM
        (SELECT idpudelka, SUM(sztuk) as sztuk FROM zawartosc GROUP BY idpudelka) as id_sztuki
            WHERE id_sztuki.sztuk > (SELECT AVG(x) FROM (SELECT SUM(sztuk) as x FROM zawartosc GROUP BY idpudelka) AS sum));

SELECT nazwa FROM pudelka WHERE idpudelka IN
    (SELECT idpudelka FROM
        (SELECT idpudelka, SUM(sztuk) as sztuk FROM zawartosc GROUP BY idpudelka) as id_sztuki
            WHERE id_sztuki.sztuk = (SELECT MAX(x) FROM (SELECT SUM(sztuk) as x FROM zawartosc GROUP BY idpudelka) AS sum)
               OR id_sztuki.sztuk = (SELECT MIN(x) FROM (SELECT SUM(sztuk) as x FROM zawartosc GROUP BY idpudelka) AS sum));
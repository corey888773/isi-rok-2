--1
SELECT k.nazwa FROM klienci k;
SELECT k.nazwa, z.idzamowienia FROM klienci k, zamowienia z order by z.idzamowienia asc;
SELECT k.nazwa, z.idzamowienia FROM klienci k, zamowienia z WHERE z.idklienta = k.idklienta;
SELECT k.nazwa, z.idzamowienia FROM klienci k NATURAL JOIN zamowienia z;
SELECT k.nazwa, z.idzamowienia FROM klienci k JOIN zamowienia z ON z.idklienta=k.idklienta;
SELECT k.nazwa, z.idzamowienia FROM klienci k JOIN zamowienia z USING (idklienta);

-- 1.1, 1.2 w zapytaniu 3

--2
select z.idzamowienia, z.datarealizacji from zamowienia z JOIN klienci k on z.idklienta = k.idklienta AND k.nazwa LIKE '%Antoni%';
select z.idzamowienia, z.datarealizacji, k.ulica from zamowienia z JOIN klienci k on z.idklienta = k.idklienta AND k.ulica LIKE '%/%';
select z.idzamowienia, z.datarealizacji from zamowienia z JOIN klienci k on z.idklienta = k.idklienta AND k.miejscowosc = 'Kraków' AND extract(month from datarealizacji) = 11;

--3
select z.idzamowienia, z.datarealizacji from zamowienia z JOIN klienci k on z.idklienta = k.idklienta AND z.datarealizacji > CURRENT_DATE - INTERVAL '5 YEARS';
select CURRENT_DATE - INTERVAL '10 MONTHS, 3 DAYS', CURRENT_DATE - INTERVAL '5 YEARS';

select distinct k.nazwa, k.ulica, k.miejscowosc from klienci k
    JOIN zamowienia z on k.idklienta = z.idklienta
    JOIN artykuly a on z.idzamowienia = a.idzamowienia
    JOIN pudelka p on a.idpudelka = p.idpudelka
                  WHERE p.nazwa in ('Kolekcja jesienna', 'Kremowa fantazja');

SELECT distinct k.nazwa, k.ulica, k.miejscowosc
FROM
    klienci k
WHERE
    k.idklienta IN (
        SELECT k2.idklienta
        FROM
            klienci k2
            INNER JOIN zamowienia z ON k2.idklienta = z.idklienta
            INNER JOIN artykuly a ON z.idzamowienia = a.idzamowienia
            INNER JOIN pudelka p ON a.idpudelka = p.idpudelka
        WHERE p.nazwa IN ('Kremowa fantazja', 'Kolekcja jesienna')
    );

select distinct k.nazwa, k.ulica, z.idklienta from zamowienia z RIGHT JOIN klienci k on z.idklienta = k.idklienta WHERE z.idklienta IS NOT NULL;
select distinct k.nazwa, k.ulica, z.idklienta from zamowienia z RIGHT JOIN klienci k on z.idklienta = k.idklienta WHERE z.idklienta IS NULL;
select distinct  * from klienci k left join zamowienia z on k.idklienta = z.idklienta where z.idzamowienia is null;

select distinct k.nazwa, k.ulica, z.idklienta from zamowienia z INNER JOIN klienci k on z.idklienta = k.idklienta;
select distinct k.nazwa, k.ulica, z.idklienta from zamowienia z FULL OUTER JOIN klienci k on z.idklienta = k.idklienta WHERE z.idklienta IS NOT NULL;

select distinct k.nazwa, k.ulica, z.datarealizacji from zamowienia z INNER JOIN klienci k on z.idklienta = k.idklienta WHERE EXTRACT(month from z.datarealizacji) = 11;

select distinct k.nazwa, k.ulica, p.nazwa, a.sztuk from klienci k
    JOIN zamowienia z on k.idklienta = z.idklienta
    JOIN artykuly a on z.idzamowienia = a.idzamowienia
    JOIN pudelka p on a.idpudelka = p.idpudelka
                  WHERE p.nazwa in ('Kolekcja jesienna', 'Kremowa fantazja') AND a.sztuk >= 2;

select distinct k.nazwa, k.ulica, c.orzechy from klienci k
    JOIN zamowienia z on k.idklienta = z.idklienta
    JOIN artykuly a on z.idzamowienia = a.idzamowienia
    JOIN pudelka p on a.idpudelka = p.idpudelka
    JOIN zawartosc z2 on p.idpudelka = z2.idpudelka
    JOIN czekoladki c on z2.idczekoladki = c.idczekoladki
    WHERE c.orzechy IS NOT NULL ;

-- 4
select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki;

select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where p.idpudelka = 'heav';

select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where p.nazwa like '%olekcja%';

-- 5
select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.idczekoladki = 'd09';
select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.nazwa like 'S%' and z.sztuk >= 1;
select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where z.sztuk >= 4;
select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.nadzienie = 'truskawki';
select * from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.czekolada != 'gorzka';
select * from pudelka p left join(select z.idpudelka from zawartosc z inner join czekoladki c on z.idczekoladki = c.idczekoladki where c.orzechy is not null) j on p.idpudelka = j.idpudelka where j.idpudelka is null;

--6
select idczekoladki, nazwa from czekoladki where koszt > (select koszt from czekoladki where idczekoladki ='d08');

WITH GAlicja as (
        select a.idpudelka from artykuly a
        join zamowienia z on z.idzamowienia = a.idzamowienia
        join klienci k on z.idklienta = k.idklienta and k.nazwa = 'Górka Alicja')
SELECT distinct kk.nazwa
FROM
    klienci kk
    INNER JOIN zamowienia zz ON kk.idklienta = zz.idklienta
    INNER JOIN artykuly aa ON zz.idzamowienia = aa.idzamowienia
    INNER JOIN GAlicja ON aa.idpudelka = GAlicja.idpudelka WHERE kk.nazwa != 'Górka Alicja';

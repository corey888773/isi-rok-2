-- 1

SELECT DISTINCT nazwa
FROM pudelka NATURAL JOIN zawartosc
WHERE idczekoladki
  IN (SELECT idczekoladki FROM czekoladki ORDER BY koszt LIMIT 3);

SELECT nazwa, koszt
FROM czekoladki
WHERE koszt = (SELECT MAX(koszt) FROM czekoladki);

SELECT p.nazwa, idpudelka
FROM (SELECT idczekoladki FROM czekoladki ORDER BY koszt LIMIT 3)
  AS ulubioneczekoladki
NATURAL JOIN zawartosc
NATURAL JOIN pudelka p;

SELECT nazwa, koszt, (SELECT MAX(koszt) FROM czekoladki) AS MAX FROM czekoladki;

-- 2
select datarealizacji, idzamowienia from zamowienia where idklienta in (select idklienta from klienci where nazwa LIKE '%Antoni%');
select datarealizacji, idzamowienia from zamowienia where idklienta in (select idklienta from klienci where ulica LIKE '%/%');
select datarealizacji, idzamowienia from zamowienia where idklienta in (select idklienta from klienci where miejscowosc LIKE '_rak_w') and extract(month from datarealizacji) = 11;

-- 3
select nazwa, ulica, miejscowosc from klienci where idklienta in (select idklienta from zamowienia where datarealizacji = '2013-11-12');
select nazwa, ulica, miejscowosc from klienci where idklienta in (select idklienta from zamowienia where extract(month from datarealizacji) = 11 and extract(year from datarealizacji) = 2013);
select nazwa, ulica, miejscowosc from klienci where idklienta in
                                                    (select idklienta from zamowienia za join artykuly a on za.idzamowienia = a.idzamowienia join pudelka p on a.idpudelka = p.idpudelka
                                                                      where p.nazwa in ('Kremowa fantazja' , 'Kolekcja jesienna'));
select nazwa, ulica, miejscowosc from klienci where idklienta in
                                                    (select idklienta from zamowienia za join artykuly a on za.idzamowienia = a.idzamowienia join pudelka p on a.idpudelka = p.idpudelka
                                                                      where p.nazwa in ('Kremowa fantazja' , 'Kolekcja jesienna') and a.sztuk >= 2);

select nazwa, ulica, miejscowosc from klienci where idklienta in
                                                    (select idklienta from zamowienia za join artykuly a on za.idzamowienia = a.idzamowienia join pudelka p on a.idpudelka = p.idpudelka join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki
                                                                      where c.orzechy = 'migdały');

select nazwa, ulica, miejscowosc from klienci where idklienta in ( select idklienta from zamowienia);
select nazwa, ulica, miejscowosc from klienci where idklienta not in ( select idklienta from zamowienia);

-- 4
select nazwa, opis, cena from pudelka where idpudelka in (select p.idpudelka from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.idczekoladki = 'd09');
select nazwa, opis, cena from pudelka where idpudelka in (select p.idpudelka from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.nazwa = 'Gorzka truskawkowa');
select nazwa, opis, cena from pudelka where idpudelka in (select p.idpudelka from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.nazwa like 'S%' and z.sztuk >= 1);
select nazwa, opis, cena from pudelka where idpudelka in (select p.idpudelka from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where z.sztuk >= 4);
--4.5
--4.6
select nazwa, opis, cena from pudelka where idpudelka not in (select p.idpudelka from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.czekolada = 'gorzka');
--4.8
select nazwa, opis, cena from pudelka where idpudelka in (select p.idpudelka from pudelka p join zawartosc z on p.idpudelka = z.idpudelka join czekoladki c on z.idczekoladki = c.idczekoladki where c.nadzienie is null);

-- 5



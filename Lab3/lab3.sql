-- 1
select idzamowienia, datarealizacji from zamowienia
    where datarealizacji < '20-11-2013' and datarealizacji > '12-11-2013';

select idzamowienia, datarealizacji from zamowienia
where datarealizacji between '1-12-2013' and '6-12-2013'
   or datarealizacji between '15-12-2013' and '20-12-2013';

select idzamowienia, datarealizacji from zamowienia
    where datarealizacji::text like '%-12-2013';

select idzamowienia, datarealizacji from zamowienia
    where extract(month from datarealizacji) = 11 and extract(year from datarealizacji) = 2013;

select idzamowienia, datarealizacji from zamowienia
    where date_part('month', datarealizacji) in(11,12) and extract(year from datarealizacji) = 2013;

-- 2
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa like 'S%';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa like 'S%i';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa ~ '^S.*i$';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa like 'S% m%';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa like 'C%' or nazwa like 'A%' or nazwa like 'B%';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa ~ '^[A-C].*';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa like '%rzech%';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa ~ '.*[O,o]rzech.*';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa ~ '.*(O|o)rzech.*';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa similar to '(A|B|C)%';

select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki
where nazwa ~ '^(A|B|C)';

-- 3
select miejscowosc from klienci where miejscowosc like '% %' or miejscowosc like '%-%';
select miejscowosc from klienci;


select * from klienci where length(telefon) > 11;

select * from klienci where telefon not like '0%';

-- 4
(select idczekoladki, nazwa, masa, koszt from czekoladki where masa between 15 and 24)
union
(select idczekoladki, nazwa, masa, koszt from czekoladki where koszt*100 between 15 and 24);

(select idczekoladki, nazwa, masa, koszt from czekoladki where masa between 25 and 35)
except
(select idczekoladki, nazwa, masa, koszt from czekoladki where koszt*100 between 25 and 35);

((select idczekoladki, nazwa, masa, koszt from czekoladki where masa between 15 and 24)
intersect
(select idczekoladki, nazwa, masa, koszt from czekoladki where koszt*100 between 15 and 24))
union
((select idczekoladki, nazwa, masa, koszt from czekoladki where masa between 25 and 35)
intersect
(select idczekoladki, nazwa, masa, koszt from czekoladki where koszt*100 between 25 and 35));

-- 5
(select idklienta from klienci)
except
(select idklienta from zamowienia);

(select idpudelka from pudelka)
except
(select idpudelka from artykuly);

-- 6
set search_path to siatkowka;

select * from siatkowka.mecze;
select * from siatkowka.statystyki;

select idmeczu,
       gospodarze[1]+gospodarze[2]+gospodarze[3]+coalesce(gospodarze[4], 0)+coalesce(gospodarze[5], 0),
       goscie[1]+goscie[2]+goscie[3]+coalesce(goscie[4], 0)+coalesce(goscie[5], 0)
from siatkowka.statystyki group by idmeczu order by idmeczu;

select idmeczu, (SELECT SUM(s) FROM UNNEST(gospodarze) s), (SELECT SUM(s) FROM UNNEST(goscie) s) from siatkowka.statystyki;

select idmeczu,
       gospodarze[1]+gospodarze[2]+gospodarze[3]+coalesce(gospodarze[4], 0)+coalesce(gospodarze[5], 0),
       goscie[1]+goscie[2]+goscie[3]+coalesce(goscie[4], 0)+coalesce(goscie[5], 0)
from siatkowka.statystyki where array_length(gospodarze , 1) = 5 and (gospodarze[5] > 15 or goscie[5] > 15) group by idmeczu;

select idmeczu, concat(
    case when gospodarze[1] > goscie[1] then 1 else 0 end
    +case when gospodarze[2] > goscie[2] then 1 else 0 end
    +case when gospodarze[3] > goscie[3] then 1 else 0 end
    +case when gospodarze[4] > goscie[4] then 1 else 0 end
    +case when gospodarze[5] > goscie[5] then 1 else 0 end
    ,':',
    case when gospodarze[1] < goscie[1] then 1 else 0 end
    +case when gospodarze[2] < goscie[2] then 1 else 0 end
    +case when gospodarze[3] < goscie[3] then 1 else 0 end
    +case when gospodarze[4] < goscie[4] then 1 else 0 end
    +case when gospodarze[5] < goscie[5] then 1 else 0 end
    ) from siatkowka.statystyki;

-- 7
set search_path to siatkowka;
\o wynikilab3.html
\H
\i zapytanielab3.sql

--8
\o wynikilab31.txt
\t
\a
\pset fieldsep ','
\i zapytanielab31.sql

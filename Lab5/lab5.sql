select count(*) from czekoladki;

select count(*) from czekoladki where nadzienie is not null ;

select count(nadzienie) from czekoladki;

select p.idpudelka, p.nazwa, sum(z.sztuk) as sm from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    group by p.nazwa, p.idpudelka order by sm desc limit 1;

select p.idpudelka, p.nazwa, sum(z.sztuk) as sum from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    group by p.nazwa, p.idpudelka;

select p.idpudelka, p.nazwa, sum(z.sztuk) as sum from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    where c.orzechy is not null
    group by p.nazwa, p.idpudelka;

-- 2
select p.idpudelka, sum(z.sztuk * c.masa) as masa from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    group by p.idpudelka
    order by masa desc;

select p.idpudelka, sum(z.sztuk * c.masa) as masa from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    group by p.idpudelka
    order by masa desc limit 1;

select sum(z.sztuk * c.masa) as masa_calkowita, avg(masa) as srednia from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki;

select p.idpudelka, sum(z.sztuk * c.masa) as masa, avg(masa) from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    group by p.idpudelka
    order by masa desc;

-- 3
select datarealizacji, count(*) from zamowienia group by datarealizacji order by datarealizacji;

select count(*) from zamowienia;

select sum(a.sztuk * p.cena) from zamowienia z join artykuly a on z.idzamowienia = a.idzamowienia
    join pudelka p on a.idpudelka = p.idpudelka;

select k.nazwa, count(z.idklienta), sum(a.sztuk * p.cena) from klienci k join zamowienia z on k.idklienta = z.idklienta
    join artykuly a on z.idzamowienia = a.idzamowienia
    join pudelka p on a.idpudelka = p.idpudelka
    group by k.nazwa;

-- 4
select c.idczekoladki, count(p.idpudelka) as suma from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    group by c.idczekoladki
    order by suma desc limit 1;

select p.idpudelka, sum(z.sztuk) from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    where c.orzechy is null group by p.idpudelka
    order by 2 desc limit 1;

select c.idczekoladki, count(p.idpudelka) as suma from pudelka p
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    group by c.idczekoladki
    order by suma asc limit 1;

select p.idpudelka, sum(a.sztuk) as suma from artykuly a
    join zamowienia z on a.idzamowienia = z.idzamowienia
    join pudelka p on a.idpudelka = p.idpudelka
    group by p.idpudelka order by suma desc limit 1;

-- 5
select count(*) ,extract(quarter from datarealizacji) as quarter from zamowienia
    group by quarter;

select count(*) ,extract(month from datarealizacji) as month from zamowienia
    group by month order by month;

select count(*) ,extract(week from datarealizacji) as week from zamowienia
    group by week order by week;

select count(*), k.miejscowosc from zamowienia z
    join klienci k on z.idklienta = k.idklienta group by k.miejscowosc;

-- 6
select sum(z.sztuk * c.masa * a.sztuk) as masa from pudelka p
    join artykuly a on p.idpudelka = a.idpudelka
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    order by masa desc;

select p.idpudelka, sum(z.sztuk * c.masa) as masa from pudelka p
    join artykuly a on p.idpudelka = a.idpudelka
    join zawartosc z on p.idpudelka = z.idpudelka
    join czekoladki c on z.idczekoladki = c.idczekoladki
    group by p.idpudelka
    order by masa desc;

select z.idpudelka, z.sztuk, c.* from zawartosc z join czekoladki c on z.idczekoladki = c.idczekoladki
            group by idpudelka, c.idczekoladki, z.sztuk order by z.idpudelka;

-- 7
select p.idpudelka, p.cena - sum(c.koszt*z.sztuk) as zysk from czekoladki c
    join zawartosc z on c.idczekoladki = z.idczekoladki
    join pudelka p on z.idpudelka = p.idpudelka
    group by p.idpudelka;

select p.idpudelka, sum(a.sztuk) * p.cena - sum(c.koszt*z.sztuk)  from czekoladki c
    join zawartosc z on c.idczekoladki = z.idczekoladki
    join pudelka p on z.idpudelka = p.idpudelka
    join artykuly a on p.idpudelka = a.idpudelka
    group by p.idpudelka;
with ceny as
    (select sum(a.sztuk) * p.cena - sum(c.koszt*z.sztuk) as zyski from czekoladki c
        join zawartosc z on c.idczekoladki = z.idczekoladki
        join pudelka p on z.idpudelka = p.idpudelka
        join artykuly a on p.idpudelka = a.idpudelka
        group by p.idpudelka)
select sum(ceny.zyski) from ceny;

-- 8
Create sequence indexNumber start 1;
select nextval('indexNumber') as index, p.idpudelka from pudelka p order by p.idpudelka;
drop sequence indexNumber;




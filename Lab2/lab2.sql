-- 2.1
select * from klienci order by nazwa;
select * from klienci order by miejscowosc desc, nazwa asc;
select * from klienci where miejscowosc in( 'Warszawa', 'Kraków') order by miejscowosc desc, nazwa asc;
select * from klienci where miejscowosc = 'Warszawa' or miejscowosc = 'Kraków' order by miejscowosc desc, nazwa asc;

-- 2.2
select * from czekoladki;
select nazwa, masa from czekoladki where masa > 20;
select nazwa, masa, koszt as koszt_w_zł from czekoladki where masa > 20 and koszt > 0.25;
select nazwa, masa, koszt*100 as koszt_w_gr from czekoladki where masa > 20 and koszt > 0.25;
select nazwa, czekolada, nadzienie, orzechy from czekoladki
                                            where (czekolada = 'mleczna' and nadzienie in ('maliny', 'truskawki'))
                                            or (orzechy = 'laskowe' and czekolada != 'gorzka');
-- 2.3
select 124 * 7 + 45;
select 2^30;
select sqrt(3);
select pi();
-- 2.4
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where 15<masa and masa<24;
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where koszt between 0.25 and 0.35;
-- 2.5
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where orzechy is not null;
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where orzechy is null;
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where orzechy is not null or nadzienie is not null;
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where czekolada in ('mleczna', 'biała') and orzechy is null;
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki where czekolada not in ('mleczna', 'biała') and orzechy is not null or nadzienie is not null;

--2.6


-- 2.8
select idczekoladki, nazwa, czekolada, orzechy, nadzienie from czekoladki;
\H
\o zapytanie1.html
\i zapytanie1.sql
-- 1
DROP FUNCTION masaPudelka(arg1 CHAR(4));
CREATE FUNCTION masaPudelka(in arg1 CHAR(4))
RETURNS INT as
$$
DECLARE
    result INT;
BEGIN
SELECT SUM(z.sztuk*c.masa) INTO result FROM zawartosc z
    JOIN czekoladki c USING(idczekoladki) WHERE z.idpudelka = arg1
    GROUP BY z.idpudelka;

    RETURN result;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION liczbaCzekoladek(in arg1 CHAR(4))
RETURNS INT AS
$$
DECLARE
    wynik INT;
    BEGIN
    SELECT SUM(sztuk) INTO wynik FROM zawartosc
        WHERE idpudelka = arg1 GROUP BY idpudelka;

    RETURN wynik;
END
$$ LANGUAGE plpgsql;


SELECT masaPudelka('alls');
SELECT liczbaCzekoladek('alls');

-- 2
CREATE OR REPLACE FUNCTION ZYSK(IN arg1 CHAR(4))
RETURNS NUMERIC(10,2) AS
$$
DECLARE
    wynik NUMERIC(10,2);
BEGIN
    SELECT p.cena - SUM(c.koszt * z.sztuk) - 0.90 INTO wynik FROM zawartosc z
        JOIN czekoladki c USING(idczekoladki)
        JOIN pudelka p USING(idpudelka)
            WHERE z.idpudelka = arg1 GROUP BY idpudelka, cena;

    RETURN wynik;
END
$$ LANGUAGE plpgsql;

SELECT zysk('alls');
SELECT datarealizacji, SUM(zysk(a.idpudelka) * a.sztuk) as zysk_z_dnia FROM artykuly a
    JOIN zamowienia z USING(idzamowienia)
    JOIN pudelka p USING(idpudelka) GROUP BY z.datarealizacji;

-- 3
CREATE FUNCTION sumaZamowien(IN arg1 INT)
RETURNS INT AS
$$
DECLARE
    wynik INT;
BEGIN
    SELECT COUNT(idklienta) INTO wynik FROM zamowienia WHERE idklienta = arg1;
    RETURN wynik;
END
$$  LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION rabat(IN arg1 INT)
RETURNS INT AS
$$
DECLARE
    wynik INT;
BEGIN
    SELECT SUM(a.sztuk * p.cena) INTO wynik FROM artykuly a
        JOIN zamowienia z USING(idzamowienia)
        JOIN pudelka p USING(idpudelka)
        WHERE z.idklienta = arg1 GROUP BY idklienta;

    IF wynik < 100
        THEN RETURN 0;
    ELSIF wynik >= 100 AND wynik < 200
        THEN RETURN 4;
    ELSIF wynik >= 200 AND wynik < 400
        THEN RETURN 7;
    ELSE RETURN 8;
END IF;
END
$$ LANGUAGE plpgsql;

SELECT sumaZamowien(4);
SELECT rabat(44);

-- 4
CREATE FUNCTION podwyzka()
RETURNS void AS
$$
DECLARE
    zmiana NUMERIC(10,2);
    c1 RECORD;
    z1 RECORD;
BEGIN
    FOR c1 IN SELECT * FROM czekoladki
    LOOP
        zmiana := CASE
            WHEN c1.koszt < 0.20 THEN 0.03
            WHEN c1.koszt BETWEEN 0.20 AND 0.29 THEN 0.04
            ELSE 0.05 END;

        UPDATE czekoladki SET koszt = koszt + zmiana WHERE idczekoladki = c1.idczekoladki;

        FOR z1 IN SELECT * FROM zawartosc WHERE idczekoladki = c1.idczekoladki
        LOOP
            UPDATE pudelka SET cena = cena + (zmiana * z1.sztuk) WHERE idpudelka = z1.idpudelka;
        END LOOP;
    END LOOP;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION obnizka()
RETURNS void AS
$$
DECLARE
    zmiana NUMERIC(10,2);
    c1 RECORD;
    z1 RECORD;
BEGIN
    FOR c1 IN SELECT * FROM czekoladki
    LOOP
        zmiana := CASE
            WHEN c1.koszt < 0.20 THEN 0.03
            WHEN c1.koszt BETWEEN 0.20 AND 0.29 THEN 0.04
            ELSE 0.05 END;

        UPDATE czekoladki SET koszt = koszt - zmiana WHERE idczekoladki = c1.idczekoladki;

        FOR z1 IN SELECT * FROM zawartosc WHERE idczekoladki = c1.idczekoladki
        LOOP
            UPDATE pudelka SET cena = cena - (zmiana * z1.sztuk) WHERE idpudelka = z1.idpudelka;
        END LOOP;
    END LOOP;
END
$$ LANGUAGE plpgsql;

SELECT podwyzka();
SELECT obnizka();
SELECT idczekoladki, koszt FROM czekoladki ORDER BY idczekoladki;

-- 6
CREATE FUNCTION zamowieniaKlienta(IN arg1 INT)
RETURNS TABLE(
    r_idzamowienia INT,
    r_idpudelka CHARACTER,
    r_datarealizacji DATE) AS
$$
BEGIN
    RETURN QUERY SELECT z.idzamowienia, a.idpudelka, z.datarealizacji
    FROM
        zamowienia z
        JOIN klienci k USING(idklienta)
        JOIN artykuly a USING(idzamowienia)
    WHERE k.idklienta = arg1;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM zamowieniaKlienta(1);

CREATE FUNCTION klienciMiasto(IN arg1 VARCHAR(40))
RETURNS TABLE(
    r_idklienta INT,
    r_nazwa VARCHAR(130),
    r_miejscowosc VARCHAR(15),
    r_ulica VARCHAR(30),
    r_kod  CHAR(6),
    r_telefon VARCHAR(20)) AS
$$
BEGIN
    RETURN QUERY SELECT * FROM klienci WHERE miejscowosc ~* arg1;
END
$$ LANGUAGE plpgsql;

SELECT * FROM klienciMiasto('katowice');

SET SEARCH_PATH TO kwiaciarnia;
CREATE OR REPLACE FUNCTION rabat(IN arg1 VARCHAR(10))
RETURNS INT AS
$$
DECLARE
    rabat INT;
    wartosc_zamowien NUMERIC(10,2);
    wartosc_historii NUMERIC(10,2);
BEGIN
    SELECT SUM(z.cena) INTO wartosc_zamowien FROM zamowienia z WHERE z.idklienta = arg1 GROUP BY z.idklienta;
    SELECT SUM(h.cena) INTO wartosc_historii FROM historia h
        WHERE h.idklienta = arg1 AND h.termin >= CURRENT_DATE - INTERVAL '7 days' GROUP BY h.idklienta;
    wartosc_zamowien := wartosc_zamowien + wartosc_historii;

    IF wartosc_zamowien BETWEEN 0 AND 100 THEN rabat:= 5;
    ELSIF wartosc_zamowien BETWEEN 100 AND 400 THEN rabat:=10;
    ELSIF wartosc_zamowien BETWEEN 400 AND 700 THEN rabat:=15;
    ELSE rabat := 20;
    END IF;

    RETURN rabat;
END
$$ LANGUAGE plpgsql;

SELECT * FROM kwiaciarnia.klienci;

SELECT rabat('mbabik');
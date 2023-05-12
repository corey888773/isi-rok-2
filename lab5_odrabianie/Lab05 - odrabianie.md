# Laboratorium 05: Programowanie współbieżne i synchronizacja (Thread)
## Programowanie zaawansowane 2

- Maksymalna liczba punktów: 10

- Skala ocen za punkty:
    - 9-10 ~ bardzo dobry (5.0)
    - 8 ~ plus dobry (4.5)
    - 7 ~ dobry (4.0)
    - 6 ~ plus dostateczny (3.5)
    - 5 ~ dostateczny (3.0)
    - 0-4 ~ niedostateczny (2.0)

Celem laboratorium jest zapoznanie z programowaniem współbieżnym i synchronizacją procesów przy użyciu klasy Thread. Zakładamy, że nie używamy wątków tła (Thread.IsBackground == false). Pamiętaj o sekcjach krytycznych! Proszę wykonać następujące programy.

1. [2 punkty] Napisz program, który uruchomi n wątków (n ma być wczytane z klawiatury) i poczeka, aż wszystkie z tych wątków zaczną się wykonywać. Uruchomienie Thread.Start() nie jest równoznaczne z tym, że dany wątek zaczął się już wykonywać. Uznajmy, że wykonanie zaczyna się wtedy, kiedy wątek wykonał co najmniej jedną instrukcje w swoim kodzie. Kiedy wszystkie wątki zostaną uruchomione główny wątek ma o tym poinformować (wypisać informację do konsoli) a następnie zainicjować zamknięcie wszystkich wątków. Po otrzymaniu informacji, że wszystkie wątki zostaną zamknięte, główny program ma o tym poinformować oraz sam zakończyć działanie. 

2. [4 punkty] Napisz program modelujący problem producent-konsument. Program ma uruchomić n wątków generujących dane oraz m wątków pobierających dane (n i m pobieramy z klawiatury). Każdy z wątków ma  przechowywać informację o swoim numerze porządkowym, załóżmy, że są numerowane od 0..n-1 i odpowiednio od 0..m-1. Generowanie i odpowiednio odczytywanie danych przez każdy wątek ma odbywać się w losowych przedziałach czasu, które będą podawane jako parametr dla danego wątku. Generowane dane mają być umieszczane na liście (lub innej strukturze), załóżmy, że dane to obiekty klasy,  które będą miały identyfikator informujący o numerze porządkowym wątku, który je wygenerował. Wątek pobierający dane pobiera i usuwa zawsze ostatni element ze struktury danych   i "zapamiętuje", jaki był identyfikator wątku producenta, który te dane tam umieścił. Programy producentów mają zatrzymać się, kiedy wygenerują sumarycznie co najmniej 100 porcji danych. Programy konsumentów mają zatrzymywać się po zatrzymaniu programów producentów, kiedy pobiorą wszystkie wyprodukowane przez producentów dane. Każdy zatrzymywany wątek konsumenta ma wypisać ile "skonsumował" danych od poszczególnych producentów,
np. Producent 0 - 4, Producent 1 - 5 itd. W tym momencie ma się również zakończyć główny program.

2. [4 punkty] Napisz program, który będzie monitorował w czasie rzeczywistym zmiany zachodzące w wybranym katalogu polegające na usuwaniu lub dodawaniu do niego plików, ma monitorować również podkatalogi! Jeżeli w katalogu pojawi się nowy plik program ma wypisać: "dodano plik [nazwa pliku]" a jeśli usunięto plik "usunięto plik [nazwa pliku]". Program ma się zatrzymywać po wciśnięciu litery "q". Monitorowanie ma być  w osobnym wątku niż oczekiwanie na wciśnięcie klawisza! 

Powodzenia!
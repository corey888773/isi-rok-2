URUCHOMIENIE PROGRAMU:
z katalogu Library-project/main/target/main-1.0-jar-with-dependencies.jar
>> java -jar main-1.0-jar-with-dependencies.jar



Program posiada klasy reprezentujące bibliotekę, czytelinków i pisarzy.
Do biblioteki może wejść maksymalnie 5 czytelników naraz lub 1 pisarz. Program nie może zagłodzić wątków tzn. gdy
czeka pisarz to nie może wejść więcej czytelników.

Po uruchomieniu zostają zainicjowane wątki dla czytelników i pisarzy, umieszczone w liście threads.
Wszystkie wątki są uruchamiane po kolei. Wątki pisarzy i czytelników są w nieskończonej pętli, w której
wykonują operacje na bibliotece.

Na początku wszystkie wątki pisarzy i czytelników są w stanie WAITING w kolejce. Następnie czekają na 'permit', aby
zgodnie z podanymi wyżej zasadami móc wejść do biblioteki, zwalniając miejsce w kolejce. Po wyjściu z biblioteki
ponownie zostają dodani do kolejki i czekają na 'permit'. Pętla jest nieskończona, aczkolwiek posiada counter, który
powstrzymuje ją przed wykonaniem się w nieskończoność. Po wykonaniu się counter'a, wątki są zatrzymywane.

Przy wywołaniu funckji add...() pobierany zostaje permit z semafora queue (zabezpiecza nas to przed zagłodzeniem writera)
który po wejściu do biblioteki zostaje zwrócony. Pozwala to na wpuszczenie kilku czytelników do biblioteki naraz, ale
gdy czeka writer to nikt nie może wejść do biblioteki.


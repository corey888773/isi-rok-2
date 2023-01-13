package pl.edu.agh.kis.pz1;

import pl.edu.agh.kis.pz1.util.Reader;
import pl.edu.agh.kis.pz1.util.Writer;
import pl.edu.agh.kis.pz1.util.Library;

import java.util.ArrayList;
import java.util.List;

public class Main {
    public static void main( String[] args ) {

        Library library = new Library();
        List<Thread> threads = new ArrayList<>();

        for (int i = 1; i <= 5; i++) {
            threads.add(new Reader(library, i));
        }

        for (int i = 1; i <= 3; i++) {
            threads.add(new Writer(library, i));
        }

        for (int i = 6; i <= 9; i++) {
            threads.add(new Reader(library, i));
        }

        for (Thread thread : threads) {
            thread.start();
        }
    }
}

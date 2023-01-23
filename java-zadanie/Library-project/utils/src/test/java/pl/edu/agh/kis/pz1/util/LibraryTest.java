package pl.edu.agh.kis.pz1.util;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Semaphore;
import static org.junit.jupiter.api.Assertions.*;

class LibraryTest {

    private static final int MAX_READERS = 5;
    private static final int NUM_READERS = 5;
    private static final int NUM_WRITERS = 1;

    private final Library library = new Library();

    @org.junit.jupiter.api.Test
    void testLibrarySemaphoreCreation() {
        Library library = new Library();
        Semaphore semaphore = library.getLibraryRoom();
        assertEquals(5, semaphore.availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testQueueSemaphoreCreation() {
        Library library = new Library();
        Semaphore semaphore = library.getQueue();
        assertEquals(1, semaphore.availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddReader() throws InterruptedException {
        library.addReader(1);
        assertEquals(4, library.getLibraryRoom().availablePermits());
        assertEquals(1, library.getQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddReaderBlockedByReaders() throws InterruptedException {
        Library library = new Library();
        List<Reader> readers = new ArrayList<>();
        readers.add(new Reader(library, 1));
        readers.add(new Reader(library, 2));
        readers.add(new Reader(library, 3));
        readers.add(new Reader(library, 4));
        readers.add(new Reader(library, 5));
        for (Reader reader : readers) {
            reader.start();
        }

        readers.add(new Reader(library, 6));
        long startTime = System.currentTimeMillis();
        readers.get(5).start();
        readers.get(5).join();
        long endTime = System.currentTimeMillis();
        long deltaTime = endTime - startTime;

        assertTrue(deltaTime >= 2000);
    }

    @org.junit.jupiter.api.Test
    void testAddReaderBlockedByWriter() throws InterruptedException {
        Library library = new Library();
        Writer writer = new Writer(library, 1);
        Reader reader = new Reader(library, 1);

        writer.start();
        long startTime = System.currentTimeMillis();
        reader.start();
        reader.join();
        long endTime = System.currentTimeMillis();
        long deltaTime = endTime - startTime;

        assertTrue(deltaTime >= 2000);
    }

    @org.junit.jupiter.api.Test
    void testReleaseReader() throws InterruptedException {
        library.addReader(1);
        library.addReader(2);
        library.addReader(3);
        library.releaseReader(1);
        assertEquals(3, library.getLibraryRoom().availablePermits());
        assertEquals(1, library.getQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddWriter() throws InterruptedException {
        library.addWriter(1);
        assertEquals(0, library.getLibraryRoom().availablePermits());
        assertEquals(1, library.getQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddWriterBlockedByReaders() throws InterruptedException {
        Library library = new Library();
        List<Reader> readers = new ArrayList<>();
        List<Writer> writers = new ArrayList<>();

        writers.add(new Writer(library, 1));
        writers.add(new Writer(library, 2));
        readers.add(new Reader(library, 1));
        readers.add(new Reader(library, 1));
        readers.add(new Reader(library, 2));
        readers.add(new Reader(library, 3));
        readers.add(new Reader(library, 4));
        readers.add(new Reader(library, 5));

        for (Reader reader : readers) {
            reader.start();
        }

        long startTime = System.currentTimeMillis();
        for (Writer writer : writers) {
            writer.start();
            writer.join();
        }

        long endTime = System.currentTimeMillis();
        long deltaTime = endTime - startTime;

        assertTrue(deltaTime >= 4000);
    }

    @org.junit.jupiter.api.Test
    void testAddWriterBlockedByWriter() throws InterruptedException {
        Writer writer1 = new Writer(library, 1);
        Writer writer2 = new Writer(library, 2);

        writer1.start();
        long startTime = System.currentTimeMillis();
        writer2.start();
        writer2.join();
        long endTime = System.currentTimeMillis();
        long deltaTime = endTime - startTime;

        assertTrue(deltaTime >= 2000);
    }

    @org.junit.jupiter.api.Test
    void testReleaseWriter() throws InterruptedException {
        library.addWriter(1);
        assertEquals(0, library.getLibraryRoom().availablePermits());
        assertEquals(1, library.getQueue().availablePermits());

        library.releaseWriter(1);
        assertEquals(5, library.getLibraryRoom().availablePermits());
        assertEquals(1, library.getQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testWriterEnteringAfterReaders() {
        Library library = new Library();
        List<Reader> readers = new ArrayList<>();
        Writer writer1 = new Writer(library, 1);
        readers.add(new Reader(library, 1));
        readers.add(new Reader(library, 2));
        readers.add(new Reader(library, 3));
        readers.add(new Reader(library, 4));
        readers.add(new Reader(library, 5));

        for (Reader reader : readers) {
            reader.start();
        }
        long startTime = System.currentTimeMillis();
        writer1.start();
        long endTime = System.currentTimeMillis();
        long deltaTime = endTime - startTime;

        assertTrue(deltaTime < 1000);
    }

    @org.junit.jupiter.api.Test
    void testReaderEnteringAfterWriter() throws InterruptedException {
        Library library = new Library();
        Writer writer1 = new Writer(library, 1);
        Reader reader1 = new Reader(library, 1);

        writer1.start();
        writer1.join();
        long startTime = System.currentTimeMillis();
        reader1.start();
        long endTime = System.currentTimeMillis();
        long deltaTime = endTime - startTime;

        assertTrue(deltaTime < 1000);
    }

    @org.junit.jupiter.api.Test
    void testReleaseReader2() throws InterruptedException {
        for (int i = 0; i < NUM_READERS; i++) {
            library.addReader(i);
        }
        for (int i = 0; i < NUM_READERS; i++) {
            library.releaseReader(i);
            assertEquals(NUM_READERS - i - 1, library.getLibraryReadersCounter());
        }
    }

    @org.junit.jupiter.api.Test
    void testGetSemaphores() {
        assertEquals(MAX_READERS, library.getLibraryRoom().availablePermits());
        assertEquals(1, library.getQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testGetData() throws InterruptedException {
        for (int i = 0; i < NUM_READERS; i++) {
            library.addReader(i);
        }

        String data = library.getSummary();
        assertTrue(data.contains("Readers in library: " + NUM_READERS));
        assertTrue(data.contains("Writers in library: 0"));
        assertTrue(data.contains("Readers in queue: 0"));
        assertTrue(data.contains("Writers in queue: 0"));
    }
    @org.junit.jupiter.api.Test
    void testGetLibraryReadersCounter() throws InterruptedException {
        for (int i = 0; i < NUM_READERS; i++) {
            library.addReader(i);
        }
        assertEquals(NUM_READERS, library.getLibraryReadersCounter());

        for (int i = 0; i < NUM_READERS; i++) {
            library.releaseReader(i);
        }
        assertEquals(0, library.getLibraryReadersCounter());
    }

    @org.junit.jupiter.api.Test
    void testGetLibraryWritersCounter() throws InterruptedException {
        library.addWriter(1);
        assertEquals(1, library.getLibraryWritersCounter());

        library.releaseWriter(1);
        assertEquals(0, library.getLibraryWritersCounter());
    }

    @org.junit.jupiter.api.Test
    void testGetQueueReadersCounter() throws InterruptedException {
        for (int i = 0; i < NUM_READERS; i++) {
            library.addReader(i);
        }
        assertEquals(0, library.getQueueReadersCounter());
    }

    @org.junit.jupiter.api.Test
    void testGetQueueWritersCounter() throws InterruptedException {
        for (int i = 0; i < NUM_WRITERS; i++) {
            library.addWriter(i);
        }
        assertEquals(0, library.getQueueWritersCounter());
    }
}

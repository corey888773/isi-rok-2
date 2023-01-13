package pl.edu.agh.kis.pz1.util;

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
        Semaphore semaphore = library.getSemaphoreLibrary();
        assertEquals(5, semaphore.availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testQueueSemaphoreCreation() {
        Library library = new Library();
        Semaphore semaphore = library.getSemaphoreQueue();
        assertEquals(1, semaphore.availablePermits());
    }


    @org.junit.jupiter.api.Test
    void testAddReader() throws InterruptedException {
        library.addReader(1);
        assertEquals(4, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());

        library.addReader(2);
        assertEquals(3, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddReaderBlockedByReaders() throws InterruptedException {
        Library library = new Library();
        Reader reader1 = new Reader(library, 1);
        Reader reader2 = new Reader(library, 2);
        Reader reader3 = new Reader(library, 3);
        Reader reader4 = new Reader(library, 4);
        Reader reader5 = new Reader(library, 5);
        Reader reader6 = new Reader(library, 6);

        reader1.start();
        reader2.start();
        reader3.start();
        reader4.start();
        reader5.start();
        long startTime = System.currentTimeMillis();
        reader6.start();
        reader6.join();
        long endTime = System.currentTimeMillis();
        long elapsedTime = endTime - startTime;

        assertTrue(elapsedTime >= 2000);
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
        long elapsedTime = endTime - startTime;

        assertTrue(elapsedTime >= 2000);
    }

    @org.junit.jupiter.api.Test
    void testReleaseReader() throws InterruptedException {
        library.addReader(1);
        library.addReader(2);
        library.addReader(3);
        library.releaseReader(1);
        assertEquals(3, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());
        library.releaseReader(2);
        assertEquals(4, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddWriter() throws InterruptedException {
        library.addWriter(1);
        assertEquals(0, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testAddWriterBlockedByReaders() throws InterruptedException {
        Library library = new Library();
        Writer writer1 = new Writer(library, 1);
        Writer writer2 = new Writer(library, 2);
        Reader reader1 = new Reader(library, 1);
        Reader reader2 = new Reader(library, 2);
        Reader reader3 = new Reader(library, 3);
        Reader reader4 = new Reader(library, 4);
        Reader reader5 = new Reader(library, 5);

        reader1.start();
        reader2.start();
        reader3.start();
        reader4.start();
        reader5.start();
        long startTime = System.currentTimeMillis();
        writer1.start();
        writer2.start();
        writer1.join();
        writer2.join();
        long endTime = System.currentTimeMillis();
        long elapsedTime = endTime - startTime;

        assertTrue(elapsedTime >= 4000);
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
        long elapsedTime = endTime - startTime;

        assertTrue(elapsedTime >= 2000);
    }

    @org.junit.jupiter.api.Test
    void testReleaseWriter() throws InterruptedException {
        library.addWriter(1);
        assertEquals(0, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());

        library.releaseWriter(1);
        assertEquals(5, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testWriterEnteringAfterReaders() {
        Library library = new Library();
        Writer writer1 = new Writer(library, 1);
        Reader reader1 = new Reader(library, 1);
        Reader reader2 = new Reader(library, 2);
        Reader reader3 = new Reader(library, 3);
        Reader reader4 = new Reader(library, 4);
        Reader reader5 = new Reader(library, 5);

        reader1.start();
        reader2.start();
        reader3.start();
        reader4.start();
        reader5.start();
        long startTime = System.currentTimeMillis();
        writer1.start();
        long endTime = System.currentTimeMillis();
        long elapsedTime = endTime - startTime;

        assertTrue(elapsedTime < 1000);
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
        long elapsedTime = endTime - startTime;

        assertTrue(elapsedTime < 1000);
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
        assertEquals(MAX_READERS, library.getSemaphoreLibrary().availablePermits());
        assertEquals(1, library.getSemaphoreQueue().availablePermits());
    }

    @org.junit.jupiter.api.Test
    void testGetData() throws InterruptedException {
        for (int i = 0; i < NUM_READERS; i++) {
            library.addReader(i);
        }

        String data = library.getData();
        assertTrue(data.contains("Readers in library: " + NUM_READERS));
        assertTrue(data.contains("Writers in library: 0"));
        assertTrue(data.contains("Readers in queue: 0"));
        assertTrue(data.contains("Writers in queue: 0"));

        for (int i = 0; i < NUM_READERS; i++) {
            library.releaseReader(i);
        }

        data = library.getData();
        assertTrue(data.contains("Readers in library: 0"));
        assertTrue(data.contains("Writers in library: 0"));
        assertTrue(data.contains("Readers in queue: 0"));
        assertTrue(data.contains("Writers in queue: 0"));

        library.addWriter(1);

        data = library.getData();
        assertTrue(data.contains("Readers in library: 0"));
        assertTrue(data.contains("Writers in library: 1"));
        assertTrue(data.contains("Readers in queue: 0"));
        assertTrue(data.contains("Writers in queue: 0"));
    }

    @org.junit.jupiter.api.Test
    void testgetLibraryReadersCounter() throws InterruptedException {
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
    void testgetLibraryWritersCounter() throws InterruptedException {
        library.addWriter(1);
        assertEquals(1, library.getLibraryWritersCounter());

        library.releaseWriter(1);
        assertEquals(0, library.getLibraryWritersCounter());
    }

    @org.junit.jupiter.api.Test
    void testgetQueueReadersCounter() throws InterruptedException {
        for (int i = 0; i < NUM_READERS; i++) {
            library.addReader(i);
        }
        assertEquals(0, library.getQueueReadersCounter());
    }

    @org.junit.jupiter.api.Test
    void testgetQueueWritersCounter() throws InterruptedException {
        for (int i = 0; i < NUM_WRITERS; i++) {
            library.addWriter(i);
        }
        assertEquals(0, library.getQueueWritersCounter());
    }
}

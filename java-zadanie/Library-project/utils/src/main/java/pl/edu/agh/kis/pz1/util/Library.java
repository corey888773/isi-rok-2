package pl.edu.agh.kis.pz1.util;

import java.util.concurrent.Semaphore;

/**
 * Library class - a class that simulates a library with readers and writers
 */
public class Library {
    private static final int MAX_READERS = 5;
    private final Semaphore libraryRoom;
    private final Semaphore queue;

    private int libraryReadersCounter = 0;
    private int libraryWritersCounter = 0;
    private int queueReadersCounter = 0;
    private int queueWritersCounter = 0;

    /**
        * Creates a library.
     */
    public Library() {
        libraryRoom = new Semaphore(MAX_READERS, true);
        queue = new Semaphore(1, true);
    }

    /**
     * Adds reader to the library.
     * @param id id of the reader
     * @throws InterruptedException
     */
    public void addReader(int id) throws InterruptedException {

        synchronized (this) {
            queueReadersCounter++;
        }

        queue.acquire();
        System.out.printf("Reader %s is waiting in a queue %n", id);
        libraryRoom.acquire();
        System.out.printf("Reader %s enters the library %n", id);

        synchronized (this) {
            queueReadersCounter--;
            libraryReadersCounter++;

            System.out.println(getData());
        }

        queue.release();
    }

    /**
     * Releases a permit, returning it to the semaphore.
     * @param id id of the reader
     */
    public void releaseReader(int id) {
        System.out.printf("Reader %s leaves the library %n", id);
        libraryRoom.release();

        synchronized (this) {
            libraryReadersCounter--;
        }
    }

    /**
     * Writers are not allowed to enter the library if there are any readers inside.
     * @param id Writer's id.
     * @throws InterruptedException
     */
    public void addWriter(int id) throws InterruptedException {

        synchronized (this) {
            queueWritersCounter++;
        }

        queue.acquire();
        System.out.printf("Writer %s is waiting in a queue %n", id);
        libraryRoom.acquire(5);
        System.out.printf("Writer %s enters the library %n", id);

        synchronized (this) {
            queueWritersCounter--;
            libraryWritersCounter++;

            System.out.println(getData());
        }

        queue.release();
    }

    /**
     * Method to release a writer.
     * @param id id of the writer
     */
    public void releaseWriter(int id) {
        System.out.printf("Writer %s leaves the library %n", id);
        libraryRoom.release(5);

        synchronized (this) {
            libraryWritersCounter--;
        }
    }

    /**
     * Method to get data about readers and writers in the library.
     * @return data about readers and writers in the library
     */
    public String getData() {
        return String.format
                ("\n------------------------------" +
                "\nReaders in library: %s" +
                "\nWriters in library: %s" +
                "\nReaders in queue: %s" +
                "\nWriters in queue: %s" +
                "\n------------------------------\n",
                libraryReadersCounter, libraryWritersCounter, queueReadersCounter, queueWritersCounter);
    }

    /** Returns the number of available permits.
     * @return the available
     */
    public Semaphore getSemaphoreLibrary() {
        return libraryRoom;
    }

    /**
     * Returns the number of available permits.
     * @return the available permits
     */
    public Semaphore getSemaphoreQueue() {
        return queue;
    }

    /**
     * Returns the number of readers in the library.
     * @return the number of readers in the library
     */
    public int getLibraryReadersCounter() {
        return libraryReadersCounter;
    }

    /**
     * Returns the number of writers in the library.
     * @return the number of writers in the library
     */
    public int getLibraryWritersCounter() {
        return libraryWritersCounter;
    }

    /**
     * Returns the number of readers in the queue.
     * @return the number of readers in the queue
     */
    public int getQueueReadersCounter() {
        return queueReadersCounter;
    }

    /**
     * Returns the number of writers in the queue.
     * @return the number of writers in the queue
     */
    public int getQueueWritersCounter() {
        return queueWritersCounter;
    }
}

package pl.edu.agh.kis.pz1.util;

import java.util.concurrent.Semaphore;

/**
 * Library class - a class that simulates a library with readers and writers
 */
public class Library {

    //region Fields and gettters/setters
    private static final int MAX_READERS = 5;
    private final Semaphore libraryRoom;
    private final Semaphore queue;
    private int libraryReadersCounter = 0;
    private int libraryWritersCounter = 0;
    private int queueReadersCounter = 0;
    private int queueWritersCounter = 0;

    /**
     * getLibraryRoom() - a method that returns a semaphore that represents the library room
     * @return a semaphore that represents the library room
     */
    public Semaphore getLibraryRoom() {
        return libraryRoom;
    }

    /**
     * getQueue() - a method that returns a semaphore that represents the queue
     * @return a semaphore that represents the queue
     */
    public Semaphore getQueue() {
        return queue;
    }

    /**
     * getLibraryReadersCounter() - a method that returns the number of readers in the library
     * @return the number of readers in the library
     */
    public int getLibraryReadersCounter() {
        return libraryReadersCounter;
    }

    /**
     * getLibraryWritersCounter() - a method that returns the number of writers in the library
     * @return the number of writers in the library
     */
    public int getLibraryWritersCounter() {
        return libraryWritersCounter;
    }

    /**
     * getQueueReadersCounter() - a method that returns the number of readers in the queue
     * @return the number of readers in the queue
     */
    public int getQueueReadersCounter() {
        return queueReadersCounter;
    }

    /**
     * getQueueWritersCounter() - a method that returns the number of writers in the queue
     * @return the number of writers in the queue
     */
    public int getQueueWritersCounter() {
        return queueWritersCounter;
    }
    //endregion

    //region Methods and constructors
    /**
        * Creates a library.
     */
    public Library() {
        libraryRoom = new Semaphore(MAX_READERS, true);
        queue = new Semaphore(1, true);
    }

    /**
     * Adds reader to the library.
     * @param code of the reader
     * @throws InterruptedException
     */
    public void addReader(int code) throws InterruptedException {

        synchronized (this) {
            queueReadersCounter++;
        }
        queue.acquire();
        System.out.printf("Reader %s is WAITING in a queue %n", code);
        libraryRoom.acquire();
        System.out.printf("Reader %s ENTERS the library %n", code);

        synchronized (this) {
            queueReadersCounter--;
            libraryReadersCounter++;

            System.out.print(getSummary());
        }
        queue.release();
    }

    /**
     * Releases a permit, returning it to the semaphore.
     * @param code of the reader
     */
    public void releaseReader(int code) {
        System.out.printf("Reader %s LEAVES the library %n", code);
        libraryRoom.release();

        synchronized (this) {
            libraryReadersCounter--;
        }
    }

    /**
     * Writers are not allowed to enter the library if there are any readers inside.
     * @param code Writer's code.
     * @throws InterruptedException
     */
    public void addWriter(int code) throws InterruptedException {

        synchronized (this) {
            queueWritersCounter++;
        }
        queue.acquire();
        System.out.printf("Writer %s IS WAITING in a queue %n", code);
        libraryRoom.acquire(5);
        System.out.printf("Writer %s ENTERS the library %n", code);

        synchronized (this) {
            queueWritersCounter--;
            libraryWritersCounter++;

            System.out.print(getSummary());
        }
        queue.release();
    }

    /**
     * Method to release a writer.
     * @param code of the writer
     */
    public void releaseWriter(int code) {
        System.out.printf("Writer %s LEAVES the library %n", code);
        libraryRoom.release(5);

        synchronized (this) {
            libraryWritersCounter--;
        }
    }

    /**
     * Method to print data about readers and writers in the library.
     */
    public String getSummary() {
        return String.format("________________SUMMARY________________" +
                        "\nReaders in library: %s" +
                        "\nWriters in library: %s" +
                        "\nReaders in queue: %s" +
                        "\nWriters in queue: %s" +
                        "\n=======================================\n\n\n",
                        libraryReadersCounter, libraryWritersCounter, queueReadersCounter, queueWritersCounter);
    }
    //endregion
}

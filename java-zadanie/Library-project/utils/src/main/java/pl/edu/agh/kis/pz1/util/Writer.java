package pl.edu.agh.kis.pz1.util;

import java.time.Duration;

/**
 * Writer class - a class that simulates a writer
 */
public class Writer extends Thread {

    //region Fields and gettters/setters
    private final Library library;
    private final int code;

    /**
     * Returns code of the writer.
     * @return code of the writer
     */
    public int getCode() {
        return code;
    }

    /**
     * Returns library.
     * @return library
     */
    public Library getLibrary() {
        return library;
    }

    /**
     * Creates a writer with a given code.
     *
     * @param library library
     * @param code      code of the writer
     */

    //endregion

    //region Methods and constructors
    public Writer(Library library, int code) {
        if (library == null) {
            throw new IllegalArgumentException("Library cannot be null");
        }

        this.library = library;
        this.code = code;
    }

    /**
     * Method to run a writer.
     */
    @Override
    public void run() {
        int counter = 0;
        while (true) {
            try {
                library.addWriter(code);
                System.out.printf("Writer %s WRITES in a library %n", code);

                Thread.sleep(Duration.ofSeconds(1).toMillis());

            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                library.releaseWriter(code);
            }

            counter++;
            if (counter == 2) {
                break;
            }
        }

    }
    //endregion
}

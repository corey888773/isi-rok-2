package pl.edu.agh.kis.pz1.util;

/**
 * Reader class - a class that simulates a reader
 */
public class Reader extends Thread {

    //region Fields and gettters/setters
    private final Library library;
    private final int code;

    /**
     * Returns code of the reader.
     * @return code of the reader
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
    //endregion

    //region Methods and constructors
    /**
     * Creates a reader with a given code.
     *
     * @param library library
     * @param code of the reader
     */
    public Reader(Library library, int code) {
        if (library == null) {
            throw new IllegalArgumentException("Library cannot be null");
        }

        this.library = library;
        this.code = code;
    }

    /**
     * Method to run a reader.
     */
    @Override
    public void run() {
        int counter = 0;
        while (true) {
            try {
                library.addReader(code);
                System.out.printf("Reader %s reads in a library %n", code);

                Thread.sleep(2000);

            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                library.releaseReader(code);
            }

            counter++;
            if (counter == 2) {
                break;
            }
        }
    }
    //endregion
}

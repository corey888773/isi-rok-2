package pl.edu.agh.kis.pz1.util;

/**
 * Reader class - a class that simulates a reader
 */
public class Reader extends Thread {
    private final Library library;
    private final int id;

    /**
     * Creates a reader with a given id.
     *
     * @param library library
     * @param id      id of the reader
     */
    public Reader(Library library, int id) {
        if (library == null) {
            throw new IllegalArgumentException("Library cannot be null");
        }

        this.library = library;
        this.id = id;
    }

    /**
     * Method to run a reader.
     */
    @Override
    public void run() {
        int counter = 0;
        while (true) {
            try {
                library.addReader(id);
                System.out.printf("Reader %s reads in a library %n", id);

                Thread.sleep(2000);

            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                library.releaseReader(id);
            }

            counter++;
            if (counter == 2) {
                break;
            }
        }
    }

    /**
     * Returns id of the reader.
     * @return id of the reader
     */
    public int getOwnId() {
        return id;
    }

    /**
     * Returns library.
     * @return library
     */
    public Library getLibrary() {
        return library;
    }
}

package pl.edu.agh.kis.pz1.util;

/**
 * Writer class - a class that simulates a writer
 */
public class Writer extends Thread {
    private final Library library;
    private final int id;

    /**
     * Creates a writer with a given id.
     *
     * @param library library
     * @param id      id of the writer
     */
    public Writer(Library library, int id) {
        if (library == null) {
            throw new IllegalArgumentException("Library cannot be null");
        }

        this.library = library;
        this.id = id;
    }

    /**
     * Method to run a writer.
     */
    @Override
    public void run() {
        int counter = 0;
        while (true) {
            try {
                library.addWriter(id);
                System.out.printf("Writer %s writes in a library %n", id);

                Thread.sleep(2000);

            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            } catch (Exception e) {
                e.printStackTrace();
            } finally {
                library.releaseWriter(id);
            }

            counter++;
            if (counter == 2) {
                break;
            }
        }

    }

    /**
     * Returns id of the writer.
     * @return id of the writer
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

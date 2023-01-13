package pl.edu.agh.kis.pz1.util;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class ReaderTest {

    @org.junit.jupiter.api.Test
    void testReaderRunMethod() throws InterruptedException {
        Library library = mock(Library.class);
        Reader reader = new Reader(library, 1);

        reader.start();
        reader.join();

        verify(library, atLeastOnce()).addReader(1);
        verify(library, atLeastOnce()).releaseReader(1);
    }

    @org.junit.jupiter.api.Test
    void testRunException() {
        Library library = mock(Library.class);

        Writer writer = new Writer(library, 1);

        assertDoesNotThrow(writer::run);
    }

    @org.junit.jupiter.api.Test
    void testReaderConstructor() {
        Library library = mock(Library.class);
        Reader reader = new Reader(library, 1);

        assertEquals(1, reader.getOwnId());
        assertEquals(library, reader.getLibrary());
    }

    @org.junit.jupiter.api.Test
    void testReaderConstructorException() {
        assertThrows(IllegalArgumentException.class, () -> new Reader(null, 1));
    }

    @org.junit.jupiter.api.Test
    void testGetOwnId() {
        Library library = mock(Library.class);
        Reader reader = new Reader(library, 1);

        assertEquals(1, reader.getOwnId());
    }

    @org.junit.jupiter.api.Test
    void testGetLibrary() {
        Library library = mock(Library.class);
        Reader reader = new Reader(library, 1);

        assertEquals(library, reader.getLibrary());
    }
}
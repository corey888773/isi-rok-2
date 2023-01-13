package pl.edu.agh.kis.pz1.util;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class WriterTest {

    @org.junit.jupiter.api.Test
    void testWriterRunMethod() throws InterruptedException {
        Library library = mock(Library.class);
        Writer writer = new Writer(library, 1);

        writer.start();
        writer.join();

        verify(library, atLeastOnce()).addWriter(1);
        verify(library, atLeastOnce()).releaseWriter(1);
    }

    @org.junit.jupiter.api.Test
    void testRunException() {
        Library library = mock(Library.class);

        Writer writer = new Writer(library, 1);

        assertDoesNotThrow(writer::run);
    }

    @org.junit.jupiter.api.Test
    void testWriterConstructor() {
        Library library = mock(Library.class);
        Writer writer = new Writer(library, 1);

        assertEquals(1, writer.getOwnId());
        assertEquals(library, writer.getLibrary());
    }

    @org.junit.jupiter.api.Test
    void testWriterConstructorException() {
        assertThrows(IllegalArgumentException.class, () -> new Writer(null, 1));
    }

    @org.junit.jupiter.api.Test
    void testGetOwnId() {
        Library library = mock(Library.class);
        Writer writer = new Writer(library, 1);

        assertEquals(1, writer.getOwnId());
    }

    @org.junit.jupiter.api.Test
    void testGetLibrary() {
        Library library = mock(Library.class);
        Writer writer = new Writer(library, 1);

        assertEquals(library, writer.getLibrary());
    }
}
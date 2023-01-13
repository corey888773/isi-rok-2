package pl.edu.agh.kis.pz1;

import static junit.framework.TestCase.assertNotNull;
import org.testng.annotations.Test;


class MainTest {

    @org.junit.jupiter.api.Test
    void shouldCreateMainObject(){
        Main main = new Main();
        assertNotNull("Main method called.", main);
    }
}



package pl.edu.agh.kis.pz1;

import org.junit.jupiter.api.Assertions;



class MainTest {

    @org.junit.jupiter.api.Test
    void testShouldInvokeMainConstructor(){
        Assertions.assertNotNull(new Main());
    }
}



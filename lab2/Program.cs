namespace test{
    abstract public class PosiadaczRachunku {
        
        override public abstract String ToString();
    }

    public class OsobaFizyczna : PosiadaczRachunku{
        public OsobaFizyczna(String imie, String nazwisko, String drugieimie, String pesel, String numerPaszportu){
            _imie=imie;
            _nazwisko=nazwisko;
            _drugieimie=drugieimie;
            _pesel=pesel;
            _numerPaszportu=numerPaszportu;
            if(_pesel==null && _numerPaszportu==null){
                throw new Exception("PESEL albo numer paszportu musza byc nie null");
            }
        }
        private String _nazwisko;
        private String _drugieimie;
        private String _pesel;
        private String _numerPaszportu;
        private String _imie;
        public override String ToString(){
            return "Osoba Ficzyna: "+ _imie + " " + _nazwisko;
        }
        public  string Imie
        {
            get => _imie;
            set => _imie = value;
        }

        public  string DrugieImie
        {
            get => _drugieimie;
            set => _drugieimie = value;
        }
        public   string Pesel
        {
            get => _pesel;
            set => _pesel = value;
        }
        public string NumerPaszportu
        {
            get => _numerPaszportu;
            set => _numerPaszportu = value;
        }
    }
    public class OsobaPrawna : PosiadaczRachunku{
        public OsobaPrawna(String nazwa,String siedziba){
            _nazwa=nazwa;
            _siedziba=siedziba;
        }
        private String _nazwa;
        private String _siedziba;
        public override String ToString(){
            return "Osoba Prawna: "+ _nazwa + " " + _siedziba;
        }
        public string Nazwa
        {
            get => _nazwa;
        }

        public string Siedziba
        {
            get => _siedziba;
        }
    }
    public class Transkacja{
        public Transkacja(RachunekBankowy rachunekZrodlowy, RachunekBankowy rachunekDocelowy, decimal kwota,string opis){
            _opis=opis;
            _rachunekDocelowy=rachunekDocelowy;
            _rachunekZrodlowy=rachunekZrodlowy;
            _kwota=kwota;
            if(rachunekDocelowy == null && rachunekZrodlowy == null){
                throw new Exception("Rachunek zrodlowy i rachunek docelowy musza zostac podane!");
            }
        }
        private RachunekBankowy _rachunekZrodlowy;
        private RachunekBankowy _rachunekDocelowy;
        private decimal _kwota;
        private String _opis;
        public RachunekBankowy RachunekZrodlowy
        {
            get => _rachunekZrodlowy;
            set => _rachunekZrodlowy = value;
        }
        public RachunekBankowy RachunekDocelowy
        {
            get => _rachunekDocelowy;
            set => _rachunekDocelowy= value;
        }
        public decimal Kwota
        {
            get => _kwota;
            set => _kwota= value;
        }
        public String Opis
        {
            get => _opis;
            set => _opis= value;
        }
        public static void DokonajTransakcji(RachunekBankowy rachunekZrodlowy, RachunekBankowy rachunekDocelowy, decimal kwota, string opis)
        {
            if(kwota < 0) 
                throw new Exception("Kwota nie moze byc ujemna");
            if(rachunekDocelowy == null && rachunekZrodlowy == null)
                throw new Exception("Rachunek zrodlowy i rachunek docelowy musza zostac podane!");
            if(rachunekZrodlowy?.StanRachunku - kwota < 0 && rachunekZrodlowy?.CzyDozwolonyDebet == false)
                throw new Exception("Niewystarczające środki");
            

            if(rachunekZrodlowy == null){
                rachunekDocelowy.StanRachunku += kwota;
                rachunekDocelowy.Transkacje.Add(new Transkacja(rachunekDocelowy, null, kwota, opis));
                return;
            }

            if(rachunekDocelowy == null){
                rachunekZrodlowy.StanRachunku -= kwota;
                rachunekZrodlowy.Transkacje.Add(new Transkacja(null, rachunekZrodlowy, kwota, opis));
                return;
            } 

        rachunekZrodlowy.StanRachunku -= kwota;
        rachunekDocelowy.StanRachunku += kwota;
        var transakcja = new Transkacja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
        rachunekZrodlowy.Transkacje.Add(transakcja);
        rachunekDocelowy.Transkacje.Add(transakcja);
        return;
        }
    }


    public class RachunekBankowy{
        public RachunekBankowy(String numer,decimal stanRachunku,bool czyDozwolonyDebet,List<PosiadaczRachunku> PosiadaczRachunku){
            _numer=numer;
            _stanRachunku=stanRachunku;
            _czyDozwolonyDebet=czyDozwolonyDebet;
            if(PosiadaczRachunku.Count>=1){
                _posiadaczeRachunku.AddRange(PosiadaczRachunku);
            }
            else{
                throw new Exception("Lista nie może być pusta");
            }
        }
        private String _numer;
        private decimal _stanRachunku;
        private bool _czyDozwolonyDebet;
        private List<PosiadaczRachunku> _posiadaczeRachunku= new List<PosiadaczRachunku>();

        private List<Transkacja> _transakcje = new List<Transkacja>();

        public string Numer
        {
            get => _numer;
            set => _numer = value;
        }

        public decimal StanRachunku
        {
            get => _stanRachunku;
            set => _stanRachunku = value;
        }

        public bool CzyDozwolonyDebet
        {
            get => _czyDozwolonyDebet;
            set => _czyDozwolonyDebet = value;
        }

        public List<PosiadaczRachunku> PosiadaczeRachunku
        {
            get => _posiadaczeRachunku;
            set => _posiadaczeRachunku = value;
        }

        public List<Transkacja> Transkacje
        {
            get => _transakcje;
            set => _transakcje = value;
        }
    }
    public class Program {
        public static void Main(string[]args)
        {
            // var o=new OsobaFizyczna("Maciej","Bobrek","Piotr",null,null);

            var posiadacz = new OsobaPrawna("nazwa", "siedziba");

            var rachunekBankowy1 = new RachunekBankowy("123", 15, false, new List<PosiadaczRachunku> {posiadacz});
            var rachunekBankowy2 = new RachunekBankowy("345", 50, true, new List<PosiadaczRachunku> {posiadacz});

            Console.WriteLine(rachunekBankowy1.StanRachunku);
            Transkacja.DokonajTransakcji(null, rachunekBankowy1, 10, "opis");
            Console.WriteLine(rachunekBankowy1.StanRachunku);


            Transkacja.DokonajTransakcji(rachunekBankowy2, rachunekBankowy1, 40, "opis");
            Console.WriteLine(rachunekBankowy1.StanRachunku);

            Transkacja.DokonajTransakcji(rachunekBankowy2, rachunekBankowy1, 20, "opis");
            Console.WriteLine(rachunekBankowy1.StanRachunku);
            Console.WriteLine(rachunekBankowy2.StanRachunku);
        }
    }
} 

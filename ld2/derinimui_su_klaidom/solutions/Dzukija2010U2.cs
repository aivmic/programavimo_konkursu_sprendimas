namespace Solutions;

using NLog;


/// <summary>
/// Solution for Metai Regionas.
/// </summary>
public class Dzukija2010U2
{
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        public int dienos;
        public int[] vaikai;
        public int[] zodziai;
    }

    /// <summary>
    /// A single output.
    /// </summary>
    public class Output
    {
        public int[] ats = { };
    }

    /// <summary>
    /// Test data. You can use a different layout if you want.
    /// </summary>
    public class TestData
    {        
        public static Input[] Inputs {get;} = {
            new Input{
              dienos = 3,
        vaikai = [ 7,7,10 ],
        zodziai = [ 3,14,2 ]
            }
            //data goes here
        };

        public static Output[] Outputs{get;} = {
            new Output{
              ats = [ 0,6,5 ]
            }
        };
    }

    /// <summary>
	/// Logger for this class.
	/// </summary>
	Logger mLog = LogManager.GetCurrentClassLogger();


    /// <summary>
    /// Runs the task solution.
    /// </summary>
    /// <param name="input">Input</param>
    /// <returns>Output</returns>
    public Output Run(Input input) 
    {
        int[] atsakymas = new int[50];
        int[] gal = new int[50];
        int skaicius = 0;
        for (int y=0; y < input.dienos;y++) {
        int pab = input.zodziai[y];
            for (int i = 0; i < input.vaikai[y]; i++)
            {
         
                if (skaicius <= pab)
                {
                    skaicius++;
                }
                if (skaicius >= pab)
                {
                    if (atsakymas[i] != -1)
                    {
                        atsakymas[i] = -1;
                    }
                    else
                    {
                        break;
                    }
                    skaicius = 1;
                }
                if (i == input.vaikai[y] - 1)
                {
                    i = -1;
                }
            }
            
            for (int i = 0; i < input.vaikai[y]; i++)
            {
                if (atsakymas[i] != -1)
                {
                    gal[y]++;
                }
            }
        
        }
        Output output = new Output();
        output.ats = gal;
        return output;

    }
}
//U2.Skaičiuotė.
//Vaikai žaidžia slėpynių.Išsiskaičiavimui, kas eis ieškoti, vaikai sustoja ratu. Vienas iš jų 
//skaičiuoja pradėdamas nuo savęs laikrodžio rodyklės kryptimi.Su kiekvienu skaičiuotės žodžiu
//pirštu parodo į eilinį vaiką. Tam, kuriam tenka paskutinis skaičiuotės žodis, išeina iš rato.
//Skaičiuotę tęsia rate toliau stovintis vaikas. Paskutinis likęs ir bus tas, kuris eis ieškoti.
//Tačiau atsitiko taip, kad nei vienas vaikas nepasitraukė iš rato.Tęsiant skaičiuotę eilinį kartą
//vaikai pastebėjo, kad paskutinis skaičiuotės žodis tenka vaikui, kuriam jau reikėjo išeiti iš
//rato. Parašykite programą, kuri suskaičiuotų, kiek tuo metu rate yra vaikų, kuriems dar nereikėjo
//išeiti iš rato.

//Duomenys.
//Tekstinio failo U2.txt pirmoje eilutėje parašytas dienų skaičius d (1  d  50 ), kada vaikai
//žaidė. Toliau kiekvienoje iš eilučių yra vienos dienos žaidžiančių vaikų skaičius n(1  n  50 )
//ir skaičiuotėje esančių žodžių skaičius m(1  m  50 ).

//Rezultatai.
    
//Ekrane spausdinti kiekvienai dienai vieną skaičių: keliems vaikams dar nereikėjo palikti rato,
//kai eilinio išsiskaičiavimo metu kuriam nors iš jų teko paskutinis skaičiuotės žodis antrą kartą.

//U2.txt
//3
//7 3
//7 14
//10 2
//Ekranas
//0
//6
//5
//Paaiškinimas
//Tokių vaikų neliko
//Liko 6 vaikai
//Liko 5 vaikai
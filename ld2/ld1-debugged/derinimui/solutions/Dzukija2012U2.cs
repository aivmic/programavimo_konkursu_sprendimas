namespace Solutions;

using NLog;
using NLog.Time;


/// <summary>
/// Solution for Metai Regionas.
/// </summary>
public class Dzukija2012U2
{
 
    
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        public int n;
        public int[] korteles;
        public int k;
        public int[] ribaa;
        public int[] ribab;
        public int dominanciosvietos;
        public int[] vietos;
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
              n = 10,
        korteles = [ 2, 5, 9, 8, 4, 7, 15, 25, 45, 99 ],
        k = 2,
        ribaa = [5, 2 ],
        ribab = [7, 8],
        dominanciosvietos = 3,
        vietos = [ 2, 5, 8 ]
            }
        };

        public static Output[] Outputs{get;} = {
            //dayta goes here
            new Output{
             ats = [ 25, 15, 5 ]
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
        int[] atsakymas = new int[input.n];
        Array.Copy(input.korteles, atsakymas, input.n);

        for (int y = 0; y < input.k; y++)
        {
            Array.Reverse(atsakymas, input.ribaa[y] - 1, input.ribab[y] - input.ribaa[y] + 1);
        }

        int[] gal = new int[input.dominanciosvietos];
        for (int i = 0; i < input.dominanciosvietos; i++)
        {
            gal[i] = atsakymas[input.vietos[i] - 1];
        }
        Output output = new Output();
        output.ats = gal;
        return output;
    }
}
//U2.Skaičiai.
//Turime n(2 ≤ n ≤ 50) kortelių.Ant kiekvienos kortel÷s užrašytas vienas sveikasis skaičius iš
//intervalo nuo 1 iki 100. Pradin÷ kortelių išd÷liojimo seka atsitiktin÷. Sekoje atliekama
//k(1 ≤ k ≤ 50) keitimų.Keitimas atliekamas nurodytoje sekos dalyje[a, b], kur 1 ≤ a<b ≤ n.
//Tos sekos nurodytos dalies kortel÷s išd÷liojamos atvirkščia tvarka, pvz., sekoje:
//15, 6, 8, 19, 22, kai duotas intervalas[2, 4], atlikus keitimą, bus gauta seka: 15, 19, 8, 6, 22.
//Keitimai atliekami jų pateikimo tvarka.Kokios kortel÷s bus nurodytose sekos vietose, atlikus
//visus keitimus?

//Duomenys.
//Failo U2.txt pirmoje eilut÷je yra užrašytas kortelių skaičius n, o antroje – kortelių sekos
//skaičiai.Trečioje eilut÷je užrašytas skaičius k.Toliau k eilučių, kurių kiekvienoje yra po du
//skaičius, reiškiančius keitimo intervalo ribas a ir b.Toliau mus dominančių vietų skaičius.
//Paskutin÷je eilut÷je yra surašytos vietos, kuriose esančių kortelių skaičiai mus domina.

//Rezultatai.
//Kiekvienoje ekrano eilut÷je spausdinkite du skaičius: sekos vietos numerį ir toje vietoje
//esančios kortel÷s skaičių.

//U2.txt 
//10 
//2 5 9 8 4 7 15 25 45 99 
//2 
//5 7 
//2 8 
//3 
//2 5 8 


//Ekranas
//2 25 
//5 15 
//8 5
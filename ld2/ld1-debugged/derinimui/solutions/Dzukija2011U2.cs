namespace Solutions;

using NLog;
using System.Runtime.Intrinsics.Arm;


/// <summary>
/// Solution for Metai Regionas.
/// </summary>
public class Dzukija2011U2
{
    /// <summary>
    /// A single input.
    /// </summary>
    public class Input
    {
        public int n;
        public int k;
        public int[] sk1;
        public int[] sk2;
    }

    /// <summary>
    /// A single output.
    /// </summary>
    public class Output
    {
        public int kk;
        public int zk;
        public String dp;
        public String dn;
        public String[] kopecios = { };
        public String[] zalciai = { };
    }

    /// <summary>
    /// Test data. You can use a different layout if you want.
    /// </summary>
    public class TestData
    {        
        public static Input[] Inputs {get;} = {
            //data goes here
             new Input{
              n = 6,
              k=10,
              sk1 = [3,5,12,14,15,17,18,21,31,35],
              sk2 = [16,7,2,11,25,4,20,32,19,22]
            }
        };

        public static Output[] Outputs{get;} = {
            //dayta goes here
            new Output{
              kk = 5,
             zk = 5,
             dp =  "3 - 16",
             dn =  "17 - 4" ,
             kopecios = ["3 - 16: 1 - 3","5 - 7: 1 - 2","15 - 25: 3 - 5",
                    "18 - 20: 3 - 4", "21 - 32: 4 - 6" ],
             zalciai = [ "12 - 2: 2 - 1","14 - 11: 3 - 2","17 - 4: 3 - 1",
                    "31 - 19: 6 - 4" , "35 - 22: 6 - 4"]
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
        int kopecios = 0;
        int zalciai = 0;
        int didpak = 0;
        int didnus = 0;
        String dp = null;
        String dn = null;
        String[] kop = new string[50];
        String[] zal = new string[50];
        for (int i = 0; i < input.k; i++)
        {
            if (input.sk1[i] < input.sk2[i])
            {
  
                kop[kopecios] = input.sk1[i].ToString() + " - " + input.sk2[i].ToString() + ": " 
                    + (Math.Ceiling((double)input.sk1[i] / input.n)).ToString() + " - " 
                    + (Math.Ceiling((double)input.sk2[i] / input.n)).ToString();
                kopecios++;
                if (input.sk2[i] - input.sk1[i] > didpak)
                {
                    didpak = input.sk2[i] - input.sk1[i];
                    dp = input.sk1[i].ToString() + " - " + input.sk2[i].ToString();
                }
            }
            else
            {
                zal[zalciai] = input.sk1[i].ToString() + " - " + input.sk2[i].ToString() + ": "
                    + (Math.Ceiling((double)input.sk1[i] / input.n)).ToString() + " - "
                    + (Math.Ceiling((double)input.sk2[i] / input.n)).ToString();
                zalciai++;
                if (input.sk1[i] - input.sk2[i] > didnus)
                {
                    didnus = input.sk1[i] - input.sk2[i];
                    dn = input.sk1[i].ToString() + " - " + input.sk2[i].ToString();
                }
            }
        }
        Output output = new Output();
        output.kk = kopecios;
        output.zk = zalciai;
        output.dp = dp;
        output.dn = dn;
        output.kopecios = kop;
        output.zalciai = zal;
        return output;
    }
}
//U2.Žalčiai ir kopėčios.
//Stalo žaidimas „Žalčiai ir kopėčios“ žaidžiamas kvadratinėje žaidimo lentoje, kuri padalinta
//į langelius.Langeliai yra sunumeruoti pradedant nuo vieneto, kaip parodyta paveiksle.Žaidime
//kopėčios reiškia pakilimą.Pavyzdžiui, žaidėjas, kurio žaidimo figūrėlė ėjimo pabaigoje pastatoma
//į 21-ą langelį,iš karto turi perkelti ją į 32-ą langelį. Žalčiai žaidime reiškia nusileidimą –
//ėjimą atgal iš langelio, kuriame nupiešta galva į langelį, kuriame baigiasi žalčio uodega. 
//Pavyzdžiui, jei ėjimo pabaigoje žaidėjo figūrėlė pastatoma į 35-ą langelį, ji nedelsiant
//turi būti perkelta į 22-ą langelį.
//
//Parašykite programą, kuri: 
//• suskaičiuotų, kiek žaidimo lentoje yra žalčių ir kiek kopėčių; 
//• surastų vieną didžiausią pakilimą ir vieną didžiausią nusileidimą, kurie matuojami langelių,
//žengtų pirmyn arba atgal, skaičiumi; 
//• visoms kopėčioms ir žalčiams nustatytų, iš kurios
//žaidimo lentos eilės į kurią žaidimo lentos eilę
//pakyla bei nusileidžia žaidėjo figūrėlė.

//Duomenys.
//Duomenų failo U2.txt pirmoje eilutėje parašytas žaidimo lentos kraštinės dydis langeliais
//n(3 ≤ n ≤ 20). Antroje eilutėje parašyta, kiek toliau bus duomenų(žalčių ir kopėčių kiekių suma)
//k(1 ≤ k ≤ 50). Kitose eilutėse yra žalčių arba kopėčių jungtys – skaičių poros.Pirmasis poros
//skaičius nurodo, iš kurio langelio, o antrasis – į kurį reikia pereiti.
//
//Rezultatai.
//Ekrane rezultatus spausdinkite taip, kaip parodyta pavyzdyje.Tolimesniuose paaiškinimuose
//eilučių numeriai nurodyti pagal pateiktą pavyzdį. Jeigu kopėčių (žalčių) nėra, tai pirmoje
//(antroje) eilutėje spausdinamas nulis, nespausdinama trečia (ketvirta) eilutė ir visai
//nespausdinama kopėčių (žalčių) dalis, prasidedanti žodžiu „Kopecios:“ („Zalciai:“)
//
//U2.txt
//6 
//10 
//3 16 
//5 7 
//12 2 
//14 11 
//15 25 
//17 4 
//18 20 
//21 32 
//31 19 
//35 22
//Ekranas
//Kopeciu skaicius: 5 
//Zalciu skaicius: 5 
//Didziausias pakilimas: 3 - 16 
//Didziausias nusileidimas: 17 – 4 
//Kopecios: 
//3 – 16: 1 – 3 
//5 – 7: 1 – 2 
//15 – 25: 3 – 5 
//18 – 20: 3 – 4 
//21 – 32: 4 – 6 
//Zalciai: 
//12 – 2: 2 – 1 
//14 – 11: 3 – 2 
//17 – 4: 3 – 1 
//31 – 19: 6 – 4
//35 – 22: 6 – 4
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Combinatorics.Collections;

namespace Poke_Program
{
    public static class GlobalVar{
        public static Dictionary<Type, Dictionary<Type, double>> GlobalEffectiveDict;
        public static SortedDictionary<int, Pokemon> pokemonDict;
        public static List<Pokemon> oppList;
        static GlobalVar()
        {
            GlobalEffectiveDict = new Dictionary<Type, Dictionary<Type, double>>();
            pokemonDict = new SortedDictionary<int, Pokemon>();
            oppList = new List<Pokemon>();
        }
    }
    class TypeWeakness
    {
        public Type MainType;
        public double Normal;
        public double Fire;
        public double Water;
        public double Electric;
        public double Grass;
        public double Ice;
        public double Fighting;
        public double Poison;
        public double Ground;
        public double Flying;
        public double Psychic;
        public double Bug;
        public double Rock;
        public double Ghost;
        public double Dragon;
        public double Dark;
        public double Steel;
        public double Fairy;
        public TypeWeakness(Type mainType, double normal, double fighting, double flying, double poison, double ground, double rock, double bug, double ghost, double steel, double fire, double water, double grass, double electric, double psychic, double ice, double dragon, double dark, double fairy)
        {
        this.MainType = mainType;
        this.Normal = normal;
        this.Fire = fire;
        this.Water = water;
        this.Electric = electric;
        this.Grass = grass;
        this.Ice = ice;
        this.Fighting = fighting;
        this.Poison = poison;
        this.Ground = ground;
        this.Flying = flying;
        this.Psychic = psychic;
        this.Bug = bug;
        this.Rock = rock;
        this.Ghost = ghost;
        this.Dragon = dragon;
        this.Dark = dark;
        this.Steel = steel;
        this.Fairy = fairy;
        }
    }
    public enum Stats
    {
        Attack,
        Defense,
        SpecialAttack,
        SpecialDefense,
        Speed,
        HP
    }
    public enum Type
    {
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Steel,
        Fairy,
        None
    };
    public class Move
    {
        public int Used2WinBattleCnt;
        public int Number;
        public int Accuracy;
        public int Power;
        public int AttackType;
        public int Chances;
        public int TM_num;
        public string Name;
        public int PP;
        public Type MoveType;
        public double PokeTypeMoveBoost;
        public Dictionary<Type, double> DamDict;
        public Move(int number,int accuracy,int power, int attackType,int chances,int tm_num,string name,int pp, Type moveType)
        {
            this.Number = number;
            this.Accuracy = accuracy;
            this.Power = power;
            this.AttackType = attackType;
            this.Chances = chances;
            this.TM_num = tm_num;
            this.Name = name;
            this.PP = pp;
            this.MoveType = moveType;
            this.DamDict = GlobalVar.GlobalEffectiveDict[moveType];
            this.Used2WinBattleCnt = 0;
        }
        public string GetStr(){
            string repr = this.Name + "\n" +
                "\tPower: " + this.Power.ToString() + "\n " +
                "\tAccuracy: " + this.Accuracy.ToString() + "\n " +
                "\tNumber: " + this.Number.ToString() + "\n " +
                "\tPP: " + this.PP.ToString() + "\n " +
                "\tMoveType: " + this.MoveType.ToString() + "\n\n ";
            return repr;
        }
        public void SetPokeTypeMoveBoost(Type type1,Type type2)
        {
            if (type1 == this.MoveType || type2 == this.MoveType)
            {
                this.PokeTypeMoveBoost = 1.5;
            }
            else
            {
                this.PokeTypeMoveBoost = 1;
            }
        }
    }
    public class Pokemon
    {
        public int Attack;
        public int Defense;
        public int HP;
        public string Name;
        public int Number;
        public int SpecialAttack;
        public int SpecialDefense;
        public int Speed;
        public Type Type1;
        public Type Type2;
        public List<Move> MoveList;
        public List<Move> Moves;
        public Pokemon(int attack, int defense, int hp, string name, int number, int specialAttack, int specialDefense, int speed, Type type1, Type type2)
        {
            this.Attack = attack;
            this.Defense = defense;
            this.HP = hp;
            this.Name = name;
            this.Number = number;
            this.SpecialAttack = specialAttack;
            this.SpecialDefense = specialDefense;
            this.Speed = speed;
            this.Type1 = type1;
            this.Type2 = type2;
            this.Moves = new List<Move>();
            this.MoveList = new List<Move>();
        }
    }
    class Program
    {
        public static Dictionary<string, Type> EnumDict;
        public static void CreateOppList()
        {
            foreach (KeyValuePair<int, Pokemon> oppPoke in GlobalVar.pokemonDict)
            {
                var combs = new Combinations<Move>(oppPoke.Value.MoveList,4);
                /*var query = from a in oppPoke.Value.MoveList
                            from b in oppPoke.Value.MoveList
                            from c in oppPoke.Value.MoveList
                            from d in oppPoke.Value.MoveList
                            select new List<Move>(new Move[]{a,b,c,d});*/
                foreach (var result in combs)
                {
                    Move m1 = result.ElementAt(0);
                    Move m2 = result.ElementAt(1);
                    Move m3 = result.ElementAt(2);
                    Move m4 = result.ElementAt(3);
                    if (m1.Power > 50 && m2.Power > 50 && m3.Power > 50 && m4.Power > 50)
                    {
                        Pokemon Copied = new Pokemon(oppPoke.Value.Attack, oppPoke.Value.Defense, oppPoke.Value.HP, oppPoke.Value.Name, oppPoke.Value.Number,
                        oppPoke.Value.SpecialAttack, oppPoke.Value.SpecialDefense, oppPoke.Value.Speed, oppPoke.Value.Type1, oppPoke.Value.Type2);
                        Copied.Moves.Add(m1);
                        Copied.Moves.Add(m2);
                        Copied.Moves.Add(m3);
                        Copied.Moves.Add(m4);
                        GlobalVar.oppList.Add(Copied);
                    }


                }
                Console.WriteLine(oppPoke.Value.Name + " " + GlobalVar.oppList.Count);
            }
        }
        public static void SetupEnumDict()
        {
            EnumDict = new Dictionary<string, Type>();
            EnumDict.Add("normal", Type.Normal);
            EnumDict.Add("fire", Type.Fire);
            EnumDict.Add("water", Type.Water);
            EnumDict.Add("electric", Type.Electric);
            EnumDict.Add("grass", Type.Grass);
            EnumDict.Add("ice", Type.Ice);
            EnumDict.Add("fighting", Type.Fighting);
            EnumDict.Add("poison", Type.Poison);
            EnumDict.Add("ground", Type.Ground);
            EnumDict.Add("flying", Type.Flying);
            EnumDict.Add("psychic", Type.Psychic);
            EnumDict.Add("bug", Type.Bug);
            EnumDict.Add("rock", Type.Rock);
            EnumDict.Add("ghost", Type.Ghost);
            EnumDict.Add("dragon", Type.Dragon);
            EnumDict.Add("dark", Type.Dark);
            EnumDict.Add("steel", Type.Steel);
            EnumDict.Add("fairy", Type.Fairy);
            EnumDict.Add("none", Type.None);
        }

        public static void InsertEffectiveDict(TypeWeakness tw)
        {
            Dictionary<Type, double> EffectiveDictTemp = new Dictionary<Type, double>{
                    {Type.Normal, tw.Normal},
                    {Type.Fire, tw.Fire},
                    {Type.Water, tw.Water},
                    {Type.Electric, tw.Electric},
                    {Type.Grass, tw.Grass},
                    {Type.Ice, tw.Ice},
                    {Type.Fighting, tw.Fighting},
                    {Type.Poison, tw.Poison},
                    {Type.Ground, tw.Ground},
                    {Type.Flying, tw.Flying},
                    {Type.Psychic, tw.Psychic},
                    {Type.Bug, tw.Bug},
                    {Type.Rock, tw.Rock},
                    {Type.Ghost, tw.Ghost},
                    {Type.Dragon, tw.Dragon},
                    {Type.Dark, tw.Dark},
                    {Type.Steel, tw.Steel},
                    {Type.Fairy, tw.Fairy},
                    {Type.None, 1}};
            GlobalVar.GlobalEffectiveDict.Add(tw.MainType ,  EffectiveDictTemp);
        }
        /*
Normal,1,1,1,1,1,0.5,1,0,0.5,1,1,1,1,1,1,1,1,1
Fighting,2,1,0.5,0.5,1,2,0.5,0,2,1,1,1,1,0.5,2,1,2,0.5
Flying,1,2,1,1,1,0.5,2,1,0.5,1,1,2,0.5,1,1,1,1,1
Poison,1,1,1,0.5,0.5,0.5,1,0.5,0,1,1,2,1,1,1,1,1,2
Ground,1,1,0,2,1,2,0.5,1,2,2,1,0.5,2,1,1,1,1,1
Rock,1,0.5,2,1,0.5,1,2,1,0.5,2,1,1,1,1,2,1,1,1
Bug,1,0.5,0.5,0.5,1,1,1,0.5,0.5,0.5,1,2,1,2,1,1,2,0.5
Ghost,0,1,1,1,1,1,1,2,1,1,1,1,1,2,1,1,0.5,1
Steel,1,1,1,1,1,2,1,1,0.5,0.5,0.5,1,0.5,1,2,1,1,2
Fire,1,1,1,1,1,0.5,2,1,2,0.5,0.5,2,1,1,2,0.5,1,1
Water,1,1,1,1,2,2,1,1,1,2,0.5,0.5,1,1,1,0.5,1,1
Grass,1,1,0.5,0.5,2,2,0.5,1,0.5,0.5,2,0.5,1,1,1,0.5,1,1
Electric,1,1,2,1,0,1,1,1,1,1,2,0.5,0.5,1,1,0.5,1,1
Psychic,1,2,1,2,1,1,1,1,0.5,1,1,1,1,0.5,1,1,0,1
Ice,1,1,2,1,2,1,1,1,0.5,0.5,0.5,2,1,1,0.5,2,1,1
Dragon,1,1,1,1,1,1,1,1,0.5,1,1,1,1,1,1,2,1,0
Dark,1,0.5,1,1,1,1,1,2,1,1,1,1,1,2,1,1,0.5,0.5
Fairy,1,2,1,0.5,1,1,1,1,0.5,0.5,1,1,1,1,1,2,2,1
*/
        static void addWeaknesses()
        {
            TypeWeakness TempTw = new TypeWeakness(Type.Normal, 1, 1, 1, 1, 1, 0.5, 1, 0, 0.5, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Fire, 2, 1, 0.5, 0.5, 1, 2, 0.5, 0, 2, 1, 1, 1, 1, 0.5, 2, 1, 2, 0.5);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Water, 1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 0.5, 0.5, 1, 1, 1, 0.5, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Electric, 1, 1, 2, 1, 0, 1, 1, 1, 1, 1, 2, 0.5, 0.5, 1, 1, 0.5, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Grass, 1, 1, 0.5, 0.5, 2, 2, 0.5, 1, 0.5, 0.5, 2, 0.5, 1, 1, 1, 0.5, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Ice, 1, 1, 2, 1, 2, 1, 1, 1, 0.5, 0.5, 0.5, 2, 1, 1, 0.5, 2, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Fighting, 2, 1, 0.5, 0.5, 1, 2, 0.5, 0, 2, 1, 1, 1, 1, 0.5, 2, 1, 2, 0.5);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Poison, 1, 1, 1, 0.5, 0.5, 0.5, 1, 0.5, 0, 1, 1, 2, 1, 1, 1, 1, 1, 2);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Ground, 1, 1, 0, 2, 1, 2, 0.5, 1, 2, 2, 1, 0.5, 2, 1, 1, 1, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Flying, 1, 2, 1, 1, 1, 0.5, 2, 1, 0.5, 1, 1, 2, 0.5, 1, 1, 1, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Psychic, 1, 2, 1, 2, 1, 1, 1, 1, 0.5, 1, 1, 1, 1, 0.5, 1, 1, 0, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Bug, 1, 0.5, 0.5, 0.5, 1, 1, 1, 0.5, 0.5, 0.5, 1, 2, 1, 2, 1, 1, 2, 0.5);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Rock, 1, 0.5, 2, 1, 0.5, 1, 2, 1, 0.5, 2, 1, 1, 1, 1, 2, 1, 1, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Ghost, 0, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 1);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Dragon, 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 1, 1, 1, 1, 1, 1, 2, 1, 0);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Dark, 1, 0.5, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 0.5);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Steel, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 0.5, 0.5, 1, 0.5, 1, 2, 1, 1, 2);
            InsertEffectiveDict(TempTw);
            TempTw = new TypeWeakness(Type.Fairy, 1, 2, 1, 0.5, 1, 1, 1, 1, 0.5, 0.5, 1, 1, 1, 1, 1, 2, 2, 1);
            InsertEffectiveDict(TempTw);
        }
        static void Main(string[] args)
        {
            SetupEnumDict();
            addWeaknesses();

            for (int i = 1; i <= 301; i++)
            {
                string path = @"D:\Poke_Program\Pokemon\poke_data.db";
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                m_dbConnection.Open();
                string sql = "SELECT * FROM POKEMON where number = @number";
                using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
                {
                    command.Parameters.AddWithValue("@number", i);
                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var vals = reader.GetValues();
                        int attack = Int32.Parse(reader.GetValue(0).ToString());
                        int defense = Int32.Parse(reader.GetValue(1).ToString());
                        int hP = Int32.Parse(reader.GetValue(2).ToString());
                        string name = reader.GetValue(3).ToString();
                        int number = Int32.Parse(reader.GetValue(4).ToString());
                        int specialAttack = Int32.Parse(reader.GetValue(5).ToString());
                        int specialDefense = Int32.Parse(reader.GetValue(6).ToString());
                        int speed = Int32.Parse(reader.GetValue(7).ToString());
                        Type type1 = EnumDict[reader.GetValue(8).ToString()];
                        Type type2;
                        try
                        {
                            type2 = EnumDict[reader.GetValue(9).ToString()];
                        }
                        catch (Exception e)
                        {
                            type2 = EnumDict["none"];
                        }
                        Pokemon pokemon = new Pokemon(attack+31, defense+31, hP + 31, 
                            name, number, specialAttack + 31, 
                            specialDefense + 31, speed + 31, type1, type2);
                        List<Move> MoveList = getMoveList(pokemon);
                        pokemon.MoveList =  MoveList;
                        GlobalVar.pokemonDict.Add(number,pokemon);
                    }
                }
            }
            CreateOppList();
            foreach (KeyValuePair<int, Pokemon> pokemon in GlobalVar.pokemonDict){
                List<Move> bestMoveSet = findBestMoves(pokemon.Value);
                Console.WriteLine(pokemon.Value.Name);
                foreach (Move move in bestMoveSet)
                {
                    Console.WriteLine(move.GetStr());
                }
                List<Stats> bestEvCombo = FindBestEVs(pokemon.Value, bestMoveSet);
                Console.WriteLine("Best EV Combo:\n\t" + bestEvCombo.ElementAt(0).ToString() + " " + bestEvCombo.ElementAt(1).ToString());
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static List<Stats> FindBestEVs(Pokemon pokemon, List<Move> bestMoveSet)
        {
            Console.WriteLine();
            List<Stats> bestEVCombo = null;
            int mostWins = 0;
            Stats[] types = new Stats[]{Stats.Attack,Stats.Defense,Stats.SpecialAttack,Stats.SpecialDefense,Stats.Speed,Stats.HP};
            var evs = new Combinations<Stats>(types, 2);
            foreach (var evCombo in evs)
            {
                Pokemon PokeWithEvs = new Pokemon(pokemon.Attack, pokemon.Defense, pokemon.HP, pokemon.Name, pokemon.Number,
                                        pokemon.SpecialAttack, pokemon.SpecialDefense, pokemon.Speed, pokemon.Type1, pokemon.Type2);
                PokeWithEvs.Moves = bestMoveSet;
                foreach (var move in PokeWithEvs.Moves)
                {
                    move.Used2WinBattleCnt = 0;
                }
                Stats ev1 = evCombo.ElementAt(0);
                Stats ev2 = evCombo.ElementAt(1);
                if (ev1 == Stats.Attack || ev2 == Stats.Attack)
                {
                    PokeWithEvs.Attack = PokeWithEvs.Attack + 63;
                }
                if (ev1 == Stats.Defense || ev2 == Stats.Defense)
                {
                    PokeWithEvs.Defense = PokeWithEvs.Defense + 63;
                }
                if (ev1 == Stats.SpecialAttack || ev2 == Stats.SpecialAttack)
                {
                    PokeWithEvs.SpecialAttack = PokeWithEvs.SpecialAttack + 63;
                }
                if (ev1 == Stats.SpecialDefense || ev2 == Stats.SpecialDefense)
                {
                    PokeWithEvs.SpecialDefense = PokeWithEvs.SpecialDefense + 63;
                }
                if (ev1 == Stats.Speed || ev2 == Stats.Speed)
                {
                    PokeWithEvs.Speed = PokeWithEvs.Speed + 63;
                }
                if (ev1 == Stats.HP || ev2 == Stats.HP)
                {
                    PokeWithEvs.HP = PokeWithEvs.HP + 63;
                }
                foreach (Pokemon opponent in GlobalVar.oppList)
                {
                    Battle(PokeWithEvs, opponent);
                }
                int wins = PokeWithEvs.Moves.Sum(item => item.Used2WinBattleCnt);
                double percentWins = (double)wins / (double)GlobalVar.oppList.Count;
                Console.WriteLine(ev1.ToString() + " " + ev2.ToString() + " " + percentWins.ToString());
                if (wins > mostWins) {
                    bestEVCombo = new List<Stats>(new Stats[] { ev1, ev2 });
                    mostWins = wins;
                }
            }
            return bestEVCombo;
        }
        public static List<Move> findBestMoves(Pokemon pokemon)
        {
            //1
            pokemon.Moves.Add(pokemon.MoveList.ElementAt(0));
            pokemon.MoveList.Remove(pokemon.MoveList.ElementAt(0));
            //2
            pokemon.Moves.Add(pokemon.MoveList.ElementAt(0));
            pokemon.MoveList.Remove(pokemon.MoveList.ElementAt(0));
            //3
            pokemon.Moves.Add(pokemon.MoveList.ElementAt(0));
            pokemon.MoveList.Remove(pokemon.MoveList.ElementAt(0));
            //4
            pokemon.Moves.Add(pokemon.MoveList.ElementAt(0));
            pokemon.MoveList.Remove(pokemon.MoveList.ElementAt(0));
            while (pokemon.MoveList.Count > 0 || pokemon.Moves.Count >= 5)
            {
                if (pokemon.MoveList.Count > 0)
                {
                    //5
                    pokemon.Moves.Add(pokemon.MoveList.ElementAt(0));
                    pokemon.MoveList.Remove(pokemon.MoveList.ElementAt(0));
                }
                foreach (Move move in pokemon.Moves)
                {
                    move.Used2WinBattleCnt = 0;
                }
                pokemon.Moves.Remove(GetLeastUsedMove(pokemon));
            }
            return pokemon.Moves;
        }
        public static List<Move> getMoveList(Pokemon pokemon)
        {
            int pokedexNumber = pokemon.Number;
            List<Move> list = new List<Move>();
            string path = @"D:\Poke_Program\Pokemon\poke_data.db";
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            m_dbConnection.Open();
            string sql = "SELECT * FROM MOVES where pokemon_fk = @number and ACCURACY > 0 AND ATTACK > 0";
            using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
            {
                command.Parameters.AddWithValue("@number", pokedexNumber);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var vars = reader.GetValues();
                    int id = Int32.Parse(reader.GetValue(0).ToString());
                    int accuracy = Int32.Parse(reader.GetValue(1).ToString());
                    int attack = Int32.Parse(reader.GetValue(2).ToString());
                    int cat = Int32.Parse(reader.GetValue(3).ToString());
                    int effect = Int32.Parse(reader.GetValue(4).ToString());
                    int level = Int32.Parse(reader.GetValue(5).ToString());
                    string name = reader.GetValue(6).ToString();
                    int pp = Int32.Parse(reader.GetValue(7).ToString());
                    Type type = EnumDict[reader.GetValue(8).ToString()];
                    int pokemon_fk = Int32.Parse(reader.GetValue(9).ToString());

                    Move move = new Move(id,accuracy,attack,cat,effect,level,name,pp,type);
                    move.SetPokeTypeMoveBoost(pokemon.Type1,pokemon.Type2);
                    list.Add(move);
                }
            }
            List<Move> junkMoves = FillEmptyMoveList();
            foreach (Move junk in junkMoves)
            {
                list.Add(junk);
            }
            return list;
        }
        public static List<Move> FillEmptyMoveList()
        {
            List<Move> list = new List<Move>();
            Move EmptyMove = new Move(-1, 1, 0, 0, 1, -1, "Filler Move", 40, Type.Normal);
            list.Add(EmptyMove);
            EmptyMove = new Move(-1, 1, 0, 0, 1, -1, "Filler Move", 40, Type.Normal);
            list.Add(EmptyMove);
            EmptyMove = new Move(-1, 1, 0, 0, 1, -1, "Filler Move", 40, Type.Normal);
            list.Add(EmptyMove);
            EmptyMove = new Move(-1, 1, 0, 0, 1, -1, "Filler Move", 40, Type.Normal);
            list.Add(EmptyMove);
            return list;
        }
        /// <summary>
        /// Returns worst move in the Pokemon's Moves List
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public static Move BattleManager(Pokemon pokemon)
        {
            foreach (Pokemon opponent in GlobalVar.oppList)
            {
                Battle(pokemon, opponent);
            }
            Move bestMove = pokemon.Moves.OrderBy(move => move.Used2WinBattleCnt).Last();
            Move leastUsedMove = pokemon.Moves.OrderBy(move => move.Used2WinBattleCnt).First();
            return leastUsedMove;
        }
        public static void Battle(Pokemon p1, Pokemon p2){
            Tuple<Move, double> bestP1MoveTuple = GetMostDamaginMove(p1, p2.Type1, p2.Type2);
            Tuple<Move, double> bestP2MoveTuple = GetMostDamaginMove(p2, p1.Type1, p1.Type2);
            Move bestP1Move = bestP1MoveTuple.Item1;
            double p1MoveDamage = bestP1MoveTuple.Item2;
            Move bestP2Move = bestP2MoveTuple.Item1;
            double p2MoveDamage = bestP2MoveTuple.Item2;

            int time1 = GetNumberTurns2Knockout(p2, p1MoveDamage);
            int time2 = GetNumberTurns2Knockout(p1, p2MoveDamage);
            //int winner;
            if (p1.Speed >= p2.Speed)
            {
                if (time1 <= time2)
                {
                    bestP1Move.Used2WinBattleCnt += 1;
                    //winner = 1;
                }
                else
                {
                    //Pokemon 2 won
                    //winner = 2;
                }
            }
            else
            {
                if (time2 <= time1)
                {
                    //Pokemon 2 won
                    //winner = 2;
                }
                else
                {
                    bestP1Move.Used2WinBattleCnt += 1;
                    //winner = 1;
                }
            }

        }
        public static Move GetLeastUsedMove(Pokemon pokemon)
        {
            return BattleManager(pokemon);
        }
        public static Tuple<Move,double> GetMostDamaginMove(Pokemon pokemon,Type p2Type1,Type p2Type2)
        {
            Dictionary<double, Move> moveDamDict = new Dictionary<double, Move>();
            foreach (Move move in pokemon.Moves)
            {
                double damage = CalcDamage(move,pokemon,p2Type1,p2Type2);
                moveDamDict[damage] = move;
            }
            double mostDamageKey = moveDamDict.Keys.Max();
            Move mostDamagingMove = moveDamDict[mostDamageKey];
            return Tuple.Create(mostDamagingMove, mostDamageKey);
        }
        public static double CalcDamage(Move move,Pokemon pokemon, Type oppType1,Type oppType2, int Level = 50,double randomFactor = 0.92){
            double damFactor1 = move.DamDict[oppType1];
            double damFactor2 = move.DamDict[oppType2];
            double damage = ((Convert.ToDouble(2 * Level + 10) / 250.0) * (pokemon.Attack / pokemon.Defense) * move.Power + 2.0) * move.PokeTypeMoveBoost * damFactor1 * damFactor2 * randomFactor;
            return damage;
        }
        public static int GetNumberTurns2Knockout(Pokemon pokemon,double damage)
        {
            if (damage == 0)
            {
                return 0;
            }
            else
            {
                return  (Convert.ToInt32(Convert.ToDouble(pokemon.HP) / Convert.ToDouble(Math.Ceiling(damage))) + 1);
            }
        }
    }
}

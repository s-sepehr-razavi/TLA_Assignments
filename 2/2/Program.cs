using System;
using System.Collections.Generic;
using System.Collections;
namespace _2
{

    class State: IComparable<State>
    {
        public string name;
        public Dictionary<string, List<State>> transitions = new Dictionary<string, List<State>>();
        public bool is_it_final = false;

        public State(string name, bool is_it_final = false)
        {
            this.name = name;
            this.is_it_final = is_it_final;
        }

        public int CompareTo(State other)
        {
            return this.name.CompareTo(other.name);
        }
    }
    internal class Program
    {

        
        static void Main(string[] args)
        {
            Dictionary<string, State> map = new Dictionary<string, State>();
            string[] keys = Console.ReadLine().Trim('{', '}').Split(',');
            string[] alphabet = Console.ReadLine().Trim('{', '}').Split(',');
            string[] final_states = Console.ReadLine().Trim('{', '}').Split(',');
            int n = int.Parse(Console.ReadLine());
            //Console.WriteLine(keys.Length);
            foreach (string key in keys)
            {
                //Console.WriteLine(key);
                map.Add(key, new State(key));
            }

            foreach (var item in map.Values)
            {
                foreach (string letter in alphabet)
                {
                    item.transitions.Add(letter, new List<State>());
                }
                item.transitions.Add("$", new List<State>());
            }

            string[] transition;
            for (int i = 0; i < n; i++)
            {
                transition = Console.ReadLine().Split(',');
                map[transition[0]].transitions[transition[1]].Add(map[transition[2]]);
            }

            foreach (var item in final_states)
            {
                map[item].is_it_final = true;
            }
            
            State current_state = map[keys[0]];
            List<List<State>> states = new List<List<State>>();

            func(ep_closure(new List<State>() { current_state }), states, alphabet);
            Console.WriteLine(states.Count);
            //Console.WriteLine(are_they_same(new List<State>(), new List<State>()));


        }

        public static bool are_they_same(List<State> current, List<State> other)
        {
            if(current.Count != other.Count)
                return false;
            for (int i = 0; i < current.Count; i++)
            {
                if (current[i].name != other[i].name)
                {
                    return false;
                }
            }
            return true;
        }
        public static void func(List<State> current, List<List<State>> new_states, string[] alphabet)
        {
            bool same = false;
            foreach (var item in new_states)
            {
                if (are_they_same(current, item))
                {
                    same = true;
                    break;
                }
            }

            if (same)
            {
                return;     
            }
            else
            {
                new_states.Add(current);
                //Console.WriteLine("====================================");
                //Console.WriteLine(new_states.Count);
                //foreach (var item in current)
                //{
                //    Console.WriteLine(item.name);
                //}
                //Console.WriteLine($"number of elements: {current.Count}");
            }

            foreach (var letter in alphabet)
            {
                List<State> new_ = new List<State>(); // should make a set
                HashSet<State> old_ = new HashSet<State>();
                foreach (var state in current) 
                {
                    List<State> tran = state.transitions[letter];
                    foreach (var x in tran)
                    {
                        //new_.Add(x);
                        old_.Add(x);
                    }
                }
                foreach (var item in old_)
                {
                    new_.Add(item);
                }
                func(ep_closure(new_), new_states, alphabet);
            }
        }
        public static List<State> ep_closure(List<State> states) // it should be recursive
        {
            List<State> result = new List<State>();
            for (int i = 0; i < states.Count; i++)
            {
                result.Add(states[i]);
                List<State> list = states[i].transitions["$"];
                foreach (var item in list)
                {
                    result.Add(item);
                }
            }
            result.Sort();
            return result;
        }

    }
}
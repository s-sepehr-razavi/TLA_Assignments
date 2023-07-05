using System;
using System.Collections.Generic;
namespace _1
{

    class State
    {
        public string name;
        public Dictionary<string, List<State>> transitions = new Dictionary<string, List<State>>();
        public bool is_it_final = false;

        public State(string name, bool is_it_final = false) 
        {
            this.name = name;
            this.is_it_final = is_it_final;
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

            string s = Console.ReadLine();
            State current_state = map[keys[0]];
            //List<State> states_to_visit;

            //for (int i = 0; i < s.Length; i++)
            //{
            //    states_to_visit = current_state.transitions[s[i] + ""];           

            //}
            bool f = false;

            check(current_state, 0, s, ref f);
            //non_rec_check(current_state, s, ref f);
            if (f)
            {
                Console.Write("Accepted");
            }
            else
            {
                Console.Write("Rejected");
            }
        }

        static void check(State current_state, int i, string s,ref bool flag) // lambda cycle
        {
            //Console.WriteLine(current_state.name);
            if (i == s.Length)
            {
                if (current_state.is_it_final)
                {
                    flag = true;
                }
                //return false;
                    return;
            }

            List<State> n_states;
            List<State> l_states;


            try
            {

                n_states = current_state.transitions[s[i] + ""];
                l_states = current_state.transitions["$"];

            }
            catch (Exception)
            {
                return;
                
            }
            foreach (var state in n_states)
            {
                check(state, i + 1, s,ref flag);
            }

            foreach (var state in l_states)
            {
                //Console.WriteLine("s");
                check(state, i, s, ref flag);
            }
            
        }

        static void non_rec_check(State current_state, string s, ref bool flag)
        {
            Stack<Tuple<State, int>> stack = new Stack<Tuple<State, int>>();
            if(current_state == null || s == null || s == "")
            {
                return;
            }
            
            bool fin = false;
            int index = 0;
            int counter = 0;
            while (true)
            {
                counter++;
                //if (counter > 0)
                //    return;
                while (index == s.Length)
                {
                    if(current_state.is_it_final)
                    {
                        flag = true;
                    }
                    if (stack.Count == 0)
                    {
                        fin = true;
                        break;
                    }
                    Tuple<State, int> next = stack.Pop();
                    current_state = next.Item1;
                    index = next.Item2;
                }
                if (fin)
                {
                    break;
                }
                List<State> n_states = current_state.transitions[s[index] + ""];
                List<State> l_states = current_state.transitions["$"];

                foreach (var state in n_states)
                {
                    stack.Push(new Tuple<State, int>(state, index + 1));
                }

                foreach (var state in l_states) // it doesn't have a lambda cycle
                {

                    stack.Push(new Tuple<State, int>(state, index));
                }
                if (stack.Count == 0)
                {
                    break;
                }
                Tuple<State, int> last_one = stack.Pop();
                current_state = last_one.Item1;
                index = last_one.Item2;
            }

        }


    }
}